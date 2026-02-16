namespace SipLine.Plugin.Sdk.Models
{
    /// <summary>
    /// Represents a contact shared between a plugin and SipLine.
    /// </summary>
    public class PluginContact
    {
        /// <summary>Unique identifier for the contact (External ID).</summary>
        public string Id { get; set; } = "";

        /// <summary>Display name of the contact.</summary>
        public string Name { get; set; } = "";

        /// <summary>Primary phone number.</summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>Secondary phone number (optional).</summary>
        public string? SecondaryNumber { get; set; }

        /// <summary>Email address (optional).</summary>
        public string? Email { get; set; }

        /// <summary>Company name (optional).</summary>
        public string? Company { get; set; }

        /// <summary>Category or Group name (e.g., "CRM", "Zendesk").</summary>
        public string? Group { get; set; } = "Plugin";
    }
}
