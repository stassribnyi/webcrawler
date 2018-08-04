
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using WebPageBFS.Interfaces;

namespace WebPageBFS.Controllers
{
    /// <summary>
    /// Class encapsulating search signalR controller
    /// </summary>
    public class SearchSignalRController : Hub
    {
        private ISearchService _searchService;
        public SearchSignalRController(ISearchService searchService)
        {
            _searchService = searchService;

            // subscribe for updates
            // _searchService.Subscribe((event) => this.Inform(event.sessionId));
        }

        public Task Inform(string sessionId)
        {
            return Clients.Clients(sessionId).SendAsync("Changed");
        }
    }
}
