using SipLine.Plugin.Sdk.Models;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// SIP line settings exposed to plugins (SDK 1.4.0). Every member requires
    /// <see cref="Licensing.PluginPermissions.LineSettings"/> and throws <see cref="Licensing.PluginPermissionException"/>
    /// without it. Members may be called from any thread.
    /// </summary>
    public interface IPluginLineSettings
    {
        /// <summary>Returns a copy of every SIP account, without passwords.</summary>
        IReadOnlyList<PluginSipAccount> GetAccounts();

        /// <summary>Identifier of the active account, or null when there is none.</summary>
        string? ActiveAccountId { get; }

        /// <summary>
        /// Changes or creates an account and saves it, as the settings screen does. The new settings are used at the
        /// next registration: call <see cref="ApplyAndWaitForRegistrationAsync"/> to apply them.
        /// </summary>
        /// <param name="update">Values to change; null values are kept.</param>
        /// <returns>The account as saved.</returns>
        /// <exception cref="ArgumentException">A value is invalid (transport, port, expiry, missing user or server); nothing is saved.</exception>
        /// <exception cref="KeyNotFoundException">The account does not exist.</exception>
        Task<PluginSipAccount> UpdateAccountAsync(PluginSipAccountUpdate update);

        /// <summary>Makes an enabled account the active account used to place calls.</summary>
        /// <exception cref="KeyNotFoundException">The account does not exist.</exception>
        /// <exception cref="InvalidOperationException">The account is disabled.</exception>
        Task SetActiveAccountAsync(string accountId);

        /// <summary>
        /// Re-registers every enabled account with the saved settings and waits for the real outcome of one account:
        /// the registrar's acceptance or failure, not the label shown on screen.
        /// </summary>
        /// <param name="accountId">Account to wait for; null waits for the active account.</param>
        /// <param name="timeout">Longest wait, 1 to 120 seconds.</param>
        /// <param name="cancellationToken">Stops waiting; the registration itself continues.</param>
        Task<PluginRegistrationResult> ApplyAndWaitForRegistrationAsync(string? accountId, TimeSpan timeout, CancellationToken cancellationToken = default);

        /// <summary>Runs the SIP authentication test of the settings screen with the active account.</summary>
        Task<PluginRegistrationTestResult> TestRegistrationAsync();
    }
}
