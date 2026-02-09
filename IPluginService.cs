namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Service de gestion des plugins.
    /// </summary>
    public interface IPluginService
    {
        /// <summary>
        /// Liste des plugins chargés.
        /// </summary>
        IReadOnlyList<PluginInfo> LoadedPlugins { get; }

        /// <summary>
        /// Charge tous les plugins depuis le dossier configuré.
        /// </summary>
        Task LoadPluginsAsync();

        /// <summary>
        /// Active ou désactive un plugin.
        /// </summary>
        Task SetPluginEnabledAsync(string pluginId, bool enabled);

        /// <summary>
        /// Décharge tous les plugins.
        /// </summary>
        Task UnloadAllAsync();

        /// <summary>
        /// Recharge un plugin spécifique.
        /// </summary>
        Task ReloadPluginAsync(string pluginId);

        /// <summary>
        /// Événement déclenché quand un plugin est chargé.
        /// </summary>
        event Action<PluginInfo>? OnPluginLoaded;

        /// <summary>
        /// Événement déclenché quand un plugin est déchargé.
        /// </summary>
        event Action<string>? OnPluginUnloaded;

        /// <summary>
        /// Événement déclenché quand l'état d'un plugin change.
        /// </summary>
        event Action<PluginInfo>? OnPluginStateChanged;
    }
}
