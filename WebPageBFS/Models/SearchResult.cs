using System.Collections.Generic;

namespace WebPageBFS.Models
{
    /// <summary>
    /// Class describing search result
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        public SearchStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the details.
        /// </summary>
        public string Details { get; set; }

        /// <summary>
        /// Gets or sets the urls.
        /// </summary>
        public ICollection<string> Urls { get; set; }
    }
}
