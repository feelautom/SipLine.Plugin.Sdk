using System.ComponentModel;
using System.Runtime.CompilerServices;
using SipLine.Plugin.Sdk.Licensing;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Status of a plugin license.
    /// </summary>
    public enum PluginLicenseStatus
    {
        /// <summary>
        /// No license is required for this free plugin.
        /// </summary>
        NotRequired,

        /// <summary>
        /// The license is valid.
        /// </summary>
        Valid,

        /// <summary>
        /// The license file is missing.
        /// </summary>
        Missing,

        /// <summary>
        /// The license is invalid or expired.
        /// </summary>
        Invalid,

        /// <summary>
        /// The application plan does not allow this plugin (Pro or higher is required).
        /// </summary>
        PlanRequired
    }

    /// <summary>
    /// Information about a loaded plugin.
    /// </summary>
    public sealed class PluginInfo : INotifyPropertyChanged
    {
        private bool _isEnabled = true;
        private bool _isInitialized;
        private string? _loadError;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the plugin instance.
        /// </summary>
        public ISipLinePlugin Plugin { get; set; } = null!;

        /// <summary>
        /// Gets or sets the plugin assembly path.
        /// </summary>
        public string DllPath { get; set; } = "";

        /// <summary>
        /// Gets or sets whether the plugin is enabled.
        /// </summary>
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the plugin initialized successfully.
        /// </summary>
        public bool IsInitialized
        {
            get => _isInitialized;
            set
            {
                if (_isInitialized != value)
                {
                    _isInitialized = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the load error, when loading failed.
        /// </summary>
        public string? LoadError
        {
            get => _loadError;
            set
            {
                if (_loadError != value)
                {
                    _loadError = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets when the plugin was loaded.
        /// </summary>
        public DateTime LoadedAt { get; set; } = DateTime.Now;

        // Raccourcis vers les propriétés du plugin
        public string Id => Plugin?.Id ?? "";
        public string Name => Plugin?.Name ?? "Unknown";
        public Version Version => Plugin?.Version ?? new Version(0, 0, 0);
        public string Author => Plugin?.Author ?? "";
        public string Description => Plugin?.Description ?? "";
        public bool HasSettingsUI => Plugin?.HasSettingsUI ?? false;

        /// <summary>
        /// Gets or sets whether the plugin is user-installed in AppData rather than bundled locally.
        /// </summary>
        public bool IsUserInstalled => DllPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        /// <summary>
        /// Gets or sets the display label for the plugin source.
        /// </summary>
        public string SourceLabel => IsUserInstalled ? "Installé" : "Intégré";

        /// <summary>
        /// Gets or sets the plugin license type.
        /// </summary>
        public PluginLicenseType LicenseType => Plugin?.LicenseType ?? PluginLicenseType.Community;

        /// <summary>
        /// Gets or sets the license status.
        /// </summary>
        public PluginLicenseStatus LicenseStatus { get; set; } = PluginLicenseStatus.NotRequired;

        /// <summary>
        /// Gets or sets detailed license information or an error message.
        /// </summary>
        public string? LicenseMessage { get; set; }

        /// <summary>
        /// Gets or sets the features allowed by the plugin license.
        /// The "*" value means every feature.
        /// </summary>
        public IReadOnlyList<string> LicensedFeatures { get; set; } = new List<string> { "*" };

        /// <summary>
        /// Gets whether the plugin can be used because its license is valid or not required.
        /// </summary>
        public bool IsLicenseValid => LicenseStatus == PluginLicenseStatus.NotRequired || LicenseStatus == PluginLicenseStatus.Valid;

        /// <summary>
        /// Gets whether the plugin is commercial and requires a license.
        /// </summary>
        public bool IsCommercial => LicenseType == PluginLicenseType.Commercial;

        /// <summary>
        /// Gets whether the plugin is blocked because the application plan is insufficient.
        /// </summary>
        public bool IsPlanRequired => LicenseStatus == PluginLicenseStatus.PlanRequired;

        /// <summary>
        /// Gets or sets the settings fields declared by the plugin.
        /// </summary>
        public List<PluginSettingsField> SettingsFields { get; set; } = new();

        /// <summary>
        /// Gets whether the plugin declares settings fields.
        /// </summary>
        public bool HasSettingsFields => SettingsFields.Count > 0;

        private bool _areSettingsExpanded;
        /// <summary>
        /// Gets or sets whether the plugin settings are expanded.
        /// </summary>
        public bool AreSettingsExpanded
        {
            get => _areSettingsExpanded;
            set
            {
                if (_areSettingsExpanded != value)
                {
                    _areSettingsExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _areRequiredSettingsFilled = true;
        /// <summary>
        /// Gets or sets whether all required settings are filled in.
        /// </summary>
        public bool AreRequiredSettingsFilled
        {
            get => _areRequiredSettingsFilled;
            set
            {
                if (_areRequiredSettingsFilled != value)
                {
                    _areRequiredSettingsFilled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ShouldShowInMenu));
                }
            }
        }

        /// <summary>
        /// Gets whether the plugin should appear in the menu based on licensing and required settings.
        /// </summary>
        public bool ShouldShowInMenu => IsEnabled && IsLicenseValid && AreRequiredSettingsFilled;

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
