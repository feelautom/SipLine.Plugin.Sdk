namespace SipLine.Plugin.Sdk.Models;

/// <summary>
/// Direction of a SIP message.
/// </summary>
public enum PluginSipDirection
{
    /// <summary>Received by SipLine.</summary>
    In,
    /// <summary>Sent by SipLine.</summary>
    Out
}

/// <summary>
/// One SIP message of the trace kept in memory by SipLine (SDK 1.4.0). Authentication headers, user parts of
/// URIs, display names and SRTP keys are masked.
/// </summary>
public sealed class PluginSipTraceEntry
{
    /// <summary>Time the message was sent or received (UTC).</summary>
    public DateTime TimestampUtc { get; set; }

    /// <summary>Direction of the message.</summary>
    public PluginSipDirection Direction { get; set; }

    /// <summary>Remote endpoint (transport:address:port).</summary>
    public string Remote { get; set; } = "";

    /// <summary>Call-ID header of the message.</summary>
    public string CallId { get; set; } = "";

    /// <summary>Request method, or the CSeq method for a response (INVITE, REGISTER, BYE...).</summary>
    public string Method { get; set; } = "";

    /// <summary>Status code for a response; null for a request.</summary>
    public int? StatusCode { get; set; }

    /// <summary>Reason phrase for a response.</summary>
    public string? ReasonPhrase { get; set; }

    /// <summary>First line of the message.</summary>
    public string StartLine { get; set; } = "";

    /// <summary>Whole masked message, headers and body.</summary>
    public string Text { get; set; } = "";
}

/// <summary>
/// Media and SRTP outcome of a call, rebuilt from its SIP messages (SDK 1.4.0).
/// </summary>
public sealed class PluginCallMediaInfo
{
    /// <summary>Call-ID of the call.</summary>
    public string CallId { get; set; } = "";

    /// <summary>Direction of the call's first INVITE (Out for an outgoing call).</summary>
    public PluginSipDirection Direction { get; set; }

    /// <summary>SDP offer (masked), or null.</summary>
    public string? OfferSdp { get; set; }

    /// <summary>SDP answer (masked), or null when the call was not answered with media.</summary>
    public string? AnswerSdp { get; set; }

    /// <summary>Audio transport profile of the offer (for example RTP/AVP or RTP/SAVP).</summary>
    public string? OfferProfile { get; set; }

    /// <summary>Audio transport profile of the answer.</summary>
    public string? AnswerProfile { get; set; }

    /// <summary>Whether the offer carries an SRTP crypto line.</summary>
    public bool OfferHasCrypto { get; set; }

    /// <summary>Whether the answer carries an SRTP crypto line.</summary>
    public bool AnswerHasCrypto { get; set; }

    /// <summary>Whether encrypted media was agreed: secure profile and crypto line in both offer and answer.</summary>
    public bool SrtpNegotiated { get; set; }

    /// <summary>Final status code of the INVITE (200, 486, 488...), or null when none was seen.</summary>
    public int? FinalStatusCode { get; set; }

    /// <summary>Reason phrase of the final status.</summary>
    public string? FinalReasonPhrase { get; set; }

    /// <summary>Whether a BYE was seen for the call.</summary>
    public bool Ended { get; set; }
}

/// <summary>
/// Registration outcome of a SIP account (SDK 1.4.0).
/// </summary>
public sealed class PluginRegistrationEvent
{
    /// <summary>Time of the outcome (UTC).</summary>
    public DateTime TimestampUtc { get; set; }

    /// <summary>Account the outcome is about.</summary>
    public string AccountId { get; set; } = "";

    /// <summary>Outcome: Registered, Failed or Removed.</summary>
    public string State { get; set; } = "";

    /// <summary>Error reported for a failure.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>SIP status code of the registrar's answer, when known.</summary>
    public int? SipStatusCode { get; set; }
}

/// <summary>
/// One recent application log line, masked (SDK 1.4.0).
/// </summary>
public sealed class PluginLogEntry
{
    /// <summary>Time of the line (UTC).</summary>
    public DateTime TimestampUtc { get; set; }

    /// <summary>Level: TRACE, DEBUG, INFO, WARNING or ERROR.</summary>
    public string Level { get; set; } = "";

    /// <summary>Masked text of the line.</summary>
    public string Message { get; set; } = "";
}
