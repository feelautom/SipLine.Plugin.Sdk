using System.IO;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using SoftLicence.SDK;

[assembly: InternalsVisibleTo("SipLine.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100dd5e11152b4fa53fc0ab3fa298c5d4c054aab8a8084238c118dac09950096ea589bcc708e78fdfae79268dcbe05dbf868ccce407b7d1895e9d2b245d441f38f538c08cddf404c2301ce04a1a62eea08f8c53cb4c03518b183b173e0dc9da6c91a932abd3d24509779e9ce4cc26b0e68363ce8735d9dfdfc1d1e6e61a321574c9")]

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Validates commercial plugin licenses.
/// </summary>
public static class LicenseValidator
{
    // Clé publique RSA pour vérifier les signatures des licences plugins
    // Cette clé est publique et distribuée avec l'application
    public const string PublicKeyXml = "<RSAKeyValue><Modulus>w0/oowAxPqkMan/gq2EoT5HFgx2xCHR6ANDq8i8HBHHVTLwk4LkTF8ECLAIpGmCvwwpGnaIU/SiotjJ4rhF37tz4lPn2cReyOxDtSK8v1n3BxI6RH6GOaS192zlLBzYeJey74TV7IZXYwFfLdhHrzi2rOyNMea179vEbLaqh6+rZFLhX6weJPu0HnIAqDOnU74RG2nWIVKcftG6akbf3YjwUXycm1ywGxy8RMbWzjM6afPUrlsL0IxgRebzD2FF/wtQqE+YHEjs7t7QPG/1IzHDNKXJctftXxnyajSbe6hfuuqBp5A86thnulBOJ0E5MNYZNQJziYkGEt+MiGxDslkoqdGtX8L4vViJIY/RGHKeNeucfMvydLaoOtGz42/WaD3aOZOdUxdr+1rGZE1//yU9h7pl/ZDYRwKLiffgLEtagITHUkQ6aF2hLu3f0trIKX9JvHDoUdnD0QM/Dr06gjWyexM/el5e8k6+PBI8QCqkDM5l+rW90nIC0idtXQURGX6VgYLtp18PmfSvMK0+V5f5xk5ZqFiSfN+Rh2VHF12IJ5kJa9uBLQMuwUtx02dn9e4ijFvoFrcrtb1NykhIsMjxmM5u1HktoojEXKpmKxtayX2D3YorBwJa0ryHjKGWHP4AxUxYN3EtLb1lXJu6zJRHLsFY0wIUHbs3UeSDALlM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

    /// <summary>
    /// Validates a license for a plugin.
    /// </summary>
    /// <param name="licenseContent">Official Base64 license content or raw signed JSON.</param>
    /// <param name="pluginId">Identifier of the plugin to validate.</param>
    /// <returns>The license validation result.</returns>
    public static LicenseValidationResult Validate(string licenseContent, string pluginId)
    {
        try
        {
            return ValidateCore(
                licenseContent,
                pluginId,
                appVersion: null,
                pluginVersion: null,
                PublicKeyXml,
                HardwareFingerprint.GetHardwareId());
        }
        catch
        {
            return LicenseValidationResult.Invalid("Licence invalide");
        }
    }

    /// <summary>
    /// Validates a license and enforces its signed version constraints.
    /// </summary>
    public static LicenseValidationResult Validate(
        string licenseContent,
        string pluginId,
        string appVersion,
        string pluginVersion)
    {
        try
        {
            return ValidateCore(
                licenseContent,
                pluginId,
                appVersion,
                pluginVersion,
                PublicKeyXml,
                HardwareFingerprint.GetHardwareId());
        }
        catch
        {
            return LicenseValidationResult.Invalid("Licence invalide");
        }
    }

