using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SipLine.Plugin.Sdk;

/// <summary>
/// Defines a settings field for a plugin.
/// </summary>
public sealed class PluginSettingsField : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// Gets or sets the unique key used to store and retrieve the value.
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Gets or sets the label shown to the user.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// Gets or sets the field type.
    /// </summary>
    public SettingsFieldType Type { get; set; } = SettingsFieldType.Text;

    /// <summary>
    /// Gets or sets whether the plugin stays hidden until this field is filled in.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Gets or sets the placeholder shown when the field is empty.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Gets or sets additional help text.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the default value.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Gets or sets the options for a Select field.
    /// </summary>
    public List<PluginSettingsOption>? Options { get; set; }

    private string? _value;
    /// <summary>
    /// Gets or sets the current field value.
    /// </summary>
    public string? Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                OnPropertyChanged();
            }
        }
    }

    /// <summary>
    /// Gets or sets the owning plugin identifier used for persistence.
    /// </summary>
    public string? PluginId { get; set; }

    /// <summary>
    /// Gets or sets the action invoked when the value changes.
    /// </summary>
    public Action<string, string?, string?>? OnValueChanged { get; set; }

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        if (propertyName == nameof(Value) && OnValueChanged != null && PluginId != null)
        {
            OnValueChanged(Key, _value, PluginId);
        }
    }
}

/// <summary>
/// Option for a Select field.
/// </summary>
public sealed class PluginSettingsOption
{
    public required string Value { get; set; }
    public required string Label { get; set; }
}

/// <summary>
/// Supported settings field types.
/// </summary>
public enum SettingsFieldType
{
    /// <summary>Plain text field.</summary>
    Text,

    /// <summary>Masked password field.</summary>
    Password,

    /// <summary>Checkbox field.</summary>
    Checkbox,

    /// <summary>Drop-down list.</summary>
    Select,

    /// <summary>Numeric field.</summary>
    Number,

    /// <summary>Informational note or warning with no input.</summary>
    Info,

    /// <summary>Clickable link.</summary>
    Link
}
