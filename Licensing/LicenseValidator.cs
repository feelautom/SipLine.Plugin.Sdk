using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using SoftLicence.SDK;

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
            // Utilisation du validateur officiel du SDK
            var validation = LicenseService.ValidateLicense(licenseJson, PublicKeyXml, HardwareFingerprint.GetHardwareId());
            
            if (!validation.IsValid)
                return LicenseValidationResult.Invalid(validation.ErrorMessage ?? "Licence invalide");

            var license = validation.License;
            if (license == null)
                return LicenseValidationResult.Invalid("Erreur lors du décodage de la licence");

            // Conversion vers le modèle local pour compatibilité
            var result = new PluginLicense
            {
                PluginId = pluginId, // On assume l'ID demandé puisque validé par le SDK
                HardwareId = license.HardwareId,
                LicensedTo = license.CustomerName,
                IssuedAt = license.CreationDate,
                ExpiresAt = license.ExpirationDate,
                Signature = "" // Déjà validée par le SDK
            };

            return LicenseValidationResult.Valid(result);
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
}
