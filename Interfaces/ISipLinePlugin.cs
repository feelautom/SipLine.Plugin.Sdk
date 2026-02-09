using SipLine.Plugin.Sdk.Licensing;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Interface principale que chaque plugin SipLine doit implémenter.
    /// </summary>
    public interface ISipLinePlugin : IDisposable
    {
        /// <summary>
        /// Identifiant unique du plugin (ex: "sipline.plugin.ovh").
        /// Utilisé pour la configuration et le stockage des settings.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Type de licence du plugin.
        /// - Integrated: Plugin intégré gratuit (pas de licence)
        /// - Community: Plugin communautaire gratuit (pas de licence)
        /// - Commercial: Plugin payant (licence requise)
        /// </summary>
        PluginLicenseType LicenseType => PluginLicenseType.Community;

        /// <summary>
        /// Nom affiché dans l'interface utilisateur.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Version du plugin.
        /// </summary>
        Version Version { get; }

        /// <summary>
        /// Auteur du plugin.
        /// </summary>
        string Author { get; }

        /// <summary>
        /// Description courte du plugin (1-2 lignes).
        /// </summary>
        string Description { get; }

        /// <summary>
        /// URL du site web ou dépôt du plugin (optionnel).
        /// </summary>
        string? WebsiteUrl { get; }

        /// <summary>
        /// Icône du plugin au format Geometry Path Data (SVG path).
        /// Si null, une icône par défaut sera utilisée.
        /// </summary>
        string? IconPathData { get; }

        /// <summary>
        /// Appelé au chargement du plugin.
        /// C'est ici que le plugin s'initialise et s'abonne aux événements.
        /// </summary>
        /// <param name="context">Contexte fournissant l'accès aux services SipLine</param>
        Task InitializeAsync(IPluginContext context);

        /// <summary>
        /// Appelé à l'arrêt de l'application.
        /// Le plugin doit se désabonner des événements et libérer ses ressources.
        /// </summary>
        Task ShutdownAsync();

        /// <summary>
        /// Indique si le plugin a une interface de configuration.
        /// </summary>
        bool HasSettingsUI { get; }

        /// <summary>
        /// Retourne le UserControl WPF de configuration.
        /// Appelé uniquement si HasSettingsUI = true.
        /// </summary>
        /// <returns>Un UserControl WPF ou null</returns>
        object? GetSettingsUI();
    }
}
