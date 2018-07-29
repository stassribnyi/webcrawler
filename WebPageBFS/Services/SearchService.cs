using System;
using System.Collections.Generic;
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
    }

    #region nested
    class SearchSession
    {
        public IEnumerable<SearchResult> CurrentStatus { get; set; }

        public IManualParallel ParallelService { get; set; }
    }
    #endregion
}
