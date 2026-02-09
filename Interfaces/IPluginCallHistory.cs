namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Interface pour accéder à l'historique des appels.
    /// </summary>
    public interface IPluginCallHistory
    {
        /// <summary>
        /// Récupère les derniers appels.
        /// </summary>
        /// <param name="count">Nombre d'appels à récupérer (max 100)</param>
        /// <returns>Liste des appels du plus récent au plus ancien</returns>
        IReadOnlyList<CallHistoryEntry> GetRecentCalls(int count = 50);

        /// <summary>
        /// Récupère les appels dans une période donnée.
        /// </summary>
        IReadOnlyList<CallHistoryEntry> GetCallsBetween(DateTime from, DateTime to);

        /// <summary>
        /// Récupère les appels pour un numéro spécifique.
        /// </summary>
        IReadOnlyList<CallHistoryEntry> GetCallsForNumber(string phoneNumber);

        /// <summary>
        /// Nombre total d'appels dans l'historique.
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Événement déclenché quand un nouvel appel est ajouté à l'historique.
        /// </summary>
        event Action<CallHistoryEntry>? OnCallAdded;
    }

    /// <summary>
    /// Représente une entrée dans l'historique des appels.
    /// </summary>
    public sealed class CallHistoryEntry
    {
        /// <summary>
        public string Id { get; set; } = "";

        /// <summary>Numéro distant (appelant ou appelé selon direction)</summary>
        public string PhoneNumber { get; set; } = "";

        /// <summary>Nom du contact (si trouvé)</summary>
        public string? ContactName { get; set; }

        /// <summary>Direction de l'appel</summary>
        public CallDirection Direction { get; set; }

        /// <summary>Date et heure de l'appel</summary>
        public DateTime Timestamp { get; set; }

        /// <summary>Durée de l'appel</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>L'appel a été décroché</summary>
        public bool WasAnswered { get; set; }

        /// <summary>L'appel a été enregistré</summary>
        public bool WasRecorded { get; set; }

        /// <summary>Chemin du fichier d'enregistrement (si enregistré)</summary>
        public string? RecordingPath { get; set; }

        /// <summary>Notes associées à l'appel</summary>
        public string? Notes { get; set; }
    }
}
