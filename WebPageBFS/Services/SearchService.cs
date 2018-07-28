using System.Collections.Generic;

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
        /// Gets the status.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>
        /// The array of search results.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IEnumerable<SearchResult> GetStatus(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Pauses the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Pause(string sessionId)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Starts the specified search parameters.
        /// </summary>
        /// <param name="searchParams">The search parameters.</param>
        /// <returns>
        /// Session identifier.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public string Start(SearchParams searchParams)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Stops the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Stop(string sessionId)
        {
            throw new System.NotImplementedException();
        }
    }
}
