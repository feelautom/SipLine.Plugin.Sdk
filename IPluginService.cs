namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Result of uninstalling a plugin.
    /// </summary>
    public enum PluginUninstallResult
    {
        /// <summary>The plugin could not be uninstalled.</summary>
        Failed,
        /// <summary>The plugin was deleted immediately.</summary>
        Deleted,
        /// <summary>Locked files are scheduled for deletion after restart.</summary>
        PendingRestart
    }

    /// <summary>
    /// Manages the installed plugins.
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Gets the loaded plugins.
        /// </summary>
        IReadOnlyList<PluginInfo> LoadedPlugins { get; }

        /// <summary>
        /// Loads every plugin from the configured directory.
        /// </summary>
        Task LoadPluginsAsync();

        /// <summary>
        /// Enables or disables a plugin.
        /// </summary>
        Task SetPluginEnabledAsync(string pluginId, bool enabled);

        /// <summary>
        /// Unloads all plugins.
        /// </summary>
        Task UnloadAllAsync();

        /// <summary>
        /// Reloads a specific plugin.
        /// </summary>
        Task ReloadPluginAsync(string pluginId);

        /// <summary>
        /// Uninstalls a plugin by unloading its assembly and deleting its directory.
        /// </summary>
        Task<PluginUninstallResult> UninstallPluginAsync(string pluginId);

        /// <summary>
        /// Raised when a plugin is loaded.
        /// </summary>
        event Action<PluginInfo>? OnPluginLoaded;

        /// <summary>
        /// Raised when a plugin is unloaded.
        /// </summary>
        event Action<string>? OnPluginUnloaded;

        /// <summary>
        /// Raised when a plugin state changes.
        /// </summary>
        event Action<PluginInfo>? OnPluginStateChanged;
    }
}
