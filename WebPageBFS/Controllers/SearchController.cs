using Microsoft.AspNetCore.Mvc;

namespace WebPageBFS.Controllers
{
    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        // GET api/search/start/https%3A%2F%2Fexample.com%2Fpath%2Fto%2Froot
        [HttpGet("start/{rootUrl}")]
        public string Start(string rootUrl)
        {
            return "SessionID";
        }

        // POST api/search/pause/SessionID
        [HttpPost("pause/{sessionId}")]
        public void Pause(string sessionId)
        {
        }

        // DELETE api/search/stop/SessionID
        [HttpDelete("stop/{sessionId}")]
        public void Stop(string sessionId)
        {
        }
    }
}
