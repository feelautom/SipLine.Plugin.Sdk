using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Valide les licences de plugins commerciaux.
/// </summary>
public static class LicenseValidator
{
    // Clé publique RSA pour vérifier les signatures (à remplacer par la vraie clé)
    // Cette clé est publique et peut être distribuée avec l'application
    private const string PublicKeyXml = @"
<RSAKeyValue>
    <Modulus>zJ5xQz3qK8Hy8X+8N7L9M6P5R2T4V6W8Y0A1B3C5D7E9F1G3H5I7J9K1L3M5N7O9P1Q3R5S7T9U1V3W5X7Y9Z1a3b5c7d9e1f3g5h7i9j1k3l5m7n9o1p3q5r7s9t1u3v5w7x9y1z=</Modulus>
    <Exponent>AQAB</Exponent>
</RSAKeyValue>";

    /// <summary>
    /// Valide une licence pour un plugin.
    /// </summary>
    /// <param name="licenseJson">Contenu JSON de la licence</param>
    /// <param name="pluginId">ID du plugin à valider</param>
    /// <returns>Résultat de la validation</returns>
    public static LicenseValidationResult Validate(string licenseJson, string pluginId)
    {
        try
        {
            // Parser la licence
            var license = JsonSerializer.Deserialize<PluginLicense>(licenseJson);
            if (license == null)
                return LicenseValidationResult.Invalid("Format de licence invalide");

            // Vérifier le plugin ID
            if (!string.Equals(license.PluginId, pluginId, StringComparison.OrdinalIgnoreCase))
                return LicenseValidationResult.Invalid("Cette licence n'est pas pour ce plugin");

            // Vérifier le Hardware ID
            var currentHardwareId = HardwareFingerprint.GetHardwareId();
            if (!string.Equals(license.HardwareId, currentHardwareId, StringComparison.OrdinalIgnoreCase))
                return LicenseValidationResult.Invalid("Cette licence n'est pas pour cet appareil");

            // Vérifier l'expiration
            if (license.ExpiresAt.HasValue && license.ExpiresAt.Value < DateTime.UtcNow)
                return LicenseValidationResult.Invalid("Cette licence a expiré");

            // Vérifier la signature
            if (!VerifySignature(license))
                return LicenseValidationResult.Invalid("Signature de licence invalide");

            return LicenseValidationResult.Valid(license);
        }
        catch (JsonException)
        {
            return LicenseValidationResult.Invalid("Format de licence invalide");
        }
        catch (Exception ex)
        {
            return LicenseValidationResult.Invalid($"Erreur de validation: {ex.Message}");
        }
    }

    /// <summary>
    /// Valide une licence depuis un fichier.
    /// </summary>
    public static LicenseValidationResult ValidateFile(string licensePath, string pluginId)
    {
        if (!File.Exists(licensePath))
            return LicenseValidationResult.Invalid("Fichier de licence introuvable");

        var licenseJson = File.ReadAllText(licensePath);
        return Validate(licenseJson, pluginId);
    }

    /// <summary>
    /// Vérifie la signature RSA de la licence.
    /// </summary>
    private static bool VerifySignature(PluginLicense license)
    {
        try
        {
            // Créer la chaîne à signer (tout sauf la signature)
            var dataToSign = $"{license.PluginId}|{license.HardwareId}|{license.LicensedTo}|{license.IssuedAt:O}|{license.ExpiresAt:O}";
            var dataBytes = Encoding.UTF8.GetBytes(dataToSign);
            var signatureBytes = Convert.FromBase64String(license.Signature);

            using var rsa = RSA.Create();
            rsa.FromXmlString(PublicKeyXml);

            return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
        catch
        {
            return false;
        }
    }

}
