using SipLine.Plugin.Sdk.Licensing;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Main interface that every SipLine plugin must implement.
    /// </summary>
    public interface ISipLinePlugin : IDisposable
    {
        /// <summary>
        /// Unique identifier for the plugin (e.g., "sipline.plugin.ovh").
        /// Used for configuration and settings storage.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// License type for the plugin.
        /// - Integrated: Free built-in plugin (no license)
        /// - Community: Free community plugin (no license)
        /// - Commercial: Paid plugin (license required)
        /// </summary>
        PluginLicenseType LicenseType => PluginLicenseType.Community;

        /// <summary>
        /// Name displayed in the user interface.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Plugin version.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Plugin author.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Short description of the plugin (1-2 lines).
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Website or repository URL for the plugin (optional).
        /// </summary>
        string? WebsiteUrl { get; }

        /// <summary>
        /// Plugin icon in Geometry Path Data format (SVG path).
        /// If null, a default icon will be used.
        /// </summary>
        string? IconPathData { get; }

        /// <summary>
        /// Called when the plugin is loaded.
        /// The plugin should initialize and subscribe to events here.
        /// </summary>
        /// <param name="context">Context providing access to SipLine services</param>
        Task InitializeAsync(IPluginContext context);

        /// <summary>
        /// Called when the application is stopping.
        /// The plugin must unsubscribe from events and release resources.
        /// </summary>
        Task ShutdownAsync();

        /// <summary>
        /// Indicates if the plugin has a custom configuration UI.
        /// </summary>
        bool HasSettingsUI { get; }

        /// <summary>
        /// Returns the WPF UserControl for configuration.
        /// Called only if HasSettingsUI = true.
        /// </summary>
        /// <returns>A WPF UserControl or null</returns>
        object? GetSettingsUI();
    }
}
