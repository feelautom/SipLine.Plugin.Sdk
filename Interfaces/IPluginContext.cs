using Microsoft.Extensions.Logging;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Contexte fourni aux plugins pour accéder aux services SipLine.
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// Service SIP pour écouter/contrôler les appels.
        /// </summary>
        IPluginSipService SipService { get; }

        /// <summary>
        /// Historique des appels.
        /// </summary>
        IPluginCallHistory CallHistory { get; }

        /// <summary>
        /// Logger pour le plugin (préfixé avec le nom du plugin).
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Dossier de données dédié au plugin pour stocker ses fichiers.
        /// Exemple: %AppData%/SipLine/plugins/sipline.plugin.ovh/
        /// </summary>
        string PluginDataPath { get; }

        /// <summary>
        /// Affiche une notification Windows Toast.
        /// </summary>
        void ShowNotification(string title, string message, NotificationSeverity severity = NotificationSeverity.Info);

        /// <summary>
        /// Affiche un message dans le snackbar de l'application.
        /// </summary>
        void ShowSnackbar(string message, SnackbarSeverity severity = SnackbarSeverity.Info);

        /// <summary>
        /// Ajoute un message dans le log de l'application.
        /// </summary>
        void AddLog(string message, string level = "Info");

        /// <summary>
        /// Enregistre un bouton dans la barre d'outils principale.
        /// </summary>
        void RegisterToolbarButton(PluginToolbarButton button);

        /// <summary>
        /// Supprime un bouton de la barre d'outils.
        /// </summary>
        void UnregisterToolbarButton(string buttonId);

        /// <summary>
        /// Enregistre un onglet dans les paramètres.
        /// </summary>
        void RegisterSettingsTab(PluginSettingsTab tab);

        /// <summary>
        /// Supprime un onglet des paramètres.
        /// </summary>
        void UnregisterSettingsTab(string tabId);

        /// <summary>
        /// Enregistre un onglet dans la barre latérale (à côté de Récents/Contacts).
        /// </summary>
        void RegisterSidebarTab(PluginSidebarTab tab);

        /// <summary>
        /// Supprime un onglet de la barre latérale.
        /// </summary>
        void UnregisterSidebarTab(string tabId);

        /// <summary>
        /// Demande l'affichage d'une vue pleine page du plugin.
        /// La vue remplace la zone centrale de l'application.
        /// </summary>
        void OpenPluginView(PluginViewRequest request);

        /// <summary>
        /// Ferme la vue plugin actuellement affichée.
        /// </summary>
        void ClosePluginView();

        /// <summary>
        /// Événement déclenché lors du changement de langue de l'application.
        /// </summary>
        event Action<string>? OnLanguageChanged;

        /// <summary>
        /// Récupère une valeur de configuration du plugin.
        /// </summary>
        T? GetSetting<T>(string key, T? defaultValue = default);

        /// <summary>
        /// Récupère une chaîne localisée via le système global de l'application.
        /// </summary>
        string GetLocalizedString(string key);

        /// <summary>
        /// Sauvegarde une valeur de configuration du plugin.
        /// </summary>
        void SetSetting<T>(string key, T value);

        /// <summary>
        /// Enregistre un ResourceManager pour la localisation globale.
        /// Les clés de ce ResourceManager seront accessibles via le système de traduction de l'application.
        /// </summary>
        void RegisterResource(System.Resources.ResourceManager resourceManager);

        /// <summary>
        /// Déclare les champs de paramètres du plugin.
        /// Ces champs seront affichés dans l'onglet Plugins des paramètres.
        /// Si un champ a IsRequired=true et n'est pas rempli, le plugin ne s'affichera pas dans le menu.
        /// </summary>
        void RegisterSettingsFields(IEnumerable<PluginSettingsField> fields);

        /// <summary>
        /// Indique si tous les paramètres requis du plugin sont remplis.
        /// </summary>
        bool AreRequiredSettingsFilled { get; }
    }

    /// <summary>
    /// Sévérité des notifications
    /// </summary>
    public enum NotificationSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// Sévérité des snackbars
    /// </summary>
    public enum SnackbarSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }
}
