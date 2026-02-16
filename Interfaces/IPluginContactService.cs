using System.Collections.Generic;
using System.Threading.Tasks;
using SipLine.Plugin.Sdk.Models;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Service to manage contacts within SipLine.
    /// Allows plugins to synchronize data with external systems (CRMs).
    /// </summary>
    public interface IPluginContactService
    {
        /// <summary>
        /// Retrieves all contacts from SipLine's local database.
        /// </summary>
        Task<IEnumerable<PluginContact>> GetContactsAsync();

        /// <summary>
        /// Adds a new contact to SipLine.
        /// </summary>
        Task AddContactAsync(PluginContact contact);

        /// <summary>
        /// Updates an existing contact.
        /// </summary>
        Task UpdateContactAsync(PluginContact contact);

        /// <summary>
        /// Deletes a contact by its unique identifier.
        /// </summary>
        Task DeleteContactAsync(string contactId);
    }
}
