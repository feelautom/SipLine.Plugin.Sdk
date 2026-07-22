using SoftLicence.SDK;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Provides hardware identity through the official SoftLicence SDK.
/// </summary>
public static class HardwareFingerprint
{
    /// <summary>
    /// Gets the unique hardware identifier in the official 16-character hexadecimal format.
    /// </summary>
    public static string GetHardwareId()
    {
        return HardwareInfo.GetHardwareId();
    }

    /// <summary>
    /// Gets the hardware identifier formatted for display (for example, A1B2-C3D4...).
    /// </summary>
    public static string GetFormattedHardwareId()
    {
        var id = GetHardwareId();
        if (id.Length < 16) return id;
        return $"{id[..4]}-{id[4..8]}-{id[8..12]}-{id[12..16]}";
    }
}
