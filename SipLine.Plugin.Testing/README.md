# SipLine Plugin Testing

Official mocks and test helpers for plugins built with `SipLine.Plugin.Sdk`.

## Installation

```xml
<PackageReference Include="SipLine.Plugin.Testing" Version="1.3.0" />
```

The package depends on the matching `SipLine.Plugin.Sdk` version.

## Example

```csharp
using SipLine.Plugin.Sdk;
using SipLine.Plugin.Testing;

var context = new MockPluginContext();
var plugin = new MyPlugin();

await plugin.InitializeAsync(context);

var sip = (MockSipService)context.SipService;
sip.TriggerIncomingCall("1000", "2000");
```

`MockPluginContext.Reset()` clears captured UI interactions, logs, settings, and SIP event subscriptions so each test can start from an isolated state.
