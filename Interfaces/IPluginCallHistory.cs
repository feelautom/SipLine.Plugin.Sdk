namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Interface to access call history records.
    /// </summary>
    public interface IPluginCallHistory
    {
        /// <summary>
        /// Retrieves the most recent calls.
        /// </summary>
        /// <param name="count">Number of calls to retrieve (max 100)</param>
        /// <returns>List of calls from newest to oldest</returns>
        IReadOnlyList<CallHistoryEntry> GetRecentCalls(int count = 50);

        /// <summary>
        /// Retrieves calls within a specific time period.
        /// </summary>
        IReadOnlyList<CallHistoryEntry> GetCallsBetween(DateTime from, DateTime to);

        /// <summary>
        /// Retrieves all calls associated with a specific phone number.
        /// </summary>
        IReadOnlyList<CallHistoryEntry> GetCallsForNumber(string phoneNumber);

        /// <summary>
        /// Total number of calls in the history.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Event triggered when a new call is added to the history.
        /// </summary>
        event Action<CallHistoryEntry>? OnCallAdded;
    }

    /// <summary>
    /// Represents an entry in the call history.
    /// </summary>
    public sealed class CallHistoryEntry
    {
        /// <summary>Unique record identifier</summary>
        public string Id { get; set; } = "";

        /// <summary>Remote phone number (caller or callee depending on direction)</summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>Contact name (if found in contacts)</summary>
        public string? ContactName { get; set; }

        /// <summary>Call direction</summary>
        public CallDirection Direction { get; set; }

        /// <summary>Date and time of the call</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Total call duration</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Indicates if the call was answered</summary>
        public bool WasAnswered { get; set; }

        /// <summary>Indicates if the call was recorded</summary>
        public bool WasRecorded { get; set; }

        /// <summary>Path to the audio recording file (if recorded)</summary>
        public string? RecordingPath { get; set; }

        /// <summary>User notes associated with the call</summary>
        public string? Notes { get; set; }
    }
}
