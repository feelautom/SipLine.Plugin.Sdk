using System.Globalization;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Provides access to the plugin's localized resources, respecting the host application's language settings.
    /// </summary>
    public interface IPluginLocalization
    {
        /// <summary>
        /// Gets the localized string for the specified key.
        /// Returns the key itself if not found.
        /// </summary>
        /// <param name="key">Resource key to look up.</param>
        /// <returns>Localized string.</returns>
        string GetString(string key);

        /// <summary>
        /// Gets the localized string for the specified key and formats it with the provided arguments.
        /// </summary>
        /// <param name="key">Resource key to look up.</param>
        /// <param name="args">Format arguments.</param>
        /// <returns>Formatted localized string.</returns>
        string GetString(string key, params object[] args);

        /// <summary>
        /// Gets the current culture used by the application (and thus the plugin).
        /// </summary>
        CultureInfo CurrentCulture { get; }
    }
}
