using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Services
{
    /// <summary>
    /// Class encapsulating implementation of ISearchService
    /// </summary>
    /// <seealso cref="WebPageBFS.Interfaces.ISearchService" />
    public class SearchService : ISearchService
    {
        /// <summary>
        /// The page analyze service
        /// </summary>
        private readonly IPageAnalyzeService _pageAnalyzeService;

        /// <summary>
        /// The search informer hub client
        /// </summary>
        private readonly ISearchInformerHubClient _searchInformerHubClient;


        /// <summary>
        /// The sessions
        /// </summary>
        private readonly ConcurrentDictionary<string, SearchSession> _sessions;

        /// <summary>
        /// Occurs when [add or update].
        /// </summary>
        public event EventHandler<SearchEventArgs> AddOrUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchService"/> class.
        /// </summary>
        /// <param name="pageAnalyzeService">The page analyze service.</param>
        /// <param name="searchInformerHubClient">The search informer hub client.</param>
        public SearchService(
            IPageAnalyzeService pageAnalyzeService,
            ISearchInformerHubClient searchInformerHubClient)
        {
            _pageAnalyzeService = pageAnalyzeService;
            _searchInformerHubClient = searchInformerHubClient;
            _sessions = new ConcurrentDictionary<string, SearchSession>();
        }

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        /// The array of search results.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<SearchResult> GetStatus(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out SearchSession result))
            {
                return Array.Empty<SearchResult>();
            }

            return result.CurrentStatus.Select(x => x.Value).ToArray();
        }

        /// <summary>
        /// Pauses the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Pause(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out SearchSession result))
            {
                return;
            }

            result.ParallelService.Pause();
        }

        /// <summary>
        /// Resumes the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Resume(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out SearchSession result))
            {
                return;
            }

            result.ParallelService.Start();
        }

        /// <summary>
        /// Starts the specified search parameters.
        /// </summary>
        /// <param name="searchParams">The search parameters.</param>
        /// <returns>
        /// Session identifier.
        /// </returns>
        public string Start(SearchParams searchParams)
        {
            var sessionId = GenerateSessionId();

            var manualParralel = new ManualParallel
                (
                    new List<Action> { () => AnalyzePage(sessionId, searchParams).Wait() },
                    new ParallelOptions { MaxDegreeOfParallelism = searchParams.MaxThreads }
                );

            _sessions.TryAdd(sessionId, new SearchSession
            {
                CurrentStatus = new ConcurrentDictionary<string, SearchResult>(),
                ParallelService = manualParralel
            });

            manualParralel.Start();

            return sessionId;
        }

        /// <summary>
        /// Stops the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Stop(string sessionId)
        {
            if (!_sessions.TryGetValue(sessionId, out SearchSession result))
            {
                return;
            }

            result.ParallelService.Stop();
        }

        /// <summary>
        /// Generates the session identifier.
        /// </summary>
        /// <returns>The session id.</returns>
        private string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// Analyzes the page.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="searchParams">The search parameters.</param>
        /// <returns>The awaiter.</returns>
        private async Task AnalyzePage(string sessionId, SearchParams searchParams)
        {
            try
            {
                await _searchInformerHubClient.Start();

                var rootUrl = searchParams.RootUrl;
                var phrase = searchParams.Phrase;

                TryAddOrUpdate(sessionId, new SearchResult
                {
                    Url = rootUrl,
                    Status = SearchStatus.Pending,
                    Details = SearchStatus.Pending.ToString()
                });

                var root = await _pageAnalyzeService.Analyze(rootUrl, phrase);

                var queue = new ConcurrentQueue<SearchResult>();
                var saved = new ConcurrentDictionary<string, SearchResult>();

                queue.Enqueue(root);
                saved.TryAdd(root.Url, root);

                while (queue.Count > 0)
                {
                    if (!queue.TryDequeue(out SearchResult page) || !TryAddOrUpdate(sessionId, page))
                    {
                        continue;
                    }

                    var urlsToProceed = page.Urls.Where(nestedUrl => !saved.Any(x => x.Key == nestedUrl)).ToArray();

                    var actions = urlsToProceed.Select((nestedUrl) => new Action(() =>
                    {
                        var pageToAdd = new SearchResult
                        {
                            Url = nestedUrl,
                            Status = SearchStatus.Pending,
                            Details = SearchStatus.Pending.ToString()
                        };

                        if (saved.Count < searchParams.MaxUrls && TryAddOrUpdate(sessionId, pageToAdd))
                        {
                            var nestedPage = _pageAnalyzeService.Analyze(nestedUrl, phrase).Result;

                            queue.Enqueue(nestedPage);
                            saved.TryAdd(nestedPage.Url, nestedPage);
                        }

                    })).ToList();

                    if (!actions.Any())
                    {
                        continue;
                    }

                    var session = _sessions[sessionId];

                    session.ParallelService = new ManualParallel(actions,
                        new ParallelOptions { MaxDegreeOfParallelism = searchParams.MaxThreads });

                    session.ParallelService.Start().Wait();
                }
            }
            catch (Exception ex)
            {
                // TODO implement general error handling
                throw ex;
            }
            finally
            {
                await _searchInformerHubClient.Inform("Stoped", sessionId);
                await _searchInformerHubClient.Stop();
            }
        }

        /// <summary>
        /// Tries the add or update.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="page">The page.</param>
        /// <returns>The status of attempt.</returns>
        private bool TryAddOrUpdate(string sessionId, SearchResult page)
        {
            if (!_sessions.TryGetValue(sessionId, out SearchSession session))
            {
                return false;
            }

            if (session.CurrentStatus.TryGetValue(page.Url, out SearchResult pageToUpdate))
            {
                pageToUpdate.Urls = page.Urls;
                pageToUpdate.Status = page.Status;
                pageToUpdate.Details = page.Details;
            }
            else if (!session.CurrentStatus.TryAdd(page.Url, page))
            {
                return false;
            }

            AddOrUpdate?.Invoke(this, new SearchEventArgs(sessionId, page));
            _searchInformerHubClient.Inform("Changed", sessionId, page);

            return true;
        }
    }

    #region nested
    class SearchSession
    {
        public ConcurrentDictionary<string, SearchResult> CurrentStatus { get; set; }

        public IManualParallel ParallelService { get; set; }
    }
    #endregion
}
