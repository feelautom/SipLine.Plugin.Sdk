# SipLine Plugin SDK — API Reference

> **SDK Version:** 1.2.2 · **Target:** .NET 9.0 Windows

This document is the complete API reference for the SipLine Plugin SDK. It covers every
interface, method, property, and event exposed to plugins, along with the access level
granted to **Community** and **Commercial** plugins.

---

## Plugin Types

SipLine distinguishes three plugin types via `ISipLinePlugin.LicenseType`:

| Type | Description |
|------|-------------|
| `Community` | Free plugin. No license required. Access to UI and monitoring APIs only. |
| `Commercial` | Paid plugin. Requires a `license.json` file. Full API access. |
| `Integrated` | Reserved for official built-in plugins. Do not use. |

### Legend

| Symbol | Meaning |
|--------|---------|
| ✅ | Fully accessible |
| 🔒 | Blocked — returns `null`, empty list, or `false`; writes are ignored |
| 👁️ | Read-only — `get` allowed, `set` silently blocked |
| ⭐ | Commercial only |

---

## Community vs Commercial — At a Glance

### What Community plugins CAN do

- Add tabs to the sidebar, toolbar buttons, settings tabs
- Open full-page plugin views
- Show dialogs, input prompts, notifications, snackbars
- Store and retrieve plugin settings
- Read SIP registration status and whether a call is active
- Read the current DND (Do Not Disturb) state
- Monitor audio volume and mute state changes
- Listen to language and device change events
- Use the logger and access the plugin data folder
- Use the localization API
- Access the full UI theming system

### What requires a Commercial license ⭐

| Category | Capability |
|----------|------------|
| **Call Events** | Incoming, outgoing, answered, ended, held, resumed, DTMF |
| **Call Actions** | Make calls, hang up, hold, transfer, send DTMF |
| **Call Info** | Caller number, callee number, call ID, direction, duration |
| **Call History** | Query recent calls, calls by date range, calls by number |
| **Contacts** | List, add, update, delete contacts |
| **DND Control** | Set DND state, receive DND change events |
| **Audio Control** | Set volume, mute/unmute microphone |
| **Search** | Register a global search provider |
| **Context Menus** | Inject options into history, contacts, and active call menus |

---

## `ISipLinePlugin` — Plugin Identity

Every plugin must implement this interface.

```csharp
public class MyPlugin : ISipLinePlugin { ... }
```

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `Id` | `string` | Unique plugin identifier (e.g. `"com.company.myplugin"`) | ✅ | ✅ |
| `Name` | `string` | Display name shown in the plugin manager | ✅ | ✅ |
| `Description` | `string` | Short description (1–2 sentences) | ✅ | ✅ |
| `Version` | `Version` | Plugin version | ✅ | ✅ |
| `Author` | `string` | Author name | ✅ | ✅ |
| `WebsiteUrl` | `string?` | Optional website or repository URL | ✅ | ✅ |
| `LicenseType` | `PluginLicenseType` | `Community`, `Commercial`, or `Integrated` | ✅ | ✅ |
| `Icon` | `PluginIcon?` | Standard icon (takes priority over `IconPathData`) | ✅ | ✅ |
| `IconPathData` | `string?` | Custom SVG path for the plugin icon | ✅ | ✅ |
| `HasSettingsUI` | `bool` | Indicates whether the plugin provides a settings view | ✅ | ✅ |
| `GetSettingsUI()` | `object?` | Returns a WPF `UserControl` for settings (or `null`) | ✅ | ✅ |
| `InitializeAsync(IPluginContext)` | `Task` | Called when the plugin loads | ✅ | ✅ |
| `ShutdownAsync()` | `Task` | Called when the application closes | ✅ | ✅ |

---

## `IPluginContext` — Main Entry Point

The context is injected into `InitializeAsync()`. It is your gateway to all SipLine services and UI integration.

```csharp
public Task InitializeAsync(IPluginContext context)
{
    context.Logger.LogInformation("Plugin loaded");
    // ...
}
```

