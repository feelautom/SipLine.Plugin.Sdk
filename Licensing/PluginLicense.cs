using System.Text.Json.Serialization;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Represents a commercial plugin license.
/// </summary>
public class PluginLicense
{
    /// <summary>
    /// Identifier of the licensed plugin.
    /// </summary>
    [JsonPropertyName("pluginId")]
    public string PluginId { get; set; } = "";

    /// <summary>
    /// Hardware identifier of the licensed device.
    /// </summary>
    [JsonPropertyName("hardwareId")]
    public string HardwareId { get; set; } = "";

    /// <summary>
    /// Name of the license holder.
    /// </summary>
    [JsonPropertyName("licensedTo")]
    public string LicensedTo { get; set; } = "";

    /// <summary>
    /// Email address of the license holder.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Date when the license was issued.
    /// </summary>
    [JsonPropertyName("issuedAt")]
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Expiration date, or null for a perpetual license.
    /// </summary>
    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Covered plugin version, or null for every version.
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Minimum required SipLine version (for example, 1.0.0).
    /// </summary>
    [JsonPropertyName("minAppVersion")]
    public string? MinAppVersion { get; set; }

    /// <summary>
    /// Exact plugin version allowed by the license, when specified.
    /// </summary>
    [JsonPropertyName("pluginVersion")]
    public string? PluginVersion { get; set; }

    /// <summary>
    /// Enabled features, or null for every feature.
    /// </summary>
    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }

    /// <summary>
    /// Base64-encoded RSA signature of the license.
    /// </summary>
    [JsonPropertyName("signature")]
    public string Signature { get; set; } = "";
}

/// <summary>
/// Result of validating a license.
/// </summary>
public class LicenseValidationResult
{
    public bool IsValid { get; set; }
    public string? ErrorMessage { get; set; }
    public PluginLicense? License { get; set; }

    public static LicenseValidationResult Valid(PluginLicense license) => new()
    {
        IsValid = true,
        License = license
    };

    public static LicenseValidationResult Invalid(string message) => new()
    {
        IsValid = false,
        ErrorMessage = message
    };
}

/// <summary>
/// Type of plugin license.
/// </summary>
public enum PluginLicenseType
{
    /// <summary>
    /// Free built-in plugin that does not require a license.
    /// </summary>
    Integrated,

    /// <summary>
    /// Free community plugin that does not require a license.
    /// </summary>
    Community,

    /// <summary>
    /// Commercial plugin that requires a license.
    /// </summary>
    Commercial
}
