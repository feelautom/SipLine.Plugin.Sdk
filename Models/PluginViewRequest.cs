namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Requête pour ouvrir une vue de plugin.
    /// </summary>
    public sealed class PluginViewRequest
    {
        /// <summary>
        /// Identifiant unique de la vue.
        /// </summary>
        public string ViewId { get; set; } = "";

        /// <summary>
        /// Titre affiché dans l'en-tête de la vue.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Sous-titre ou description (optionnel).
        /// </summary>
        public string? Subtitle { get; set; }

        /// <summary>
        /// Icône au format Geometry Path Data (SVG path).
        /// </summary>
        public string? IconPathData { get; set; }

        /// <summary>
        /// Le contenu de la vue (UserControl WPF).
        /// </summary>
        public object Content { get; set; } = null!;

        /// <summary>
        /// Afficher un bouton de retour pour fermer la vue.
        /// </summary>
        public bool ShowBackButton { get; set; } = true;

        /// <summary>
        /// Callback appelé quand la vue est fermée.
        /// </summary>
        public Action? OnClosed { get; set; }
    }
}
