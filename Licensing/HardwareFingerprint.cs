using SoftLicence.SDK;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Passerelle vers le SDK officiel SoftLicence pour l'identité matérielle.
/// </summary>
public static class HardwareFingerprint
{
    /// <summary>
    /// Obtient l'identifiant matériel unique au format officiel (16 Hexa).
    /// </summary>
    public static string GetHardwareId()
    {
        return HardwareInfo.GetHardwareId();
    }

    /// <summary>
    /// Obtient l'identifiant formaté pour affichage (ex: A1B2-C3D4...).
    /// </summary>
    public static string GetFormattedHardwareId()
    {
        var id = GetHardwareId();
        if (id.Length < 16) return id;
        return $"{id[..4]}-{id[4..8]}-{id[8..12]}-{id[12..16]}";
    }
}
