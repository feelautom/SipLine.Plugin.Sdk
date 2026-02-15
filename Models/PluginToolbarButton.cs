using System.Windows.Input;
using SipLine.Plugin.Sdk.Enums;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Définit un bouton de barre d'outils fourni par un plugin.
    /// </summary>
    public sealed class PluginToolbarButton
    {
        /// <summary>
        /// Identifiant unique du bouton.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Tooltip affiché au survol.
        /// </summary>
        public string Tooltip { get; set; } = "";

        /// <summary>
        /// Icône standard (prioritaire sur IconPathData).
        /// </summary>
        public PluginIcon? Icon { get; set; }

        /// <summary>
        /// Icône au format Geometry Path Data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// Commande exécutée au clic.
        /// </summary>
        public ICommand? Command { get; set; }

        /// <summary>
        /// Paramètre passé à la commande.
        /// </summary>
        public object? CommandParameter { get; set; }

        /// <summary>
        /// Ordre d'affichage (plus petit = plus à gauche).
        /// Les boutons de l'app sont à 0-100, les plugins commencent à 200.
        /// </summary>
        public int Order { get; set; } = 200;

        /// <summary>
        /// Le bouton est-il visible ?
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Le bouton est-il activé ?
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
