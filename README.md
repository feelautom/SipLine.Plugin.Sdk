# SipLine Plugin SDK

![Platform](https://img.shields.io/badge/platform-Windows-blue)
![Framework](https://img.shields.io/badge/.NET-9.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

The official SDK for developing plugins for **SipLine**, the professional SIP softphone. This SDK allows you to extend the application's functionality, add UI elements, and interact with SIP calls.

---

### 🌐 Official Website
For more information, downloads, and documentation about the softphone itself, visit:
**[https://sipline.feelautom.fr](https://sipline.feelautom.fr)**

---

## 🚀 Features

*   **UI Integration**: Add tabs to the sidebar, buttons to the toolbar, or create full-page views using WPF.
*   **Interactive Dialogs**: Display confirmation dialogs and prompt for user input.
*   **Application Context**: Access current application version, theme mode (dark/light), and UI thread execution helpers.
*   **UI Navigation**: Programmatically select sidebar tabs.
*   **SIP Events**: Intercept incoming calls, detect outgoing calls, and monitor call state changes.
*   **Settings Management**: Easily store and retrieve persistent configuration for your plugin.
*   **Notifications**: Trigger native Windows toasts or in-app snackbars.
*   **Audio**: Access audio device events and stream or inject real-time call audio when licensed.

## 📦 Installation

To start building a plugin, create a new **.NET 9.0 Windows Class Library** and reference the SDK:

```xml
<ItemGroup>
    <PackageReference Include="SipLine.Plugin.Sdk" Version="1.3.0" Private="false" ExcludeAssets="runtime" />
</ItemGroup>
```

> **Note:** Ensure you set `<Private>false</Private>` and `<ExcludeAssets>runtime</ExcludeAssets>` for the SDK reference to avoid DLL conflicts at runtime.

## ⚡ Quick Start

Implement the `ISipLinePlugin` interface:

```csharp
using SipLine.Plugin.Sdk;
using SipLine.Plugin.Sdk.Enums;
using Microsoft.Extensions.Logging;

public class MyAwesomePlugin : ISipLinePlugin
{
    public string Id => "com.mycompany.plugin";
    public string Name => "My Awesome Plugin";
    public string Description => "Adds super powers to SipLine.";
    public Version Version => new(1, 0, 0);
    public string Author => "Me";

    // Use standard icon
    public PluginIcon? Icon => PluginIcon.Rocket;

    private IPluginContext _context;

    public Task InitializeAsync(IPluginContext context)
    {
        _context = context;
        _context.Logger.LogInformation("Hello from MyAwesomePlugin!");

        // Add a sidebar tab
        _context.RegisterSidebarTab(new PluginSidebarTab
        {
            Id = "my-tab", // Unique ID for your tab
            Title = "My Plugin",
            Tooltip = "Open My Plugin View",
            Icon = PluginIcon.Star, // Use standard icon or IconPathData for custom SVG
            Order = 200, // Position in the sidebar
            ContentFactory = () => new MyPluginView() // Your WPF UserControl for the tab content
        });

        return Task.CompletedTask;
    }

    public Task ShutdownAsync()
    {
        // Unregister your tab when the plugin shuts down
        _context?.UnregisterSidebarTab("my-tab");
        return Task.CompletedTask;
    }
}
```

## 🏗️ Architecture

Your plugin interacts with SipLine through the `IPluginContext`.

| Feature | `IPluginContext` Member | Description |
|---------|-------------------------|-------------|
| **SIP Core** | `SipService` | Control calls (Make, Answer, Hangup, Transfer, DTMF) |
| | `CallHistory` | Access call history records |
| | `Contacts` | Access SipLine contact list |
| **UI Interaction** | `ShowNotification`, `ShowSnackbar` | Display messages to the user |
| | `ShowDialogAsync`, `ShowInputAsync` | Show modal dialogs for confirmation or input |
| | `SelectSidebarTab` | Programmatically select a sidebar tab |
| **App Context** | `IsDarkTheme`, `AppVersion` | Get application theme and version |
| | `RunOnUIThread` | Execute code safely on the UI thread |
| **Plugin Specific** | `Logger`, `PluginDataPath` | Logging and persistent storage for plugin data |
| | `GetSetting`, `SetSetting` | Store and retrieve plugin settings |
| | `RegisterSidebarTab`, `RegisterSettingsTab` | Extend SipLine UI with custom views |
| | `RegisterContextMenuOption` | Add options to context menus (Contacts, History, Active Call) |

## 🎨 Theming & Styles

To ensure your plugin looks native to SipLine, use the standardized theme resources provided by `PluginTheme`.

```xml
<UserControl ...
             xmlns:theme="clr-namespace:SipLine.Plugin.Sdk.Theme;assembly=SipLine.Plugin.Sdk">
    <Border Background="{DynamicResource {x:Static theme:PluginTheme.SurfaceBrushKey}}">
        <TextBlock Text="Hello" Foreground="{DynamicResource {x:Static theme:PluginTheme.TextBrushKey}}"/>
    </Border>
</UserControl>
```

## 🌍 Localization (I18N)

SipLine automatically detects `Resources.resx` files (e.g., `MyPlugin.Resources.Strings.resx`) in your plugin assembly.
Use `context.Localization` to access localized strings:

```csharp
var greeting = context.Localization.GetString("HelloMessage");
var formattedGreeting = context.Localization.GetString("WelcomeMessage", "User123");
```

For dynamic UI updates in XAML, implement an indexer in your ViewModel:
```csharp
public string this[string key] => _context.Localization.GetString(key);

public MyViewModel(IPluginContext context)
{
    _context = context;
    // Notify WPF to refresh all keys when the language changes
    _context.OnLanguageChanged += (lang) => OnPropertyChanged("Item[]");
}
```

## 🖼️ Icons

You can use standard icons from the `PluginIcon` enum instead of providing raw SVG paths. The host application will render them with the correct theme and style.

```csharp
public PluginIcon? Icon => PluginIcon.Message;
```

**Available Icons:**
`Default`, `Phone`, `Message`, `Contact`, `Settings`, `Database`, `Headset`, `Microphone`, `History`, `Cloud`, `Security`, `Chart`, `Star`, `Home`, `Calendar`, `Camera`, `Wifi`, `Lock`, `User`, `Trash`, `Edit`, `Info`, `Help`, `Play`, `Pause`, `Stop`, `Warning`, `Error`, `Check`, `CRM`.

## 🧪 Testing

Use `SipLine.Plugin.Testing` to unit test your plugin logic without running the full application.

```csharp
using SipLine.Plugin.Testing;

[Fact]
public async Task Should_Log_When_Call_Incoming()
{
    var mockContext = new MockPluginContext();
    var plugin = new MyPlugin();
    await plugin.InitializeAsync(mockContext);

    // Simulate incoming call
    ((MockSipService)mockContext.SipService).TriggerIncomingCall("123", "456");

    Assert.Contains(mockContext.Logs, l => l.Contains("Incoming call from 123"));
}
```

## 💾 Data Lifecycle

*   **Storage:** Use `context.PluginDataPath` to store files.
*   **Location:** `%AppData%\FeelAutomCorp\SipLine\plugins\{PluginId}\data\`.
*   **Persistence:** Data is preserved during plugin updates.
*   **Uninstall:** Data is NOT automatically deleted when the plugin is removed (to prevent accidental loss).

## 🔑 Licensing

SipLine supports three types of plugins, defined by the `LicenseType` property:

1.  **Community (Default):** Free, open-source, or internal plugins. No license verification is performed.
    ```csharp
    public PluginLicenseType LicenseType => PluginLicenseType.Community;
    ```

2.  **Commercial:** Paid plugins protected by SoftLicence. SipLine validates `license.lic` (or `license.json`) in the plugin folder.
    ```csharp
    public PluginLicenseType LicenseType => PluginLicenseType.Commercial;
    ```

3.  **Integrated:** Reserved for official built-in plugins. Do not use this type for your own plugins.

For commercial plugins, SipLine also supports:
- `pluginId` check against `ISipLinePlugin.Id`
- `minAppVersion` / `pluginVersion` checks
- `allowedFeatures` feature gating (`*`, bool, CSV string, array, or object flags)
- An absent `allowedFeatures` keeps the historical `*` policy; an explicitly empty/false allow-list grants no feature and is never promoted to `*`.
- Native enforcement in both `Debug` and `Release` (`SipLine.Native.dll`)

Security contract:
- `license.lic` and `license.json` may contain the official Base64 license file or the directly signed SoftLicence JSON.
- SipLine never treats the presence of a `Signature` property as proof of authenticity. RSA verification must succeed before `pluginId`, hardware binding, versions, or features are trusted.
- Invalid JSON, unknown formats, invalid signatures, missing native validation, and unverifiable signed constraints fail closed.
- The historical two-parameter `LicenseValidator.Validate` API remains available, but rejects a license carrying version constraints because that API has no current versions to compare. Use the four-parameter overload for constrained licenses.
- Do not rewrite, normalize, or reorder signed JSON before validation.

Official feature keys:
- `*`
- `ui.sidebar_tab`
- `ui.settings_tab`
- `ui.toolbar_button`
- `ui.open_view`
- `ui.context_menu`
- `search.provider`

## 🤝 Contributing

Pull requests are welcome! Please ensure your code adheres to the existing coding standards.

## 📄 License

This project is licensed under the MIT License.
