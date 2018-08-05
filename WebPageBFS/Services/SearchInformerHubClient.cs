using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Services
{
    /// <summary>
    /// Class encapsulating search informer hub client
    /// </summary>
    /// <seealso cref="WebPageBFS.Interfaces.ISearchInformerHubClient" />
    public class SearchInformerHubClient : ISearchInformerHubClient
    {
        /// <summary>
        /// The connection
        /// </summary>
        private readonly HubConnection _connection;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchInformerHubClient"/> class.
        /// </summary>
        public SearchInformerHubClient(IHttpContextAccessor contextAccessor)
        {
            var baseUrl = GetBaseUrl(contextAccessor.HttpContext);

            _connection = new HubConnectionBuilder()
            .WithUrl($"{baseUrl}/searchinformer")
            .Build();

            _connection.Closed += (ex) => _connection.StartAsync();
        }

        /// <summary>
        /// Informs the specified action name.
        /// </summary>
        /// <param name="actionName">Name of the action.</param>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="searchResults">The search results.</param>
        /// <returns>
        /// The awaiter.
        /// </returns>
        public async Task Inform(string actionName, string sessionId, SearchResult searchResults)
        {
            await _connection.InvokeAsync("Inform", actionName, sessionId, searchResults);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns>
        /// The awaiter.
        /// </returns>
        public async Task Start()
        {
            await _connection.StartAsync();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        /// <returns>
        /// The awaiter.
        /// </returns>
        public async Task Stop()
        {
            await _connection.StopAsync();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public async void Dispose()
        {
            await _connection.DisposeAsync();
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        /// <param name="currentContext">The current context.</param>
        /// <returns>The base url.</returns>
        private string GetBaseUrl(HttpContext currentContext)
        {
            var request = currentContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }
    }
}
