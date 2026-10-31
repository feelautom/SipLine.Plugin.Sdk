using SipLine.Plugin.Sdk.Models;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Diagnostics exposed to plugins for tests and support (SDK 1.4.0). Every member requires
    /// <see cref="Licensing.PluginPermissions.Diagnostics"/> and throws <see cref="Licensing.PluginPermissionException"/>
    /// without it. SipLine keeps the data in memory only, in bounded buffers, and masks secrets and personal
    /// identifiers before returning them.
    /// </summary>
    public interface IPluginDiagnostics
    {
        /// <summary>Returns the most recent SIP messages, oldest first.</summary>
        /// <param name="maxCount">Number of messages, 1 to 500.</param>
        /// <param name="callId">Only the messages of this Call-ID (exact match); null for every message.</param>
        IReadOnlyList<PluginSipTraceEntry> GetSipTrace(int maxCount = 100, string? callId = null);

        /// <summary>Returns the media and SRTP outcome of a call.</summary>
        /// <param name="callId">Call-ID (exact match); null for the most recent call.</param>
        /// <returns>The outcome, or null when no INVITE of that call is in memory.</returns>
        PluginCallMediaInfo? GetCallMedia(string? callId = null);

        /// <summary>Returns the most recent registration outcomes, oldest first.</summary>
        /// <param name="maxCount">Number of outcomes, 1 to 200.</param>
        IReadOnlyList<PluginRegistrationEvent> GetRegistrationEvents(int maxCount = 50);

        /// <summary>Returns the most recent application log lines, masked, oldest first.</summary>
        /// <param name="maxCount">Number of lines, 1 to 1000.</param>
        IReadOnlyList<PluginLogEntry> GetRecentLogs(int maxCount = 200);
    }
}
