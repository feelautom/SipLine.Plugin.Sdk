namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Feature flags supported by a commercial plugin license.
/// </summary>
public static class PluginFeatureKeys
{
    /// <summary>
    /// Wildcard allowing every feature.
    /// </summary>
    public const string All = "*";

    public const string UiSidebarTab = "ui.sidebar_tab";
    public const string UiSettingsTab = "ui.settings_tab";
    public const string UiToolbarButton = "ui.toolbar_button";
    public const string UiOpenView = "ui.open_view";
    public const string UiContextMenu = "ui.context_menu";
    public const string SearchProvider = "search.provider";

    /// <summary>
    /// Allows the plugin to answer incoming calls programmatically.
    /// </summary>
    public const string TelephonyAnswerCall = "telephony.answer_call";

    /// <summary>
    /// Allows the plugin to receive real-time PCM audio frames during calls.
    /// </summary>
    public const string TelephonyAudioStream = "telephony.audio_stream";

    /// <summary>
    /// Allows the plugin to inject audio (TTS, tones, etc.) into an active call.
    /// </summary>
    public const string TelephonySendAudio = "telephony.send_audio";
}
