using System;
using System.Collections.Generic;

using WebPageBFS.Models;

namespace WebPageBFS.Interfaces
{
    /// <summary>
    /// Interface describing search service
    /// </summary>
    public interface ISearchService
    {
        /// <summary>
        /// Occurs when [add or update].
        /// </summary>
        event EventHandler<SearchEventArgs> AddOrUpdate;

        /// <summary>
        /// Gets the status.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The array of search results.</returns>
        IEnumerable<SearchResult> GetStatus(string sessionId);

        /// <summary>
        /// Pauses the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        void Pause(string sessionId);

        /// <summary>
        /// Resumes the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        void Resume(string sessionId);

        /// <summary>
        /// Starts the specified search parameters.
        /// </summary>
        /// <param name="searchParams">The search parameters.</param>
        /// <returns>Session identifier.</returns>
        string Start(SearchParams searchParams);

        /// <summary>
        /// Stops the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        void Stop(string sessionId);
    }
}
