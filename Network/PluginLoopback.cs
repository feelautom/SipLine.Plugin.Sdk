using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace SipLine.Plugin.Sdk.Network;

/// <summary>
/// Local-only listening rule for plugins that open a port (SDK 1.4.0): a plugin listens on the loopback address
/// only, never on a network address, so no other machine can reach it and Windows shows no firewall prompt.
/// </summary>
public static class PluginLoopback
{
    /// <summary>IPv4 loopback address a plugin listener must bind to.</summary>
    public static IPAddress Address => IPAddress.Loopback;

    /// <summary>Tells whether an address is a loopback address (127.0.0.0/8 or ::1).</summary>
    /// <param name="address">Address to check; null is not loopback.</param>
    public static bool IsLoopback(IPAddress? address)
        => address != null && IPAddress.IsLoopback(address);
}

/// <summary>
/// Pairing token between a plugin's local server and its client (SDK 1.4.0): random, compared in constant time.
/// </summary>
public static class PluginPairingToken
{
    /// <summary>Number of random bytes of a token.</summary>
    public const int ByteLength = 32;

    /// <summary>Creates a new random token, base64url without padding (43 ASCII characters).</summary>
    public static string Create()
    {
        var bytes = RandomNumberGenerator.GetBytes(ByteLength);
        return Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    /// <summary>
    /// Compares a presented token with the expected one, exactly (ordinal, case-sensitive), in constant time for
    /// tokens of equal length. An empty expected token never matches.
    /// </summary>
    /// <param name="expected">Token stored by the plugin.</param>
    /// <param name="presented">Token sent by the client.</param>
    public static bool Matches(string? expected, string? presented)
    {
        if (string.IsNullOrEmpty(expected) || presented == null)
            return false;

        var expectedBytes = Encoding.UTF8.GetBytes(expected);
        var presentedBytes = Encoding.UTF8.GetBytes(presented);
        return CryptographicOperations.FixedTimeEquals(expectedBytes, presentedBytes);
    }
}
