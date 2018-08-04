using System.Threading.Tasks;

using WebPageBFS.Models;

namespace WebPageBFS.Interfaces
{
    /// <summary>
    /// Interface describing page analyze service
    /// </summary>
    public interface IPageAnalyzeService
    {
        /// <summary>
        /// Analyzes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="text">The text.</param>
        /// <returns>The details of search.</returns>
        Task<SearchResult> Analyze(string url, string text);
    }
}