### Services

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `SipService` | `IPluginSipService` | Call control and SIP events | 👁️ Limited | ✅ Full |
| `CallHistory` | `IPluginCallHistory` | Access to call history | 👁️ Limited | ✅ Full |
| `Contacts` | `IPluginContactService` | Read and manage contacts | 🔒 | ✅ Full |
| `Audio` | `IPluginAudioService` | Audio volume and mute control | 👁️ Limited | ✅ Full |
| `Localization` | `IPluginLocalization` | Localized strings for your plugin | ✅ | ✅ |

### Application Info

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `AppVersion` | `string` | Current SipLine version (e.g. `"2.3.1"`) | ✅ | ✅ |
| `IsDarkTheme` | `bool` | `true` if the app is in dark mode | ✅ | ✅ |
| `PluginDataPath` | `string` | Dedicated storage folder for your plugin data | ✅ | ✅ |
| `AreRequiredSettingsFilled` | `bool` | `true` if all required settings have values | ✅ | ✅ |

### Logger

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `Logger` | `ILogger` | Plugin-scoped logger (prefixed with plugin name) | ✅ | ✅ |
| `AddLog(message, level)` | `void` | Appends a message to the application log | ✅ | ✅ |

### Events

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `OnLanguageChanged` | `event Action<string>?` | Fires when the UI language changes | ✅ | ✅ |
| `OnDevicesChanged` | `event Action?` | Fires when audio devices are added or removed | ✅ | ✅ |

### UI Registration

| Member | Description | Community | Commercial |
|--------|-------------|-----------|------------|
| `RegisterSidebarTab(PluginSidebarTab)` | Adds a tab to the main sidebar | ✅ | ✅ |
| `UnregisterSidebarTab(string tabId)` | Removes your sidebar tab | ✅ | ✅ |
| `SelectSidebarTab(string tabId)` | Programmatically focuses a sidebar tab | ✅ | ✅ |
| `RegisterSettingsTab(PluginSettingsTab)` | Adds a tab to the settings window | ✅ | ✅ |
| `UnregisterSettingsTab(string tabId)` | Removes your settings tab | ✅ | ✅ |
| `RegisterToolbarButton(PluginToolbarButton)` | Adds a button to the sidebar toolbar | ✅ | ✅ |
| `UnregisterToolbarButton(string buttonId)` | Removes your toolbar button | ✅ | ✅ |
| `OpenPluginView(PluginViewRequest)` | Opens a full-page view | ✅ | ✅ |
| `ClosePluginView()` | Closes the current full-page view | ✅ | ✅ |
| `RegisterSearchProvider(IPluginSearchProvider)` | Adds a provider to global search | 🔒 | ✅ ⭐ |
| `UnregisterSearchProvider(string name)` | Removes your search provider | 🔒 | ✅ ⭐ |
| `RegisterContextMenuOption(MenuArea, label, Action<object>)` | Injects an option into a context menu | 🔒 | ✅ ⭐ |

### User Feedback

| Member | Description | Community | Commercial |
|--------|-------------|-----------|------------|
| `ShowNotification(title, message, severity)` | Windows Toast notification | ✅ | ✅ |
| `ShowSnackbar(message, severity)` | In-app snackbar | ✅ | ✅ |
| `ShowDialogAsync(title, message, primary, secondary)` | Modal confirmation dialog → returns `bool` | ✅ | ✅ |
| `ShowInputAsync(title, message, defaultValue, primary, secondary)` | Modal text input dialog → returns `string?` | ✅ | ✅ |

### Settings

| Member | Description | Community | Commercial |
|--------|-------------|-----------|------------|
| `GetSetting<T>(key, defaultValue)` | Retrieves a saved plugin setting | ✅ | ✅ |
| `SetSetting<T>(key, value)` | Persists a plugin setting | ✅ | ✅ |
| `RegisterSettingsFields(IEnumerable<PluginSettingsField>)` | Declares settings fields for the settings page | ✅ | ✅ |

### Threading

| Member | Description | Community | Commercial |
|--------|-------------|-----------|------------|
| `RunOnUIThread(Action)` | Executes an action on the WPF UI thread | ✅ | ✅ |
| `RunOnUIThread<T>(Func<T>)` | Executes a function on the UI thread and returns the result | ✅ | ✅ |

