using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Controllers
{
    /// <summary>
    /// Class encapsulating search controller
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Controller" />
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        /// <summary>
        /// The search service
        /// </summary>
        private readonly ISearchService _searchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchController"/> class.
        /// </summary>
        /// <param name="searchService">The search service.</param>
        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        // GET api/search/status/SessionID
        [HttpGet("status/{sessionId}")]
        public IEnumerable<SearchResult> Status(string sessionId)
        {
          return  _searchService.GetStatus(sessionId);
        }

        // POST api/search/pause/SessionID
        [HttpPost("pause/{sessionId}")]
        public void Pause(string sessionId)
        {
            _searchService.Pause(sessionId);
        }

        // POST api/search/resume/SessionID
        [HttpPost("resume/{sessionId}")]
        public void Resume(string sessionId)
        {
            _searchService.Resume(sessionId);
        }

        // GET api/search/start
        //{
        //  "rootUrl": "https://nih.gov/sem/mauris/laoreet/ut/12345",
        //  "maxThread": 1,
        //  "maxUrls": 1,
        //  "phrase": "Vestibulum quam sapien, varius ut, blandit non, interdum in, ante."
        //}
        [HttpPost("start")]
        public string Start([FromBody] SearchParams searchParams)
        {
            return _searchService.Start(searchParams);
        }
        
        // POST api/search/stop/SessionID
        [HttpPost("stop/{sessionId}")]
        public void Stop(string sessionId)
        {
            _searchService.Stop(sessionId);
        }
    }
}
