using System.Windows;

namespace SipLine.Plugin.Sdk.Theme
{
    /// <summary>
    /// Provides standardized resource keys for consistent plugin theming.
    /// Use these keys with <see cref="ComponentResourceKey"/> to match the host application style.
    /// <example>
    /// Usage in XAML:
    /// {DynamicResource {x:Static theme:PluginTheme.BackgroundBrushKey}}
    /// </example>
    /// </summary>
    public static class PluginTheme
    {
        private static ComponentResourceKey CreateKey(string resourceId)
        {
            return new ComponentResourceKey(typeof(PluginTheme), resourceId);
        }

        #region Brushes

        /// <summary>
        /// Key for the primary background brush (e.g., window background).
        /// </summary>
        public static ComponentResourceKey BaseBrushKey => CreateKey("BaseBrush");

        /// <summary>
        /// Key for surface/panel background brush (slightly lighter than base).
        /// </summary>
        public static ComponentResourceKey SurfaceBrushKey => CreateKey("SurfaceBrush");

        /// <summary>
        /// Key for input fields background brush.
        /// </summary>
        public static ComponentResourceKey InputBrushKey => CreateKey("InputBrush");

        /// <summary>
        /// Key for borders and separators brush.
        /// </summary>
        public static ComponentResourceKey BorderBrushKey => CreateKey("BorderBrush");

        /// <summary>
        /// Key for hover/highlight background brush.
        /// </summary>
        public static ComponentResourceKey HoverBrushKey => CreateKey("HoverBrush");

        /// <summary>
        /// Key for primary accent/action color brush.
        /// </summary>
        public static ComponentResourceKey AccentBrushKey => CreateKey("AccentBrush");

        /// <summary>
        /// Key for success state brush (green).
        /// </summary>
        public static ComponentResourceKey SuccessBrushKey => CreateKey("SuccessBrush");

        /// <summary>
        /// Key for warning state brush (yellow/orange).
        /// </summary>
        public static ComponentResourceKey WarningBrushKey => CreateKey("WarningBrush");

        /// <summary>
        /// Key for error/danger state brush (red).
        /// </summary>
        public static ComponentResourceKey DangerBrushKey => CreateKey("DangerBrush");

        /// <summary>
        /// Key for primary text/foreground brush.
        /// </summary>
        public static ComponentResourceKey TextBrushKey => CreateKey("TextBrush");

        /// <summary>
        /// Key for muted/secondary text brush.
        /// </summary>
        public static ComponentResourceKey MutedBrushKey => CreateKey("MutedBrush");

        #endregion

        #region Colors

        /// <summary>
        /// Key for the primary background color.
        /// </summary>
        public static ComponentResourceKey BaseColorKey => CreateKey("BaseColor");

        /// <summary>
        /// Key for the accent color.
        /// </summary>
        public static ComponentResourceKey AccentColorKey => CreateKey("AccentColor");

        #endregion
    }
}
