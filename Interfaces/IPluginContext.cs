using Microsoft.Extensions.Logging;
using SipLine.Plugin.Sdk.Models;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Context provided to plugins to access SipLine services.
    /// </summary>
    public interface IPluginContext
    {
        /// <summary>
        /// SIP service to listen to and control calls.
        /// </summary>
        IPluginSipService SipService { get; }

        /// <summary>
        /// Access to call history records.
        /// </summary>
        IPluginCallHistory CallHistory { get; }

        /// <summary>
        /// Service to manage contacts within SipLine.
        /// </summary>
        IPluginContactService Contacts { get; }

        /// <summary>
        /// Service to monitor and control audio hardware.
        /// </summary>
        IPluginAudioService Audio { get; }

        /// <summary>
        /// Logger for the plugin (prefixed with the plugin name).
        /// </summary>
        ILogger Logger { get; }

        /// <summary>
        /// Dedicated data folder for the plugin to store its local files.
        /// Example: %AppData%/SipLine/plugins/sipline.plugin.ovh/
        /// </summary>
        string PluginDataPath { get; }

        /// <summary>
        /// Shows a confirmation dialog with custom title, message and button labels.
        /// </summary>
        /// <returns>True if the user clicked the primary button, false otherwise.</returns>
        Task<bool> ShowDialogAsync(string title, string message, string primaryButton = "OK", string secondaryButton = "Cancel");

        /// <summary>
        /// Shows an input dialog to ask for a text value.
        /// </summary>
        /// <returns>The text entered by the user, or null if canceled.</returns>
        Task<string?> ShowInputAsync(string title, string message, string defaultValue = "", string primaryButton = "OK", string secondaryButton = "Cancel");

        /// <summary>
        /// Shows a native Windows Toast notification.
        /// </summary>
        void ShowNotification(string title, string message, NotificationSeverity severity = NotificationSeverity.Info);

        /// <summary>
        /// Shows a message in the application's snackbar.
        /// </summary>
        void ShowSnackbar(string message, SnackbarSeverity severity = SnackbarSeverity.Info);

        /// <summary>
        /// Adds a message to the application's internal logs.
        /// </summary>
        void AddLog(string message, string level = "Info");

        /// <summary>
        /// Registers a button in the main sidebar toolbar.
        /// </summary>
        void RegisterToolbarButton(PluginToolbarButton button);

        /// <summary>
        /// Removes a button from the sidebar toolbar.
        /// </summary>
        void UnregisterToolbarButton(string buttonId);

        /// <summary>
        /// Registers a new tab in the settings window.
        /// </summary>
        void RegisterSettingsTab(PluginSettingsTab tab);

        /// <summary>
        /// Removes a tab from the settings window.
        /// </summary>
        void UnregisterSettingsTab(string tabId);

        /// <summary>
        /// Registers a tab in the main sidebar (alongside Recents/Contacts).
        /// </summary>
        void RegisterSidebarTab(PluginSidebarTab tab);

        /// <summary>
        /// Removes a tab from the main sidebar.
        /// </summary>
        void UnregisterSidebarTab(string tabId);

        /// <summary>
        /// Selects a specific sidebar tab by its ID.
        /// </summary>
        void SelectSidebarTab(string tabId);

        /// <summary>
        /// Registers a search provider to extend global search.
        /// </summary>
        void RegisterSearchProvider(IPluginSearchProvider provider);

        /// <summary>
        /// Unregisters a search provider.
        /// </summary>
        void UnregisterSearchProvider(string providerName);

        /// <summary>
        /// Registers a custom option in a context menu.
        /// </summary>
        /// <param name="area">Target menu area</param>
        /// <param name="label">Label displayed in the menu</param>
        /// <param name="callback">Action executed when the option is clicked. Object is context data (CallInfo, Contact, etc.)</param>
        void RegisterContextMenuOption(MenuArea area, string label, Action<object> callback);

        /// <summary>
        /// Requests to display a full-page view for the plugin.
        /// This view replaces the main dashboard area.
        /// </summary>
        void OpenPluginView(PluginViewRequest request);

        /// <summary>
        /// Closes the currently displayed full-page plugin view.
        /// </summary>
        void ClosePluginView();

        /// <summary>
        /// Event triggered when the application language changes.
        /// </summary>
        event Action<string>? OnLanguageChanged;

        /// <summary>
        /// Event triggered when audio devices are added or removed.
        /// </summary>
        event Action? OnDevicesChanged;

        /// <summary>
        /// Retrieves a configuration value for the plugin.
        /// </summary>
        T? GetSetting<T>(string key, T? defaultValue = default);

        /// <summary>
        /// Provides access to the plugin's localized resources.
        /// Replaces <see cref="GetLocalizedString(string)"/> and <see cref="RegisterResource(System.Resources.ResourceManager)"/>.
        /// </summary>
        IPluginLocalization Localization { get; }

        /// <summary>
        /// Retrieves a localized string via the application's global localization system.
        /// </summary>
        [Obsolete("Use context.Localization.GetString(key) instead.")]
        string GetLocalizedString(string key);

        /// <summary>
        /// Saves a configuration value for the plugin.
        /// </summary>
        void SetSetting<T>(string key, T value);

        /// <summary>
        /// Registers a ResourceManager for global localization.
        /// Keys from this ResourceManager will be accessible via GetLocalizedString.
        /// </summary>
        [Obsolete("Use context.Localization instead. The plugin's primary ResourceManager is automatically detected.")]
        void RegisterResource(System.Resources.ResourceManager resourceManager);

        /// <summary>
        /// Declares settings fields for the plugin.
        /// These fields will be displayed in the Settings > Plugins tab.
        /// If a field has IsRequired=true and is empty, the plugin tab will be hidden.
        /// </summary>
        void RegisterSettingsFields(IEnumerable<PluginSettingsField> fields);

        /// <summary>
        /// Indicates if all required settings for the plugin are filled.
        /// </summary>
        bool AreRequiredSettingsFilled { get; }

        /// <summary>
        /// Indicates if the application is currently using a dark theme.
        /// </summary>
        bool IsDarkTheme { get; }

        /// <summary>
        /// Current version of the SipLine application.
        /// </summary>
        string AppVersion { get; }

        /// <summary>
        /// Executes an action on the application's UI thread.
        /// Useful for updating UI from background tasks.
        /// </summary>
        Task RunOnUIThread(Action action);

        /// <summary>
        /// Executes a function on the application's UI thread and returns its result.
        /// </summary>
        Task<T> RunOnUIThread<T>(Func<T> function);
    }

    /// <summary>
    /// Notification severity levels
    /// </summary>
    public enum NotificationSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }

    /// <summary>
    /// Snackbar severity levels
    /// </summary>
    public enum SnackbarSeverity
    {
        Info,
        Success,
        Warning,
        Error
    }
}
