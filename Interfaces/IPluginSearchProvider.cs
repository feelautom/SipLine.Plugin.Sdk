using System.Collections.Generic;
using System.Threading.Tasks;
using SipLine.Plugin.Sdk.Models;

namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Interface for plugins that want to extend the global search capabilities of SipLine.
    /// </summary>
    public interface IPluginSearchProvider
    {
        /// <summary>
        /// Name of the search provider (e.g., "HubSpot", "Zendesk").
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// Performs a search based on the user query.
        /// </summary>
        /// <param name="query">The search term (name, number, etc.)</param>
        /// <returns>A list of search results</returns>
        Task<IEnumerable<PluginSearchResult>> SearchAsync(string query);
    }
}
