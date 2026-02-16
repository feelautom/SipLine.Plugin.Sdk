namespace SipLine.Plugin.Sdk.Models
{
    /// <summary>
    /// Represents a single result returned by a plugin search provider.
    /// </summary>
    public class PluginSearchResult
    {
        /// <summary>Unique ID from the provider.</summary>
        public string Id { get; set; } = "";

        /// <summary>Main title of the result (e.g., Contact Name).</summary>
        public string Title { get; set; } = "";

        /// <summary>Sub-title or description (e.g., Company or Number).</summary>
        public string? Subtitle { get; set; }

        /// <summary>Phone number associated with the result (if dialable).</summary>
        public string? PhoneNumber { get; set; }

        /// <summary>Icon to display alongside the result (enum or SVG path).</summary>
        public string? Icon { get; set; }

        /// <summary>Type of result (Contact, Company, Ticket, etc.).</summary>
        public string? ResultType { get; set; } = "Contact";
    }
}
