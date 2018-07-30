using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

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
        /// The sessions
        /// </summary>
        private readonly Dictionary<string, SearchSession> _sessions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchService"/> class.
        /// </summary>
        public SearchService()
        {
            _sessions = new Dictionary<string, SearchSession>();
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
            if (!_sessions.ContainsKey(sessionId))
            {
                return Array.Empty<SearchResult>();
            }

            return _sessions[sessionId].CurrentStatus;
        }

        /// <summary>
        /// Pauses the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Pause(string sessionId)
        {
            if (!_sessions.ContainsKey(sessionId))
            {
                return;
            }

            _sessions[sessionId].ParallelService.Pause();
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

            _sessions.Add(sessionId, new SearchSession
            {
                CurrentStatus = new List<SearchResult>
                {
                    new SearchResult
                    {
                        Url = searchParams.RootUrl,
                        Status = SearchStatus.Pending,
                        Details = SearchStatus.Pending.ToString()
                    }
                },
                ParallelService = new ManualParallel
                (
                    new List<Action> { () => LoadPage(searchParams.RootUrl) },
                    new ParallelOptions { MaxDegreeOfParallelism = searchParams.MaxThread }
                )
            });

            return sessionId;
        }

        /// <summary>
        /// Stops the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        public void Stop(string sessionId)
        {
            if (!_sessions.ContainsKey(sessionId))
            {
                return;
            }

            _sessions[sessionId].ParallelService.Stop();
            _sessions.Remove(sessionId);
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
        /// Loads the page.
        /// </summary>
        /// <param name="url">The URL.</param>
        private void LoadPage(string url)
        {
            var result = "";
            try
            {
                // load page
                // analyze
                // if exists external links 
                // check if some already have been processed
                // run LoadPage for each 

            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
        }

        /// <summary>
        /// Searches the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="text">The text.</param>
        /// <returns>The details of search.</returns>
        private async Task<SearchResult> Search(string url, string text)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.GetAsync(url);
                var pageContents = await response.Content.ReadAsStringAsync();

                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var pageNode = pageDocument.DocumentNode;

                var nodesContainingSearchText = pageNode.SelectNodes($"//*[contains(., '{text}')]");
                var searchResult = nodesContainingSearchText != null
                        ? nodesContainingSearchText.Count()
                        : 0;

                var nodesContainingLinks = pageNode.SelectNodes("//a[@href]");
                var linkResult = nodesContainingLinks == null
                        ? new List<string>()
                        : nodesContainingLinks.Select(x => x.Attributes["href"].Value).ToList();

                const string linkValidationRegex = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                linkResult = linkResult.Where(
                    l => Regex.IsMatch(l, linkValidationRegex))
                    .ToList();

                return new SearchResult
                {
                    Details = searchResult > 0 ? $"Found: ({searchResult})" : "Not found",
                    Status = searchResult > 0 ? SearchStatus.Found : SearchStatus.NotFound,
                    Url = url,
                    Urls = linkResult.Distinct().ToList()
                };
            }
            catch (Exception ex)
            {
                return new SearchResult
                {
                    Details = ex.ToString(),
                    Status = SearchStatus.Error,
                    Url = url,
                    Urls = new List<string>()
                };
            }
        }
    }

    #region nested
    class SearchSession
    {
        public IEnumerable<SearchResult> CurrentStatus { get; set; }

        public IManualParallel ParallelService { get; set; }
    }
    #endregion
}
