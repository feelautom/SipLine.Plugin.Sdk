namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Rights a plugin asks the user for (SDK 1.4.0). The user grants them once, in a SipLine consent window;
/// the host stores the choice outside the plugin's reach and checks it on every call.
/// </summary>
/// <remarks>
/// A commercial plugin keeps the access it had before SDK 1.4.0 (<see cref="CallHistoryRead"/>,
/// <see cref="ContactsRead"/> and <see cref="CallControl"/>) without asking. <see cref="LineSettings"/> and
/// <see cref="Diagnostics"/> always require the user's consent, whatever the license.
/// </remarks>
[Flags]
public enum PluginPermissions
{
    /// <summary>No right beyond the license defaults.</summary>
    None = 0,

    /// <summary>Read the call history (<see cref="IPluginCallHistory"/> queries and new-call events).</summary>
    CallHistoryRead = 1,

    /// <summary>Read the SipLine contacts (<see cref="IPluginContactService.GetContactsAsync"/>).</summary>
    ContactsRead = 2,

    /// <summary>
    /// Place, answer, hold, transfer and hang up calls, send DTMF, read the current call and its events,
    /// and switch Do Not Disturb.
    /// </summary>
    CallControl = 4,

    /// <summary>
    /// Read and change the SIP accounts (never their password), choose the active account, apply the settings and
    /// test the registration (<see cref="IPluginLineSettings"/>), and switch Do Not Disturb.
    /// </summary>
    LineSettings = 8,

    /// <summary>
    /// Read the masked SIP trace, the media and SRTP result of a call, the registration events and the recent
    /// masked application logs (<see cref="IPluginDiagnostics"/>).
    /// </summary>
    Diagnostics = 16,

    /// <summary>Every right defined by this SDK version.</summary>
    All = CallHistoryRead | ContactsRead | CallControl | LineSettings | Diagnostics
}

/// <summary>
/// Request passed to <see cref="IPluginContext.RequestPermissionsAsync"/>.
/// </summary>
public sealed class PluginPermissionRequest
{
    /// <summary>Rights the plugin needs. Unknown bits are ignored.</summary>
    public PluginPermissions Requested { get; set; }

    /// <summary>
    /// Short sentence shown to the user in the consent window, explaining why the plugin needs these rights.
    /// </summary>
    public string? Reason { get; set; }

    /// <summary>
    /// Shows the consent window even when every requested right is already granted, so the user can review or
    /// withdraw rights (for example from a "Change permissions" button). Without it, a request already fully granted
    /// returns at once without any window: consent is asked once.
    /// </summary>
    public bool AlwaysAsk { get; set; }
}

/// <summary>
/// Thrown by the SDK 1.4.0 services (<see cref="IPluginLineSettings"/>, <see cref="IPluginDiagnostics"/>) when
/// the plugin calls them without the right the user must grant.
/// </summary>
public sealed class PluginPermissionException : InvalidOperationException
{
    /// <summary>Creates the exception for the missing right.</summary>
    /// <param name="required">Right the operation needs.</param>
    /// <param name="operation">Name of the refused operation, for the message.</param>
    public PluginPermissionException(PluginPermissions required, string operation)
        : base($"The operation '{operation}' requires the permission '{required}', which the user has not granted.")
    {
        Required = required;
        Operation = operation;
    }

    /// <summary>Right the refused operation needs.</summary>
    public PluginPermissions Required { get; }

    /// <summary>Name of the refused operation.</summary>
    public string Operation { get; }
}
