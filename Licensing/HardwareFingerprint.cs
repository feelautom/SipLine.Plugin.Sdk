using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Génère une empreinte matérielle unique pour l'appareil.
/// </summary>
public static class HardwareFingerprint
{
    private static string? _cachedFingerprint;

    /// <summary>
    /// Obtient l'identifiant matériel unique de l'appareil.
    /// Format: 16 caractères hexadécimaux (ex: A1B2C3D4E5F6G7H8)
    /// </summary>
    public static string GetHardwareId()
    {
        if (_cachedFingerprint != null)
            return _cachedFingerprint;

        var components = new StringBuilder();

        // CPU ID
        components.Append(GetWmiValue("Win32_Processor", "ProcessorId"));

        // Carte mère
        components.Append(GetWmiValue("Win32_BaseBoard", "SerialNumber"));

        // BIOS
        components.Append(GetWmiValue("Win32_BIOS", "SerialNumber"));

        // Disque système
        components.Append(GetWmiValue("Win32_DiskDrive", "SerialNumber"));

        // Nom de la machine (pour différencier les VMs identiques)
        components.Append(Environment.MachineName);

        // Hash SHA256 puis prendre les 16 premiers caractères
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(components.ToString()));
        _cachedFingerprint = Convert.ToHexString(hash)[..16];

        return _cachedFingerprint;
    }

    /// <summary>
    /// Obtient l'identifiant formaté pour affichage (avec tirets).
    /// Format: A1B2-C3D4-E5F6-G7H8
    /// </summary>
    public static string GetFormattedHardwareId()
    {
        var id = GetHardwareId();
        return $"{id[..4]}-{id[4..8]}-{id[8..12]}-{id[12..16]}";
    }

    private static string GetWmiValue(string wmiClass, string property)
    {
        try
        {
            using var searcher = new ManagementObjectSearcher($"SELECT {property} FROM {wmiClass}");
            foreach (var obj in searcher.Get())
            {
                var value = obj[property]?.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                    return value.Trim();
            }
        }
        catch
        {
            // Ignorer les erreurs WMI (certains systèmes n'ont pas toutes les infos)
        }
        return "";
    }
}
