
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Controllers
{
    /// <summary>
    /// Class encapsulating search signalR controller
    /// </summary>
    public class SearchInformer : Hub
    {
        /// <summary>
        /// Informs the specified action name.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="searchResults">The search results.</param>
        /// <returns>The awaiter.</returns>
        public async Task Inform(string actionName, string sessionId, SearchResult searchResults)
        {
            await Clients.All.SendAsync(actionName, sessionId, searchResults);
        }
    }
}
