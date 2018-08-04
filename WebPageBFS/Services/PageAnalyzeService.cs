using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using HtmlAgilityPack;

using WebPageBFS.Interfaces;
using WebPageBFS.Models;

namespace WebPageBFS.Services
{
    /// <summary>
    /// Class encapsulating page analyzer interface
    /// </summary>
    /// <seealso cref="WebPageBFS.Interfaces.IPageAnalyzeService" />
    public class PageAnalyzeService : IPageAnalyzeService
    {
        /// <summary>
        /// Analyzes the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="text">The text.</param>
        /// <returns>
        /// The details of search.
        /// </returns>
        public async Task<SearchResult> Analyze(string url, string text)
        {
            try
            {
                var client = new HttpClient();

                var response = await client.GetAsync(url);
                var pageContents = await response.Content.ReadAsStringAsync();

                var pageDocument = new HtmlDocument();
                pageDocument.LoadHtml(pageContents);

                var pageNode = pageDocument.DocumentNode;

                var nodesContainingSearchText = pageNode.SelectNodes($"//*[contains(., '{text}')]");
                var searchResult = nodesContainingSearchText != null
                        ? nodesContainingSearchText.Count()
                        : 0;

                var nodesContainingLinks = pageNode.SelectNodes("//a[@href]");
                var linkResult = nodesContainingLinks == null
                        ? new List<string>()
                        : nodesContainingLinks.Select(x => x.Attributes["href"].Value).ToList();

                const string linkValidationRegex = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
                linkResult = linkResult.Where(
                    l => Regex.IsMatch(l, linkValidationRegex))
                    .ToList();

                return new SearchResult
                {
                    Details = searchResult > 0 ? $"Found: ({searchResult})" : "Not found",
                    Status = searchResult > 0 ? SearchStatus.Found : SearchStatus.NotFound,
                    Url = url,
                    Urls = linkResult.Distinct().ToList()
                };
            }
            catch (Exception ex)
            {
                return new SearchResult
                {
                    Details = ex.ToString(),
                    Status = SearchStatus.Error,
                    Url = url,
                    Urls = new List<string>()
                };
            }
        }
    }
}
