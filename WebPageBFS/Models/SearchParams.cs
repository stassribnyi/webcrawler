namespace WebPageBFS.Models
{
    /// <summary>
    /// Class descibing search parameters
    /// </summary>
    public class SearchParams
    {
        /// <summary>
        /// Gets or sets the maximum thread.
        /// </summary>
        public int MaxThread { get; set; }

        /// <summary>
        /// Gets or sets the maximum urls.
        /// </summary>
        public int MaxUrls { get; set; }

        /// <summary>
        /// Gets or sets the root URL.
        /// </summary>
        public string RootUrl { get; set; }

        /// <summary>
        /// Gets or sets the phrase.
        /// </summary>
        public string Phrase { get; set; }
    }
}
