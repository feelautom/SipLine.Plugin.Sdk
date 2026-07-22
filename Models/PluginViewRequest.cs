namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Request to open a plugin view.
    /// </summary>
    public sealed class PluginViewRequest
    {
        /// <summary>
        /// Gets or sets the unique view identifier.
        /// </summary>
        public string ViewId { get; set; } = "";

        /// <summary>
        /// Gets or sets the title shown in the view header.
        /// </summary>
        public string Title { get; set; } = "";

        /// <summary>
        /// Gets or sets the optional subtitle or description.
        /// </summary>
        public string? Subtitle { get; set; }

        /// <summary>
        /// Gets or sets the icon as geometry path data (SVG path).
        /// </summary>
        public string? IconPathData { get; set; }

        /// <summary>
        /// Gets or sets the WPF UserControl used as view content.
        /// </summary>
        public object Content { get; set; } = null!;

        /// <summary>
        /// Gets or sets whether to show a back button that closes the view.
        /// </summary>
        public bool ShowBackButton { get; set; } = true;

        /// <summary>
        /// Gets or sets the callback invoked when the view closes.
        /// </summary>
        public Action? OnClosed { get; set; }
    }
}
