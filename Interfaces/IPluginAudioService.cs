using System;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Service to monitor and control audio hardware through SipLine.
    /// </summary>
    public interface IPluginAudioService
    {
        /// <summary>
        /// Gets or sets the speaker/playback volume (0 to 100).
        /// </summary>
        int Volume { get; set; }

        /// <summary>
        /// Indicates if the microphone is currently muted within the application.
        /// </summary>
        bool IsMuted { get; }

        /// <summary>
        /// Sets the microphone mute state.
        /// </summary>
        void SetMute(bool mute);

        /// <summary>
        /// Triggered when the volume is changed.
        /// </summary>
        event Action<int> OnVolumeChanged;

        /// <summary>
        /// Triggered when the mute state is changed.
        /// </summary>
        event Action<bool> OnMuteChanged;
    }
}
