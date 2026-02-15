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
*   **SIP Events**: Intercept incoming calls, detect outgoing calls, and monitor call state changes.
*   **Settings Management**: Easily store and retrieve persistent configuration for your plugin.
*   **Notifications**: Trigger native Windows toasts or in-app snackbars.
*   **Audio**: Access audio device events (mute, volume).

## 📦 Installation

To start building a plugin, create a new **.NET 9.0 Windows Class Library** and reference the SDK:

```xml
<ItemGroup>
    <PackageReference Include="SipLine.Plugin.Sdk" Version="1.0.0" />
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

        // Add a button to the main toolbar
        _context.RegisterToolbarButton(new PluginToolbarButton
        {
            Id = "my-btn",
            Icon = PluginIcon.Star, // Or use IconPathData for custom SVG
            Command = new RelayCommand(() => _context.ShowSnackbar("Button clicked!")),
            Tooltip = "Click Me"
        });

        return Task.CompletedTask;
    }

    public Task ShutdownAsync()
    {
        return Task.CompletedTask;
    }
}
```

## 🏗️ Architecture

Your plugin interacts with SipLine through the `IPluginContext`.

| Service | Usage |
|---------|-------|
| `IPluginContext.SipService` | Call control (Answer, Hangup, Transfer) |
| `IPluginContext.CallHistory` | Access past logs |
| `IPluginContext.RegisterSidebarTab` | Add custom XAML views to the main menu |
| `IPluginContext.PluginDataPath` | Path to store your local data/files |

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

SipLine automatically detects `Resources.resx` files (Properties.Resources) in your plugin assembly.
Use the context to access localized strings:

```csharp
var greeting = context.Localization.GetString("HelloMessage");
```

## 🖼️ Icons

You can use standard icons from the `PluginIcon` enum instead of providing raw SVG paths:

```csharp
public PluginIcon? Icon => PluginIcon.Message;
```

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
*   **Location:** `%AppData%\SipLine\plugins\{PluginId}\data\`.
*   **Persistence:** Data is preserved during plugin updates.
*   **Uninstall:** Data is NOT automatically deleted when the plugin is removed (to prevent accidental loss).

## 🤝 Contributing

Pull requests are welcome! Please ensure your code adheres to the existing coding standards.

## 📄 License

This project is licensed under the MIT License.