    internal static LicenseValidationResult ValidateCore(
        string licenseContent,
        string pluginId,
        string? appVersion,
        string? pluginVersion,
        string publicKeyXml,
        string currentHardwareId)
    {
        if (string.IsNullOrWhiteSpace(pluginId) ||
            string.IsNullOrWhiteSpace(publicKeyXml) ||
            string.IsNullOrWhiteSpace(currentHardwareId) ||
            !TryGetSdkTransport(licenseContent, out var sdkTransport))
        {
            return LicenseValidationResult.Invalid("Format de licence invalide");
        }

        SoftLicence.SDK.LicenseModel license;
        try
        {
            var validation = LicenseService.ValidateLicense(sdkTransport, publicKeyXml, currentHardwareId);
            if (!validation.IsValid || validation.License == null)
                return LicenseValidationResult.Invalid("Licence invalide");

            license = validation.License;
        }
        catch
        {
            return LicenseValidationResult.Invalid("Licence invalide");
        }

        if (string.IsNullOrWhiteSpace(license.PluginId) ||
            !string.Equals(license.PluginId, pluginId, StringComparison.OrdinalIgnoreCase))
        {
            return LicenseValidationResult.Invalid("Cette licence n'est pas pour ce plugin");
        }

        if (!ValidateVersionConstraints(
                license.MinAppVersion,
                license.PluginVersion,
                appVersion,
                pluginVersion,
                out var versionError))
        {
            return LicenseValidationResult.Invalid(versionError);
        }

        return LicenseValidationResult.Valid(new PluginLicense
        {
            PluginId = license.PluginId,
            HardwareId = license.HardwareId,
            LicensedTo = license.CustomerName,
            IssuedAt = license.CreationDate,
            ExpiresAt = license.ExpirationDate,
            MinAppVersion = license.MinAppVersion,
            PluginVersion = license.PluginVersion,
            Features = license.AllowedFeatures?.ToList(),
            Signature = string.Empty
        });
    }

    /// <summary>
    /// Validates a license from a file.
    /// </summary>
    public static LicenseValidationResult ValidateFile(string licensePath, string pluginId)
    {
        try
        {
            if (!File.Exists(licensePath))
                return LicenseValidationResult.Invalid("Fichier de licence introuvable");

            return Validate(File.ReadAllText(licensePath), pluginId);
        }
        catch
        {
            return LicenseValidationResult.Invalid("Licence invalide");
        }
    }

    /// <summary>
    /// Validates a license from a file and enforces its signed version constraints.
    /// </summary>
    public static LicenseValidationResult ValidateFile(
        string licensePath,
        string pluginId,
        string appVersion,
        string pluginVersion)
    {
        try
        {
            if (!File.Exists(licensePath))
                return LicenseValidationResult.Invalid("Fichier de licence introuvable");

            return Validate(File.ReadAllText(licensePath), pluginId, appVersion, pluginVersion);
        }
        catch
        {
            return LicenseValidationResult.Invalid("Licence invalide");
        }
    }

    private static bool TryGetSdkTransport(string content, out string sdkTransport)
    {
        sdkTransport = string.Empty;
        if (string.IsNullOrWhiteSpace(content))
            return false;

        var trimmed = content.TrimStart();
        if (trimmed.StartsWith("{", StringComparison.Ordinal))
        {
            if (!IsJsonObject(content))
                return false;

            sdkTransport = Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
            return true;
        }

        try
        {
            var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(content));
            if (!IsJsonObject(decoded))
                return false;

            sdkTransport = content;
            return true;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsJsonObject(string text)
    {
        try
        {
            using var doc = JsonDocument.Parse(text);
            return doc.RootElement.ValueKind == JsonValueKind.Object;
        }
        catch
        {
            return false;
        }
    }

    private static bool ValidateVersionConstraints(
        string? signedMinAppVersion,
        string? signedPluginVersion,
        string? currentAppVersion,
        string? currentPluginVersion,
        out string error)
    {
        error = string.Empty;
        if (!string.IsNullOrWhiteSpace(signedMinAppVersion))
        {
            if (!TryParseVersion(signedMinAppVersion, out var minimum) ||
                !TryParseVersion(currentAppVersion, out var current) ||
                CompareVersions(current, minimum) < 0)
            {
                error = "Version de SipLine incompatible avec la licence";
                return false;
            }
        }

        if (!string.IsNullOrWhiteSpace(signedPluginVersion))
        {
            if (!TryParseVersion(signedPluginVersion, out var licensed) ||
                !TryParseVersion(currentPluginVersion, out var current) ||
                CompareVersions(current, licensed) != 0)
            {
                error = "Version du plugin incompatible avec la licence";
                return false;
            }
        }

        return true;
    }

    private static bool TryParseVersion(string? raw, out int[] parts)
    {
        parts = new int[4];
        if (string.IsNullOrWhiteSpace(raw))
            return false;

        var tokens = raw.Split('.');
        if (tokens.Length is < 1 or > 4)
            return false;

        for (var i = 0; i < tokens.Length; i++)
        {
            if (tokens[i].Length == 0 ||
                tokens[i].Any(character => character is < '0' or > '9') ||
                !int.TryParse(tokens[i], NumberStyles.None, CultureInfo.InvariantCulture, out parts[i]))
                return false;
        }

        return true;
    }

    private static int CompareVersions(IReadOnlyList<int> left, IReadOnlyList<int> right)
    {
        for (var i = 0; i < 4; i++)
        {
            var comparison = left[i].CompareTo(right[i]);
            if (comparison != 0)
                return comparison;
        }

        return 0;
    }
}
