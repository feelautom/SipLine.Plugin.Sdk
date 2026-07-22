using SipLine.Plugin.Sdk.Enums;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Defines a custom settings tab provided by a plugin.
    /// </summary>
    public sealed class PluginSettingsTab
    {
        /// <summary>
        /// Gets or sets the unique tab identifier.
        /// </summary>
        public string Id { get; set; } = "";

        /// <summary>
        /// Gets or sets the title shown in the navigation menu.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the standard icon, which takes precedence over IconPathData.
        /// </summary>
        public PluginIcon? Icon { get; set; }

        /// <summary>
        /// Gets or sets the icon as geometry path data (SVG path).
        /// </summary>
        public string IconPathData { get; set; } = "";

        /// <summary>
        /// Gets or sets the WPF UserControl containing the tab interface.
        /// </summary>
        public object Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets the display order; lower values appear first.
        /// Application tabs use 0-100 and plugin tabs start at 200.
        /// </summary>
        public int Order { get; set; } = 200;
    }
}
