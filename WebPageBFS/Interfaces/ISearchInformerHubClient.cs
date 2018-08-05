using System;
using System.Threading.Tasks;

using WebPageBFS.Models;

namespace WebPageBFS.Interfaces
{
    /// <summary>
    /// Interface descibing search informer client
    /// </summary>
    public interface ISearchInformerHubClient: IDisposable
    {
        /// <summary>
        /// Informs the specified action name.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="searchResults">The search results.</param>
        /// <returns>The awaiter.</returns>
        Task Inform(string actionName, string sessionId, SearchResult searchResults);

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns>The awaiter.</returns>
        Task Start();

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>The awaiter.</returns>
        Task Stop();
    }
}
