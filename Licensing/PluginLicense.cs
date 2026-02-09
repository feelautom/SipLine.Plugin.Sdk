using System.Text.Json.Serialization;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Représente une licence de plugin commercial.
/// </summary>
public class PluginLicense
{
    /// <summary>
    /// Identifiant du plugin concerné.
    /// </summary>
    [JsonPropertyName("pluginId")]
    public string PluginId { get; set; } = "";

    /// <summary>
    /// Hardware ID de l'appareil autorisé.
    /// </summary>
    [JsonPropertyName("hardwareId")]
    public string HardwareId { get; set; } = "";

    /// <summary>
    /// Nom du titulaire de la licence.
    /// </summary>
    [JsonPropertyName("licensedTo")]
    public string LicensedTo { get; set; } = "";

    /// <summary>
    /// Email du titulaire.
    /// </summary>
    [JsonPropertyName("email")]
    public string? Email { get; set; }

    /// <summary>
    /// Date d'émission de la licence.
    /// </summary>
    [JsonPropertyName("issuedAt")]
    public DateTime IssuedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Date d'expiration (null = licence perpétuelle).
    /// </summary>
    [JsonPropertyName("expiresAt")]
    public DateTime? ExpiresAt { get; set; }

    /// <summary>
    /// Version du plugin couverte (null = toutes versions).
    /// </summary>
    [JsonPropertyName("version")]
    public string? Version { get; set; }

    /// <summary>
    /// Fonctionnalités activées (null = toutes).
    /// </summary>
    [JsonPropertyName("features")]
    public List<string>? Features { get; set; }

    /// <summary>
    /// Signature RSA de la licence (Base64).
    /// </summary>
    [JsonPropertyName("signature")]
    public string Signature { get; set; } = "";
}

/// <summary>
/// Résultat de la validation d'une licence.
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
/// Type de licence d'un plugin.
/// </summary>
public enum PluginLicenseType
{
    /// <summary>
    /// Plugin gratuit intégré (pas de licence requise).
    /// </summary>
    Integrated,

    /// <summary>
    /// Plugin communautaire gratuit (pas de licence requise).
    /// </summary>
    Community,

    /// <summary>
    /// Plugin commercial (licence requise).
    /// </summary>
    Commercial
}
