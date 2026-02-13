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
using Microsoft.Extensions.Logging;

public class MyAwesomePlugin : ISipLinePlugin
{
    public string Id => "com.mycompany.plugin";
    public string Name => "My Awesome Plugin";
    public string Description => "Adds super powers to SipLine.";
    public Version Version => new(1, 0, 0);
    
    // Lucide Icon Path data
    public string IconPathData => "M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5";

    private IPluginContext _context;

    public Task InitializeAsync(IPluginContext context)
    {
        _context = context;
        _context.Logger.LogInformation("Hello from MyAwesomePlugin!");

        // Add a button to the main toolbar
        _context.RegisterToolbarButton(new PluginToolbarButton
        {
            Id = "my-btn",
            IconPathData = IconPathData,
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

## 🤝 Contributing

Pull requests are welcome! Please ensure your code adheres to the existing coding standards.

## 📄 License

This project is licensed under the MIT License.

## Author

Built by [FeelAutom](https://feelautom.fr) — contact@feelautom.fr
