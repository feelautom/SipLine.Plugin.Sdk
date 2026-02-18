namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// SIP interface exposed to plugins.
    /// Provides read-only access to call information and events.
    /// </summary>
    public interface IPluginSipService
    {
        #region Events

        /// <summary>Incoming call received</summary>
        event Action<CallInfo>? OnCallIncoming;

        /// <summary>Outgoing call initiated</summary>
        event Action<CallInfo>? OnCallOutgoing;

        /// <summary>Call answered (incoming or outgoing)</summary>
        event Action<CallInfo>? OnCallAnswered;

        /// <summary>Call ended</summary>
        event Action<CallEndedInfo>? OnCallEnded;

        /// <summary>Call placed on hold</summary>
        event Action<CallInfo>? OnCallHeld;

        /// <summary>Call resumed from hold</summary>
        event Action<CallInfo>? OnCallResumed;

        /// <summary>DTMF digit received</summary>
        event Action<DtmfInfo>? OnDtmfReceived;

        /// <summary>SIP registration status changed</summary>
        event Action<RegistrationStatus>? OnRegistrationChanged;

        /// <summary>Do Not Disturb state changed</summary>
        event Action<bool>? OnDndChanged;

        #endregion

        #region Current State

        /// <summary>Current registration status</summary>
        RegistrationStatus RegistrationStatus { get; }

        /// <summary>Indicates if a call is currently active</summary>
        bool IsInCall { get; }

        /// <summary>Active call information (null if no active call)</summary>
        CallInfo? CurrentCall { get; }

        /// <summary>User's SIP extension/number</summary>
        string? SipUser { get; }

        /// <summary>Do Not Disturb state. When true, incoming calls are rejected.</summary>
        bool IsDndEnabled { get; set; }

        #endregion

        #region Actions

        /// <summary>
        /// Initiates an outgoing call.
        /// </summary>
        /// <param name="destination">Target phone number or extension</param>
        /// <returns>True if the call request was successfully initiated</returns>
        Task<bool> MakeCallAsync(string destination);

        /// <summary>
        /// Sends a DTMF digit during an active call.
        /// </summary>
        /// <param name="digit">Digit to send (0-9, *, #)</param>
        void SendDtmf(char digit);

        /// <summary>
        /// Terminates a specific call.
        /// </summary>
        /// <param name="callId">ID of the call to hang up</param>
        Task HangupCallAsync(string callId);

        /// <summary>
        /// Places a call on hold or resumes it.
        /// </summary>
        /// <param name="callId">ID of the call</param>
        /// <param name="hold">True to hold, false to resume</param>
        Task SetHoldAsync(string callId, bool hold);

        /// <summary>
        /// Transfers a call to another destination (Blind Transfer).
        /// </summary>
        /// <param name="callId">ID of the call to transfer</param>
        /// <param name="destination">Target number or extension</param>
        Task TransferCallAsync(string callId, string destination);

        #endregion
    }

    /// <summary>
    /// Detailed call information
    /// </summary>
    public sealed class CallInfo
    {
        /// <summary>Unique call identifier</summary>
        public string CallId { get; set; } = "";

        /// <summary>Caller's phone number</summary>
        public string CallerNumber { get; set; } = "";

        /// <summary>Caller's display name (if available)</summary>
        public string? CallerName { get; set; }

        /// <summary>Called phone number</summary>
        public string CalleeNumber { get; set; } = "";

        /// <summary>Call direction</summary>
        public CallDirection Direction { get; set; }

        /// <summary>Call start time</summary>
        public DateTime StartTime { get; set; }

        /// <summary>Indicates if the call is on hold</summary>
        public bool IsOnHold { get; set; }

        /// <summary>Indicates if the call is being recorded</summary>
        public bool IsRecording { get; set; }
    }

    /// <summary>
    /// Information about a finished call
    /// </summary>
    public sealed class CallEndedInfo
    {
        /// <summary>Final call information</summary>
        public CallInfo Call { get; set; } = new();

        /// <summary>Total call duration</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Reason for call termination</summary>
        public CallEndReason Reason { get; set; }

        /// <summary>SIP termination code (e.g., 200, 486, 603)</summary>
        public int? SipCode { get; set; }
    }

    /// <summary>
    /// DTMF signal information
    /// </summary>
    public sealed class DtmfInfo
    {
        /// <summary>Pressed digit</summary>
        public char Digit { get; set; }

        /// <summary>Duration in milliseconds</summary>
        public int DurationMs { get; set; }

        /// <summary>Associated call ID</summary>
        public string CallId { get; set; } = "";
    }

    /// <summary>
    /// Call direction
    /// </summary>
    public enum CallDirection
    {
        Incoming,
        Outgoing
    }

    /// <summary>
    /// Reason for call termination
    /// </summary>
    public enum CallEndReason
    {
        /// <summary>Hung up normally</summary>
        Normal,
        /// <summary>Recipient was busy</summary>
        Busy,
        /// <summary>No answer from recipient</summary>
        NoAnswer,
        /// <summary>Call was rejected</summary>
        Rejected,
        /// <summary>Network or server error</summary>
        NetworkError,
        /// <summary>Cancelled by caller</summary>
        Cancelled,
        /// <summary>Other reason</summary>
        Other
    }

    /// <summary>
    /// SIP Registration status
    /// </summary>
    public enum RegistrationStatus
    {
        /// <summary>Not registered</summary>
        Unregistered,
        /// <summary>Registration in progress</summary>
        Registering,
        /// <summary>Successfully registered</summary>
        Registered,
        /// <summary>Registration failed</summary>
        Failed
    }
}
