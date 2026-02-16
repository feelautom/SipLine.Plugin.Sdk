namespace SipLine.Plugin.Sdk.Models
{
    /// <summary>
    /// Areas where a plugin can inject context menu options.
    /// </summary>
    public enum MenuArea
    {
        /// <summary>Right-click on a call log in the History page.</summary>
        History,
        /// <summary>Right-click on a contact in the Contacts page.</summary>
        Contacts,
        /// <summary>Right-click or options menu during an active call.</summary>
        ActiveCall
    }
}
