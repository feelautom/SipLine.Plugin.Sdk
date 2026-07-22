using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SipLine.Plugin.Sdk.Enums;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Defines a tab to add to the sidebar.
    /// </summary>
    public sealed class PluginSidebarTab : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Gets or sets the unique tab identifier.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Gets or sets the identifier of the owning plugin.
        /// </summary>
        public string PluginId { get; set; } = "";

        private string _title = "";
        /// <summary>
        /// Gets or sets the tab title shown in the expanded sidebar.
        /// </summary>
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _tooltip = "";
        /// <summary>
        /// Gets or sets the tooltip shown when hovering over the collapsed sidebar.
        /// </summary>
        public string Tooltip
        {
            get => _tooltip;
            set
            {
                if (_tooltip != value)
                {
                    _tooltip = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the standard icon, which takes precedence over IconPathData.
        /// </summary>
        public PluginIcon? Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon as geometry path data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// Gets or sets the display order; lower values appear first.
        /// Application tabs use 0-100 and plugin tabs start at 200.
        /// </summary>
        public int Order { get; set; } = 200;

        private bool _isVisible = true;
        /// <summary>
        /// Gets or sets whether the tab is visible.
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

        private int? _badge;
        /// <summary>
        /// Gets or sets the badge text, such as a notification count; null hides the badge.
        /// </summary>
        public int? Badge
        {
            get => _badge;
            set
            {
                if (_badge != value)
                {
                    _badge = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the factory used to create the tab content.
        /// The factory must return a WPF UserControl.
        /// </summary>
        public Func<object>? ContentFactory { get; set; }

        /// <summary>
        /// Gets or sets the optional command executed when the tab is selected.
        /// When null, ContentFactory is used to display the content.
        /// </summary>
        public ICommand? Command { get; set; }
    }
}
