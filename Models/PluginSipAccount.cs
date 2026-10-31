namespace SipLine.Plugin.Sdk.Models;

/// <summary>
/// SIP transport of an account.
/// </summary>
public enum PluginSipTransport
{
    /// <summary>UDP.</summary>
    Udp,
    /// <summary>TCP.</summary>
    Tcp,
    /// <summary>TLS (SIP over TLS).</summary>
    Tls
}

/// <summary>
/// Copy of a SIP account's settings (SDK 1.4.0). The password is never exposed: only <see cref="HasPassword"/>.
/// </summary>
public sealed class PluginSipAccount
{
    /// <summary>Stable account identifier.</summary>
    public string Id { get; set; } = "";

    /// <summary>Name shown in SipLine.</summary>
    public string AccountName { get; set; } = "";

    /// <summary>Whether the account is enabled.</summary>
    public bool IsEnabled { get; set; }

    /// <summary>Whether the account is the active account used to place calls.</summary>
    public bool IsActive { get; set; }

    /// <summary>SIP user (extension or number).</summary>
    public string SipUser { get; set; } = "";

    /// <summary>Authentication user when it differs from <see cref="SipUser"/>.</summary>
    public string? AuthUser { get; set; }

    /// <summary>Display name sent with calls.</summary>
    public string? DisplayName { get; set; }

    /// <summary>Registrar host, without port.</summary>
    public string ServerHost { get; set; } = "";

    /// <summary>Registrar port when one is set in the server address; null means the transport default.</summary>
    public int? ServerPort { get; set; }

    /// <summary>Outbound proxy (host[:port]), or null.</summary>
    public string? Proxy { get; set; }

    /// <summary>SIP transport.</summary>
    public PluginSipTransport Transport { get; set; }

    /// <summary>Registration expiry in seconds.</summary>
    public int RegistrationExpirySeconds { get; set; }

    /// <summary>Whether encrypted media (SDES SRTP) is required for calls.</summary>
    public bool SrtpEnabled { get; set; }

    /// <summary>Whether the TLS certificate of the server is verified.</summary>
    public bool VerifyCertificate { get; set; }

    /// <summary>Whether a password is stored. The password itself is never returned.</summary>
    public bool HasPassword { get; set; }

    /// <summary>Registration state as shown by SipLine (for example "Online", "Offline").</summary>
    public string RegistrationStatus { get; set; } = "";
}

/// <summary>
/// Change or creation of a SIP account (SDK 1.4.0). A null property keeps the current value.
/// </summary>
public sealed class PluginSipAccountUpdate
{
    /// <summary>Account to change; null creates a new account (then <see cref="SipUser"/> and <see cref="ServerHost"/> are required).</summary>
    public string? AccountId { get; set; }

    /// <summary>New name shown in SipLine.</summary>
    public string? AccountName { get; set; }

    /// <summary>Enables or disables the account.</summary>
    public bool? IsEnabled { get; set; }

    /// <summary>New SIP user.</summary>
    public string? SipUser { get; set; }

    /// <summary>New authentication user; an empty string removes it.</summary>
    public string? AuthUser { get; set; }

    /// <summary>New display name; an empty string removes it.</summary>
    public string? DisplayName { get; set; }

    /// <summary>New registrar host, without port.</summary>
    public string? ServerHost { get; set; }

    /// <summary>New registrar port, 1 to 65535; 0 removes the explicit port.</summary>
    public int? ServerPort { get; set; }

    /// <summary>New outbound proxy (host[:port]); an empty string removes it.</summary>
    public string? Proxy { get; set; }

    /// <summary>New transport.</summary>
    public PluginSipTransport? Transport { get; set; }

    /// <summary>New registration expiry, 60 to 3600 seconds.</summary>
    public int? RegistrationExpirySeconds { get; set; }

    /// <summary>Requires or stops requiring SRTP.</summary>
    public bool? SrtpEnabled { get; set; }

    /// <summary>Verifies or stops verifying the TLS certificate.</summary>
    public bool? VerifyCertificate { get; set; }

    /// <summary>New password, write-only. Null keeps the stored password.</summary>
    public string? NewPassword { get; set; }
}

/// <summary>
/// Final registration state observed after applying the settings.
/// </summary>
public enum PluginRegistrationState
{
    /// <summary>The registrar accepted the registration.</summary>
    Registered,
    /// <summary>The registration failed (see <see cref="PluginRegistrationResult.ErrorMessage"/>).</summary>
    Failed,
    /// <summary>No final answer was observed before the timeout.</summary>
    TimedOut,
    /// <summary>The account is disabled, so no registration was attempted.</summary>
    Disabled,
    /// <summary>The account does not exist.</summary>
    NotFound
}

/// <summary>
/// Result of <see cref="IPluginLineSettings.ApplyAndWaitForRegistrationAsync"/>.
/// </summary>
public sealed class PluginRegistrationResult
{
    /// <summary>Account the result is about.</summary>
    public string AccountId { get; set; } = "";

    /// <summary>Observed state.</summary>
    public PluginRegistrationState State { get; set; }

    /// <summary>Error reported by SipLine or the registrar, when the registration failed.</summary>
    public string? ErrorMessage { get; set; }

    /// <summary>SIP status code of the registrar's final answer, when known.</summary>
    public int? SipStatusCode { get; set; }

    /// <summary>Time between applying the settings and the observed state.</summary>
    public TimeSpan Elapsed { get; set; }
}

/// <summary>
/// Result of <see cref="IPluginLineSettings.TestRegistrationAsync"/>.
/// </summary>
public sealed class PluginRegistrationTestResult
{
    /// <summary>Whether the authentication test succeeded.</summary>
    public bool Success { get; set; }

    /// <summary>Message of the test, as SipLine shows it.</summary>
    public string Message { get; set; } = "";
}
