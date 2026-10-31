using SoftLicence.SDK;

namespace SipLine.Plugin.Sdk.Licensing;

/// <summary>
/// Provides hardware identity through the official SoftLicence SDK.
/// </summary>
public static class HardwareFingerprint
{
    /// <summary>
    /// Gets the UUID-derived 16-character uppercase hexadecimal identity.
    /// Throws MachineIdentityRefusedException on UUID refusal; no fallback identity is produced.
    /// </summary>
    /// <returns>The accepted canonical SDK hardware identifier.</returns>
    /// <exception cref="MachineIdentityRefusedException">SMBIOS UUID evidence is missing or refused by SDK policy.</exception>
    public static string GetHardwareId()
    {
        var identity = Resolve();
        if (!identity.IsAccepted) throw new MachineIdentityRefusedException(identity);
        return identity.HardwareId!;
    }

    /// <summary>Resolves the official UUID contract without throwing on a refused device.</summary>
    /// <returns>The SDK result including an accepted identifier or stable refusal/support codes.</returns>
    /// <remarks>Reads local machine evidence only and never supplies a legacy fallback.</remarks>
    public static MachineIdentityResult Resolve() => MachineIdentity.Resolve();

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
