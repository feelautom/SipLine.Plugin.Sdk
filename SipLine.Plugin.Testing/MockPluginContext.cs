using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using SipLine.Plugin.Sdk;
using SipLine.Plugin.Sdk.Enums;
using SipLine.Plugin.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace SipLine.Plugin.Testing
{
    public class MockPluginContext : IPluginContext
    {
        public IPluginSipService SipService { get; set; } = new MockSipService();
        public IPluginCallHistory CallHistory { get; set; } = new MockCallHistory();
        public IPluginContactService Contacts { get; set; } = new MockContactService();
        public IPluginAudioService Audio { get; set; } = new MockAudioService();
        public ILogger Logger { get; set; } = NullLogger.Instance;
        public string PluginDataPath { get; set; } = Path.Combine(Path.GetTempPath(), "SipLineTest");
        public IPluginLocalization Localization { get; set; } = new MockLocalization();

        public List<string> Notifications { get; } = new();
        public List<string> Logs { get; } = new();
        public Dictionary<string, object> Settings { get; } = new();
        public List<IPluginSearchProvider> SearchProviders { get; } = new();
        public List<(MenuArea Area, string Label)> ContextMenuOptions { get; } = new();

        public event Action<string>? OnLanguageChanged;
        public event Action? OnDevicesChanged;

        public void ShowNotification(string title, string message, NotificationSeverity severity = NotificationSeverity.Info)
        {
            Notifications.Add($"[{severity}] {title}: {message}");
        }

        public void ShowSnackbar(string message, SnackbarSeverity severity = SnackbarSeverity.Info)
        {
            Notifications.Add($"[Snackbar-{severity}] {message}");
        }

        public Task<bool> ShowDialogAsync(string title, string message, string primaryButton = "OK", string secondaryButton = "Cancel")
        {
            Logs.Add($"[Dialog] {title}: {message} ({primaryButton}/{secondaryButton})");
            return Task.FromResult(true); // Toujours true en mock
        }

        public Task<string?> ShowInputAsync(string title, string message, string defaultValue = "", string primaryButton = "OK", string secondaryButton = "Cancel")
        {
            Logs.Add($"[Input] {title}: {message} (Default: {defaultValue})");
            return Task.FromResult<string?>(defaultValue); // Retourne la valeur par défaut en mock
        }

        public void AddLog(string message, string level = "Info")
        {
            Logs.Add($"[{level}] {message}");
        }

        public void RegisterToolbarButton(PluginToolbarButton button) { }
        public void UnregisterToolbarButton(string buttonId) { }
        public void RegisterSettingsTab(PluginSettingsTab tab) { }
        public void UnregisterSettingsTab(string tabId) { }
        public void RegisterSidebarTab(PluginSidebarTab tab) { }
        public void UnregisterSidebarTab(string tabId) { }
        public void SelectSidebarTab(string tabId) { }

        public void RegisterSearchProvider(IPluginSearchProvider provider) => SearchProviders.Add(provider);
        public void UnregisterSearchProvider(string providerName) => SearchProviders.RemoveAll(p => p.ProviderName == providerName);
        
        public void RegisterContextMenuOption(MenuArea area, string label, Action<object> callback) 
            => ContextMenuOptions.Add((area, label));

        public void OpenPluginView(PluginViewRequest request) { }
        public void ClosePluginView() { }

        public T? GetSetting<T>(string key, T? defaultValue = default)
        {
            if (Settings.TryGetValue(key, out var value))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return defaultValue;
        }

        public void SetSetting<T>(string key, T value)
        {
            Settings[key] = value!;
        }

        public string GetLocalizedString(string key) => Localization.GetString(key);

        public void RegisterResource(System.Resources.ResourceManager resourceManager) { }

        public void RegisterSettingsFields(IEnumerable<PluginSettingsField> fields) { }

        public bool AreRequiredSettingsFilled => true;

        public bool IsDarkTheme { get; set; } = true;

        public string AppVersion { get; set; } = "1.0.0-mock";

        public Task RunOnUIThread(Action action)
        {
            action();
            return Task.CompletedTask;
        }

        public Task<T> RunOnUIThread<T>(Func<T> function)
        {
            return Task.FromResult(function());
        }

        public void TriggerLanguageChanged(string lang) => OnLanguageChanged?.Invoke(lang);
        public void TriggerDevicesChanged() => OnDevicesChanged?.Invoke();
    }

    public class MockSipService : IPluginSipService
    {
        public event Action<CallInfo>? OnCallIncoming;
        public event Action<CallEndedInfo>? OnCallEnded;
        public event Action<CallInfo>? OnCallOutgoing;
        public event Action<RegistrationStatus>? OnRegistrationChanged;
        public event Action<CallInfo>? OnCallAnswered;
        public event Action<CallInfo>? OnCallHeld;
        public event Action<CallInfo>? OnCallResumed;
        public event Action<DtmfInfo>? OnDtmfReceived;
        public event Action<bool>? OnDndChanged;

        public RegistrationStatus RegistrationStatus { get; set; } = RegistrationStatus.Registered;
        public bool IsInCall { get; set; }
        public CallInfo? CurrentCall { get; set; }
        public string? SipUser { get; set; } = "1000";
        public bool IsDndEnabled
        {
            get => _isDndEnabled;
            set { _isDndEnabled = value; OnDndChanged?.Invoke(value); }
        }
        private bool _isDndEnabled;

        public Task<bool> MakeCallAsync(string destination)
        {
            OnCallOutgoing?.Invoke(new CallInfo { CalleeNumber = destination, Direction = CallDirection.Outgoing });
            return Task.FromResult(true);
        }

        public void SendDtmf(char digit) 
        {
            OnDtmfReceived?.Invoke(new DtmfInfo { Digit = digit, DurationMs = 100 });
        }

        public Task HangupCallAsync(string callId) => Task.CompletedTask;
        public Task SetHoldAsync(string callId, bool hold) => Task.CompletedTask;
        public Task TransferCallAsync(string callId, string destination) => Task.CompletedTask;

        public void TriggerIncomingCall(string from, string to)
        {
            var info = new CallInfo { CallerNumber = from, CalleeNumber = to, Direction = CallDirection.Incoming };
            CurrentCall = info;
            IsInCall = true;
            OnCallIncoming?.Invoke(info);
        }

        public void TriggerCallEnded()
        {
            if (CurrentCall != null)
            {
                OnCallEnded?.Invoke(new CallEndedInfo { Call = CurrentCall, Reason = CallEndReason.Normal });
                CurrentCall = null;
                IsInCall = false;
            }
        }

        public void TriggerRegistrationChanged(RegistrationStatus status) => OnRegistrationChanged?.Invoke(status);
        public void TriggerCallAnswered(CallInfo call) => OnCallAnswered?.Invoke(call);
        public void TriggerCallHeld(CallInfo call) => OnCallHeld?.Invoke(call);
        public void TriggerCallResumed(CallInfo call) => OnCallResumed?.Invoke(call);

        /// <summary>
        /// Clears all event subscriptions.
        /// </summary>
        public void Reset()
        {
            OnCallIncoming = null;
            OnCallEnded = null;
            OnCallOutgoing = null;
            OnRegistrationChanged = null;
            OnCallAnswered = null;
            OnCallHeld = null;
            OnCallResumed = null;
            OnDtmfReceived = null;
        }
    }

    public class MockCallHistory : IPluginCallHistory
    {
        public int TotalCount => 0;
        public event Action<CallHistoryEntry>? OnCallAdded;

        public void TriggerCallAdded(CallHistoryEntry entry) => OnCallAdded?.Invoke(entry);

        public IReadOnlyList<CallHistoryEntry> GetRecentCalls(int count = 50) => new List<CallHistoryEntry>();
        public IReadOnlyList<CallHistoryEntry> GetCallsBetween(DateTime from, DateTime to) => new List<CallHistoryEntry>();
        public IReadOnlyList<CallHistoryEntry> GetCallsForNumber(string phoneNumber) => new List<CallHistoryEntry>();

        public void Reset()
        {
            OnCallAdded = null;
        }
    }

    public class MockContactService : IPluginContactService
    {
        public List<PluginContact> LocalContacts { get; } = new();
        public Task<IEnumerable<PluginContact>> GetContactsAsync() => Task.FromResult((IEnumerable<PluginContact>)LocalContacts);
        public Task AddContactAsync(PluginContact contact) { LocalContacts.Add(contact); return Task.CompletedTask; }
        public Task UpdateContactAsync(PluginContact contact) { return Task.CompletedTask; }
        public Task DeleteContactAsync(string contactId) { LocalContacts.RemoveAll(c => c.Id == contactId); return Task.CompletedTask; }
    }

    public class MockAudioService : IPluginAudioService
    {
        public int Volume { get; set; } = 100;
        public bool IsMuted { get; private set; }
        public void SetMute(bool mute) { IsMuted = mute; OnMuteChanged?.Invoke(mute); }
        public event Action<int>? OnVolumeChanged;
        public event Action<bool>? OnMuteChanged;
        public void TriggerVolumeChanged(int vol) => OnVolumeChanged?.Invoke(vol);
    }

    public class MockLocalization : IPluginLocalization
    {
        public CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentUICulture;
        public string GetString(string key) => key;
        public string GetString(string key, params object[] args) => string.Format(key, args);
    }
}