---

## `IPluginSipService` — SIP & Call Control

Access via `context.SipService`.

> Commercial plugins receive the full-access adapter.
> Community plugins receive a restricted adapter — call events never fire, call actions are no-ops, and sensitive properties return `null`.

### Events

| Event | Payload | Description | Community | Commercial |
|-------|---------|-------------|-----------|------------|
| `OnCallIncoming` | `CallInfo` | Incoming call received | 🔒 | ✅ ⭐ |
| `OnCallOutgoing` | `CallInfo` | Outgoing call initiated | 🔒 | ✅ ⭐ |
| `OnCallAnswered` | `CallInfo` | Call answered | 🔒 | ✅ ⭐ |
| `OnCallEnded` | `CallEndedInfo` | Call ended | 🔒 | ✅ ⭐ |
| `OnCallHeld` | `CallInfo` | Call placed on hold | 🔒 | ✅ ⭐ |
| `OnCallResumed` | `CallInfo` | Call resumed from hold | 🔒 | ✅ ⭐ |
| `OnDtmfReceived` | `DtmfInfo` | DTMF digit received during call | 🔒 | ✅ ⭐ |
| `OnRegistrationChanged` | `RegistrationStatus` | SIP registration state changed | ✅ | ✅ |
| `OnDndChanged` | `bool` | Do Not Disturb state changed | 🔒 | ✅ ⭐ |

### State Properties

| Property | Type | Description | Community | Commercial |
|----------|------|-------------|-----------|------------|
| `RegistrationStatus` | `RegistrationStatus` | Current SIP registration state | ✅ | ✅ |
| `IsInCall` | `bool` | `true` if a call is currently active | ✅ | ✅ |
| `CurrentCall` | `CallInfo?` | Active call details | 🔒 (null) | ✅ ⭐ |
| `SipUser` | `string?` | Logged-in SIP extension/number | 🔒 (null) | ✅ ⭐ |
| `IsDndEnabled` | `bool` | Current Do Not Disturb state | 👁️ Read-only | ✅ ⭐ |

### Actions

| Method | Returns | Description | Community | Commercial |
|--------|---------|-------------|-----------|------------|
| `MakeCallAsync(destination)` | `Task<bool>` | Initiates an outgoing call | 🔒 | ✅ ⭐ |
| `HangupCallAsync(callId)` | `Task` | Terminates a call | 🔒 | ✅ ⭐ |
| `SetHoldAsync(callId, hold)` | `Task` | Holds or resumes a call | 🔒 | ✅ ⭐ |
| `TransferCallAsync(callId, destination)` | `Task` | Blind transfer to another number | 🔒 | ✅ ⭐ |
| `SendDtmf(digit)` | `void` | Sends a DTMF tone during a call | 🔒 | ✅ ⭐ |

### Model Classes

#### `CallInfo`
Represents an active or recent call.

| Property | Type | Description |
|----------|------|-------------|
| `CallId` | `string` | Unique call identifier |
| `CallerNumber` | `string` | Caller's phone number |
| `CallerName` | `string?` | Caller's display name (if available) |
| `CalleeNumber` | `string` | Called number |
| `Direction` | `CallDirection` | `Incoming` or `Outgoing` |
| `StartTime` | `DateTime` | When the call started |
| `IsOnHold` | `bool` | Whether the call is on hold |
| `IsRecording` | `bool` | Whether the call is being recorded |

#### `CallEndedInfo`
Provided when a call ends via `OnCallEnded`.

| Property | Type | Description |
|----------|------|-------------|
| `Call` | `CallInfo` | Final state of the call |
| `Duration` | `TimeSpan` | Total call duration |
| `Reason` | `CallEndReason` | Why the call ended |
| `SipCode` | `int?` | SIP termination code (e.g. 200, 486, 603) |

#### `DtmfInfo`
Provided via `OnDtmfReceived`.

