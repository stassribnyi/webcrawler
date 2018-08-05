using System;

namespace WebPageBFS.Models
{
    /// <summary>
    /// Class encapsulating search event args
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SearchEventArgs: EventArgs
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        public SearchResult Value { get; private set; }

        /// <summary>
        /// Gets the session identifier.
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchEventArgs"/> class.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <param name="value">The value.</param>
        public SearchEventArgs(string sessionId, SearchResult value)
        {
            SessionId = sessionId;
            Value = value;
        }
    }
}
