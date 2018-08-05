
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;

namespace WebPageBFS.Controllers
{
    /// <summary>
    /// Class encapsulating search signalR controller
    /// </summary>
    public class SearchInformer : Hub
    {
        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchInformer"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        public SearchInformer(ISearchService searchService)
        {
            _searchService = searchService;

            // subscribe for updates
            // _searchService.Subscribe((event) => this.Inform(event.sessionId));
        }

        /// <summary>
        /// Informs the specified session identifier.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>The awaiter.</returns>
        public Task Inform(string sessionId)
        {
            return Clients.All.SendAsync("Changed", sessionId);
        }
    }
}