| Property | Type | Description |
|----------|------|-------------|
| `Digit` | `char` | The pressed digit (0–9, *, #) |
| `DurationMs` | `int` | Duration in milliseconds |
| `CallId` | `string` | Associated call ID |

### Enums

#### `RegistrationStatus`
| Value | Description |
|-------|-------------|
| `Unregistered` | Not registered to any SIP server |
| `Registering` | Registration in progress |
| `Registered` | Successfully registered |
| `Failed` | Registration failed |

#### `CallDirection`
| Value | Description |
|-------|-------------|
| `Incoming` | Call was received |
| `Outgoing` | Call was initiated |

#### `CallEndReason`
| Value | Description |
|-------|-------------|
| `Normal` | Call ended normally |
| `Busy` | Recipient was busy |
| `NoAnswer` | No answer |
| `Rejected` | Call was rejected |
| `NetworkError` | Network or server error |
| `Cancelled` | Caller cancelled before answer |
| `Other` | Other reason |

---

## `IPluginCallHistory` — Call History

Access via `context.CallHistory`.

| Member | Returns | Description | Community | Commercial |
|--------|---------|-------------|-----------|------------|
| `TotalCount` | `int` | Total number of calls in history | ✅ | ✅ |
| `OnCallAdded` | `event Action<CallHistoryEntry>?` | Fires when a new call is recorded | 🔒 | ✅ ⭐ |
| `GetRecentCalls(count = 50)` | `IReadOnlyList<CallHistoryEntry>` | Returns the most recent N calls | 🔒 | ✅ ⭐ |
| `GetCallsBetween(from, to)` | `IReadOnlyList<CallHistoryEntry>` | Returns calls within a date range | 🔒 | ✅ ⭐ |
| `GetCallsForNumber(phoneNumber)` | `IReadOnlyList<CallHistoryEntry>` | Returns all calls for a phone number | 🔒 | ✅ ⭐ |

### `CallHistoryEntry`

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique record identifier |
| `PhoneNumber` | `string` | The external phone number |
| `ContactName` | `string?` | Matched contact name (if any) |
| `Direction` | `CallDirection` | `Incoming` or `Outgoing` |
| `Timestamp` | `DateTime` | When the call occurred |
| `Duration` | `TimeSpan` | Call duration |
| `WasAnswered` | `bool` | Whether the call was answered |
| `WasRecorded` | `bool` | Whether a recording exists |
| `RecordingPath` | `string?` | Path to the recording file |
| `Notes` | `string?` | User notes attached to the call |

---

## `IPluginContactService` — Contacts

Access via `context.Contacts`.

> Completely blocked for Community plugins.

| Method | Returns | Description | Community | Commercial |
|--------|---------|-------------|-----------|------------|
| `GetContactsAsync()` | `Task<IEnumerable<PluginContact>>` | Returns all SipLine contacts | 🔒 | ✅ ⭐ |
| `AddContactAsync(contact)` | `Task` | Adds a new contact | 🔒 | ✅ ⭐ |
| `UpdateContactAsync(contact)` | `Task` | Updates an existing contact | 🔒 | ✅ ⭐ |
| `DeleteContactAsync(contactId)` | `Task` | Deletes a contact by ID | 🔒 | ✅ ⭐ |

### `PluginContact`

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique contact identifier |
| `Name` | `string` | Full name |
| `PhoneNumber` | `string` | Primary phone number |
| `SecondaryNumber` | `string?` | Secondary phone number |
| `Email` | `string?` | Email address |
| `Company` | `string?` | Company name |
| `Group` | `string?` | Contact group (defaults to `"Plugin"`) |

---

## `IPluginAudioService` — Audio

Access via `context.Audio`.

| Member | Type | Description | Community | Commercial |
|--------|------|-------------|-----------|------------|
| `Volume` | `int` (0–100) | Current speaker volume | 👁️ Read-only | ✅ |
| `IsMuted` | `bool` | Whether the microphone is muted | ✅ | ✅ |
| `SetMute(bool mute)` | `void` | Mutes or unmutes the microphone | 🔒 | ✅ ⭐ |
| `OnVolumeChanged` | `event Action<int>?` | Fires when volume changes | ✅ | ✅ |
| `OnMuteChanged` | `event Action<bool>?` | Fires when mute state changes | ✅ | ✅ |

---

## `IPluginLocalization` — Localization

Access via `context.Localization`.

| Member | Returns | Description | Community | Commercial |
|--------|---------|-------------|-----------|------------|
| `CurrentCulture` | `CultureInfo` | The current UI culture | ✅ | ✅ |
| `GetString(key)` | `string` | Returns the localized string for a key | ✅ | ✅ |
| `GetString(key, params object[] args)` | `string` | Returns a formatted localized string | ✅ | ✅ |

Place your `.resx` files in a `Resources/` folder in your plugin project. SipLine auto-detects them.

---

## `IPluginSearchProvider` — Search Integration ⭐

> Registration requires a Commercial license. The interface itself can be implemented by anyone, but `RegisterSearchProvider()` is blocked for Community plugins.

```csharp
public class MySearchProvider : IPluginSearchProvider
{
    public string ProviderName => "MyCRM";

    public async Task<IEnumerable<PluginSearchResult>> SearchAsync(string query)
    {
        // Query your service...
        return results.Select(r => new PluginSearchResult
        {
            Id = r.Id,
            Title = r.Name,
            Subtitle = r.Company,
            PhoneNumber = r.Phone
        });
    }
}

// In InitializeAsync:
context.RegisterSearchProvider(new MySearchProvider());
```

### `PluginSearchResult`

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique result identifier |
| `Title` | `string` | Primary display text (e.g. contact name) |
| `Subtitle` | `string?` | Secondary text (e.g. company) |
| `PhoneNumber` | `string?` | Phone number (enables click-to-call) |
| `Icon` | `string?` | SVG icon path |
| `ResultType` | `string?` | Category label (default: `"Contact"`) |

---

## UI Models

### `PluginSidebarTab`

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Id` | `string` | — | Unique identifier |
| `PluginId` | `string` | — | Owner plugin ID |
| `Title` | `string` | — | Label shown in the sidebar |
| `Tooltip` | `string` | — | Tooltip shown in collapsed sidebar |
| `Icon` | `PluginIcon?` | `null` | Standard icon (takes priority) |
| `IconPathData` | `string` | — | Custom SVG icon path |
| `Order` | `int` | `200` | Position (smaller = higher) |
| `IsVisible` | `bool` | `true` | Whether the tab is visible |
| `Badge` | `int?` | `null` | Badge count (null = no badge) |
| `ContentFactory` | `Func<object>?` | `null` | Factory returning a WPF `UserControl` |
| `Command` | `ICommand?` | `null` | Command executed on tab click |

### `PluginSettingsTab`

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Id` | `string` | — | Unique identifier |
| `Title` | `string` | — | Tab title in the settings window |
| `Icon` | `PluginIcon?` | `null` | Standard icon |
| `IconPathData` | `string` | — | Custom SVG icon path |
| `Content` | `object` | — | WPF `UserControl` instance |
| `Order` | `int` | `200` | Position in the settings navigation |

### `PluginToolbarButton`

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `Id` | `string` | — | Unique identifier |
| `Tooltip` | `string` | — | Tooltip on hover |
| `Icon` | `PluginIcon?` | `null` | Standard icon |
| `IconPathData` | `string` | — | Custom SVG icon path |
| `Command` | `ICommand?` | `null` | Command executed on click |
| `CommandParameter` | `object?` | `null` | Parameter passed to the command |
| `Order` | `int` | `200` | Position (smaller = leftmost) |
| `IsVisible` | `bool` | `true` | Visibility toggle |
| `IsEnabled` | `bool` | `true` | Enabled state |

### `PluginViewRequest`

| Property | Type | Default | Description |
|----------|------|---------|-------------|
| `ViewId` | `string` | — | Unique view identifier |
| `Title` | `string` | — | Title in the page header |
| `Subtitle` | `string?` | `null` | Optional subtitle |
| `IconPathData` | `string?` | `null` | SVG icon path |
| `Content` | `object` | — | WPF `UserControl` |
| `ShowBackButton` | `bool` | `true` | Show back button to close the view |
| `OnClosed` | `Action?` | `null` | Callback when the view closes |

### `PluginSettingsField`

Declare your plugin's settings fields programmatically so SipLine renders them automatically.

| Property | Type | Description |
|----------|------|-------------|
| `Key` | `string` | Storage key |
| `Label` | `string` | Display label |
| `Type` | `SettingsFieldType` | Field type (see below) |
| `IsRequired` | `bool` | If `true`, the plugin is hidden until this field is filled |
| `Placeholder` | `string?` | Placeholder text |
| `Description` | `string?` | Help text shown below the field |
| `DefaultValue` | `string?` | Default value |
| `Options` | `List<PluginSettingsOption>?` | Options for `Select` type |
| `OnValueChanged` | `Action<string, string?, string?>?` | Callback on value change |

#### `SettingsFieldType` Enum

| Value | Renders As |
|-------|-----------|
| `Text` | Text input |
| `Password` | Masked password input |
| `Checkbox` | Toggle / checkbox |
| `Select` | Dropdown list |
| `Number` | Numeric input |
| `Info` | Informational text (no input) |
| `Link` | Clickable hyperlink |

### `MenuArea` Enum ⭐

| Value | Context |
|-------|---------|
| `History` | Right-click on a call history entry |
| `Contacts` | Right-click on a contact |
| `ActiveCall` | Right-click during an active call |

---

## Standard Icons — `PluginIcon`

Use standard icons for consistent rendering with SipLine's theme. The host automatically applies the correct color and size.

```csharp
public PluginIcon? Icon => PluginIcon.Phone;
```

Available values: `Default`, `Phone`, `Message`, `Contact`, `Settings`, `Database`, `Headset`, `Microphone`, `History`, `Cloud`, `Security`, `Chart`, `Star`, `Home`, `Calendar`, `Camera`, `Wifi`, `Lock`, `User`, `Trash`, `Edit`, `Info`, `Help`, `Play`, `Pause`, `Stop`, `Warning`, `Error`, `Check`, `CRM`

For custom icons, use an SVG path string in `IconPathData`:
```csharp
public string? IconPathData => "M12 2a10 10 0 1 0 0 20A10 10 0 0 0 12 2z";
```
Browse icons at [lucide.dev/icons](https://lucide.dev/icons).

---

## Testing

Use the `SipLine.Plugin.Testing` NuGet package to write unit tests without running the host application.

```xml
<PackageReference Include="SipLine.Plugin.Testing" Version="1.2.2" />
```

```csharp
using SipLine.Plugin.Testing;

[Fact]
public async Task Should_Show_Notification_On_Incoming_Call()
{
    var ctx = new MockPluginContext();
    var plugin = new MyPlugin();
    await plugin.InitializeAsync(ctx);

    ((MockSipService)ctx.SipService).TriggerIncomingCall("0612345678", "1000");

    Assert.Contains(ctx.Notifications, n => n.Contains("Incoming"));
}
```

### `MockPluginContext` available triggers

| Method | Description |
|--------|-------------|
| `((MockSipService)ctx.SipService).TriggerIncomingCall(from, to)` | Simulates an incoming call |
| `((MockSipService)ctx.SipService).TriggerCallEnded()` | Ends the current call |
| `((MockSipService)ctx.SipService).TriggerCallAnswered(call)` | Marks call as answered |
| `((MockSipService)ctx.SipService).TriggerCallHeld(call)` | Places call on hold |
| `((MockSipService)ctx.SipService).TriggerCallResumed(call)` | Resumes held call |
| `((MockSipService)ctx.SipService).TriggerRegistrationChanged(status)` | Changes registration state |
| `((MockCallHistory)ctx.CallHistory).TriggerCallAdded(entry)` | Adds an entry to history |
| `((MockAudioService)ctx.Audio).TriggerVolumeChanged(vol)` | Simulates volume change |
| `ctx.TriggerLanguageChanged("fr")` | Simulates language switch |
| `ctx.TriggerDevicesChanged()` | Simulates audio device change |

---

## Useful Links

- [SipLine Website](https://sipline.feelautom.fr)
- [Plugin SDK on NuGet](https://www.nuget.org/packages/SipLine.Plugin.Sdk)
- [Plugin SDK on GitHub](https://github.com/feelautom/SipLine.Plugin.Sdk)
- [Lucide Icons](https://lucide.dev/icons)
