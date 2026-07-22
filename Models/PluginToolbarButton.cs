using System.Windows.Input;
using SipLine.Plugin.Sdk.Enums;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Defines a toolbar button provided by a plugin.
    /// </summary>
    public sealed class PluginToolbarButton
    {
        /// <summary>
        /// Gets or sets the unique button identifier.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Gets or sets the tooltip shown on hover.
        /// </summary>
        public string Tooltip { get; set; } = "";

        /// <summary>
        /// Gets or sets the standard icon, which takes precedence over IconPathData.
        /// </summary>
        public PluginIcon? Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon as geometry path data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// Gets or sets the command executed when the button is clicked.
        /// </summary>
        public ICommand? Command { get; set; }

        /// <summary>
        /// Gets or sets the command parameter.
        /// </summary>
        public object? CommandParameter { get; set; }

        /// <summary>
        /// Gets or sets the display order; lower values appear farther left.
        /// Application buttons use 0-100 and plugin buttons start at 200.
        /// </summary>
        public int Order { get; set; } = 200;

        /// <summary>
        /// Gets or sets whether the button is visible.
        /// </summary>
        public bool IsVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets whether the button is enabled.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
