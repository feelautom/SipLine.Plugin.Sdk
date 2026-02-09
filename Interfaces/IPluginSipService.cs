namespace SipLine.Plugin.Sdk
{
    /// <summary>
    /// Interface SIP exposée aux plugins.
    /// Fournit un accès en lecture aux informations d'appel et des événements.
    /// </summary>
    public interface IPluginSipService
    {
        #region Événements

        /// <summary>Appel entrant reçu</summary>
        event Action<CallInfo>? OnCallIncoming;

        /// <summary>Appel sortant initié</summary>
        event Action<CallInfo>? OnCallOutgoing;

        /// <summary>Appel décroché (entrant ou sortant)</summary>
        event Action<CallInfo>? OnCallAnswered;

        /// <summary>Appel terminé</summary>
        event Action<CallEndedInfo>? OnCallEnded;

        /// <summary>Appel mis en attente</summary>
        event Action<CallInfo>? OnCallHeld;

        /// <summary>Appel repris (fin d'attente)</summary>
        event Action<CallInfo>? OnCallResumed;

        /// <summary>Touche DTMF reçue</summary>
        event Action<DtmfInfo>? OnDtmfReceived;

        /// <summary>Statut d'enregistrement SIP changé</summary>
        event Action<RegistrationStatus>? OnRegistrationChanged;

        #endregion

        #region État actuel

        /// <summary>Statut d'enregistrement actuel</summary>
        RegistrationStatus RegistrationStatus { get; }

        /// <summary>Un appel est en cours</summary>
        bool IsInCall { get; }

        /// <summary>Informations sur l'appel en cours (null si pas d'appel)</summary>
        CallInfo? CurrentCall { get; }

        /// <summary>Numéro SIP de l'utilisateur</summary>
        string? SipUser { get; }

        #endregion

        #region Actions (limité pour les plugins)

        /// <summary>
        /// Initie un appel vers un numéro.
        /// </summary>
        /// <param name="destination">Numéro à appeler</param>
        /// <returns>True si l'appel a été initié</returns>
        Task<bool> MakeCallAsync(string destination);

        /// <summary>
        /// Envoie une touche DTMF pendant un appel.
        /// </summary>
        /// <param name="digit">Touche (0-9, *, #)</param>
        void SendDtmf(char digit);

        #endregion
    }

    /// <summary>
    /// Informations sur un appel
    /// </summary>
    public sealed class CallInfo
    {
        /// <summary>Identifiant unique de l'appel</summary>
        public string CallId { get; set; } = "";

        /// <summary>Numéro de l'appelant</summary>
        public string CallerNumber { get; set; } = "";

        /// <summary>Nom de l'appelant (si disponible)</summary>
        public string? CallerName { get; set; }

        /// <summary>Numéro appelé</summary>
        public string CalleeNumber { get; set; } = "";

        /// <summary>Direction de l'appel</summary>
        public CallDirection Direction { get; set; }

        /// <summary>Heure de début de l'appel</summary>
        public DateTime StartTime { get; set; }

        /// <summary>L'appel est en attente</summary>
        public bool IsOnHold { get; set; }

        /// <summary>L'appel est enregistré</summary>
        public bool IsRecording { get; set; }
    }

    /// <summary>
    /// Informations sur la fin d'un appel
    /// </summary>
    public sealed class CallEndedInfo
    {
        /// <summary>Informations de l'appel</summary>
        public CallInfo Call { get; set; } = new();

        /// <summary>Durée de l'appel</summary>
        public TimeSpan Duration { get; set; }

        /// <summary>Raison de la fin</summary>
        public CallEndReason Reason { get; set; }

        /// <summary>Code SIP de fin (ex: 200, 486, 603)</summary>
        public int? SipCode { get; set; }
    }

    /// <summary>
    /// Informations DTMF
    /// </summary>
    public sealed class DtmfInfo
    {
        /// <summary>Touche pressée</summary>
        public char Digit { get; set; }

        /// <summary>Durée en millisecondes</summary>
        public int DurationMs { get; set; }

        /// <summary>Identifiant de l'appel</summary>
        public string CallId { get; set; } = "";
    }

    /// <summary>
    /// Direction de l'appel
    /// </summary>
    public enum CallDirection
    {
        Incoming,
        Outgoing
    }

    /// <summary>
    /// Raison de fin d'appel
    /// </summary>
    public enum CallEndReason
    {
        /// <summary>Raccroché normalement</summary>
        Normal,
        /// <summary>Occupé</summary>
        Busy,
        /// <summary>Pas de réponse</summary>
        NoAnswer,
        /// <summary>Refusé</summary>
        Rejected,
        /// <summary>Erreur réseau</summary>
        NetworkError,
        /// <summary>Annulé par l'appelant</summary>
        Cancelled,
        /// <summary>Autre raison</summary>
        Other
    }

    /// <summary>
    /// Statut d'enregistrement SIP
    /// </summary>
    public enum RegistrationStatus
    {
        /// <summary>Non enregistré</summary>
        Unregistered,
        /// <summary>En cours d'enregistrement</summary>
        Registering,
        /// <summary>Enregistré avec succès</summary>
        Registered,
        /// <summary>Échec d'enregistrement</summary>
        Failed
    }
}
