namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Définit un onglet de paramètres personnalisé fourni par un plugin.
    /// </summary>
    public sealed class PluginSettingsTab
    {
        /// <summary>
        /// Identifiant unique de l'onglet.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Titre affiché dans le menu de navigation.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Icône au format Geometry Path Data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// UserControl WPF contenant l'interface de l'onglet.
        /// </summary>
        public object Content { get; set; } = null!;

        /// <summary>
        /// Ordre d'affichage (plus petit = plus haut).
        /// Les onglets de l'app sont à 0-100, les plugins commencent à 200.
        /// </summary>
        public int Order { get; set; } = 200;
    }
}
