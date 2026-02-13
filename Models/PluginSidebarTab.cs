using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Définition d'un onglet à ajouter dans la barre latérale.
    /// </summary>
    public sealed class PluginSidebarTab : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Identifiant unique de l'onglet.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// ID du plugin propriétaire.
        /// </summary>
        public string PluginId { get; set; } = "";

        /// <summary>
        /// Titre de l'onglet (affiché dans la sidebar étendue).
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Tooltip affiché au survol (sidebar réduite).
        /// </summary>
        public string Tooltip { get; set; } = "";

        /// <summary>
        /// Icône au format Geometry Path Data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// Ordre d'affichage (plus petit = plus haut).
        /// Les onglets de l'app sont à 0-100, les plugins commencent à 200.
        /// </summary>
        public int Order { get; set; } = 200;

        private bool _isVisible = true;
        /// <summary>
        /// L'onglet est-il visible ?
        /// </summary>
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Badge à afficher (ex: nombre de notifications). Null = pas de badge.
        /// </summary>
        public int? Badge { get; set; }

        /// <summary>
        /// Fonction appelée pour créer le contenu de l'onglet.
        /// Doit retourner un UserControl WPF.
        /// </summary>
        public Func<object>? ContentFactory { get; set; }

        /// <summary>
        /// Commande exécutée quand l'onglet est sélectionné (optionnel).
        /// Si null, le ContentFactory est utilisé pour afficher le contenu.
        /// </summary>
        public ICommand? Command { get; set; }
    }
}
