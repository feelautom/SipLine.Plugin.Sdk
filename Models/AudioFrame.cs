namespace SipLine.Plugin.Sdk.Models
{
    /// <summary>
    /// Represents a frame of PCM audio data from an active call.
    /// Audio is PCM 16-bit signed, mono, 8000 Hz (standard SIP/RTP).
    /// Each frame is typically 20ms (160 samples).
    /// </summary>
    public sealed class AudioFrame
    {
        /// <summary>
        /// The call this audio frame belongs to.
        /// </summary>
        public string CallId { get; set; } = "";

        /// <summary>
        /// PCM 16-bit signed samples (mono, 8000 Hz).
        /// </summary>
        public short[] Samples { get; set; } = Array.Empty<short>();

        /// <summary>
        /// Timestamp of when this frame was captured.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Audio direction relative to the local user.
        /// </summary>
        public AudioDirection Direction { get; set; }

        /// <summary>
        /// Sample rate in Hz (always 8000 for SIP).
        /// </summary>
        public int SampleRate => 8000;

        /// <summary>
        /// Number of channels (always 1 — mono).
        /// </summary>
        public int Channels => 1;

        /// <summary>
        /// Duration of this frame based on sample count.
        /// </summary>
        public TimeSpan Duration => TimeSpan.FromSeconds((double)Samples.Length / SampleRate);
    }

    /// <summary>
    /// Direction of an audio frame.
    /// </summary>
    public enum AudioDirection
    {
        /// <summary>Audio received from the remote party.</summary>
        Incoming,

        /// <summary>Audio captured from the local microphone.</summary>
        Outgoing
    }
}
