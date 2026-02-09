using System.ComponentModel;
using System.Runtime.CompilerServices;
using SipLine.Plugin.Sdk.Licensing;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Statut de la licence d'un plugin.
    /// </summary>
    public enum PluginLicenseStatus
    {
        /// <summary>
        /// Licence non requise (plugin gratuit).
        /// </summary>
        NotRequired,

        /// <summary>
        /// Licence valide.
        /// </summary>
        Valid,

        /// <summary>
        /// Fichier de licence manquant.
        /// </summary>
        Missing,

        /// <summary>
        /// Licence invalide ou expirée.
        /// </summary>
        Invalid
    }

    /// <summary>
    /// Informations sur un plugin chargé.
    /// </summary>
    public sealed class PluginInfo : INotifyPropertyChanged
    {
        private bool _isEnabled = true;
        private bool _isInitialized;
        private string? _loadError;

        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Instance du plugin.
        /// </summary>
        public ISipLinePlugin Plugin { get; set; } = null!;

        /// <summary>
        /// Chemin du fichier DLL.
        /// </summary>
        public string DllPath { get; set; } = "";

        /// <summary>
        /// Le plugin est activé.
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
        /// Le plugin a été initialisé avec succès.
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
        /// Erreur lors du chargement (si échec).
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
        /// Date de chargement du plugin.
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
        /// Indique si le plugin est installé par l'utilisateur (AppData) ou bundled (local).
        /// </summary>
        public bool IsUserInstalled => DllPath.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        /// <summary>
        /// Label de la source du plugin.
        /// </summary>
        public string SourceLabel => IsUserInstalled ? "Installé" : "Intégré";

        /// <summary>
        /// Type de licence du plugin.
        /// </summary>
        public PluginLicenseType LicenseType => Plugin?.LicenseType ?? PluginLicenseType.Community;

        /// <summary>
        /// Statut de la licence.
        /// </summary>
        public PluginLicenseStatus LicenseStatus { get; set; } = PluginLicenseStatus.NotRequired;

        /// <summary>
        /// Message détaillé sur la licence (erreur ou info).
        /// </summary>
        public string? LicenseMessage { get; set; }

        /// <summary>
        /// Indique si le plugin peut être utilisé (licence ok ou non requise).
        /// </summary>
        public bool IsLicenseValid => LicenseStatus == PluginLicenseStatus.NotRequired || LicenseStatus == PluginLicenseStatus.Valid;

        /// <summary>
        /// Indique si le plugin est commercial (nécessite une licence).
        /// </summary>
        public bool IsCommercial => LicenseType == PluginLicenseType.Commercial;

        /// <summary>
        /// Champs de paramètres déclarés par le plugin.
        /// </summary>
        public List<PluginSettingsField> SettingsFields { get; set; } = new();

        /// <summary>
        /// Indique si le plugin a des champs de paramètres.
        /// </summary>
        public bool HasSettingsFields => SettingsFields.Count > 0;

        private bool _areSettingsExpanded;
        /// <summary>
        /// Indique si les settings du plugin sont expandus.
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
        /// Indique si tous les paramètres requis sont remplis.
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
        /// Indique si le plugin doit apparaître dans le menu (licence ok + settings remplis).
        /// </summary>
        public bool ShouldShowInMenu => IsEnabled && IsLicenseValid && AreRequiredSettingsFilled;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
