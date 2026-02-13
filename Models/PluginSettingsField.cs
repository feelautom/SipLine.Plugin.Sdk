using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SipLine.Plugin.Sdk;

/// <summary>
/// Définit un champ de paramètre pour un plugin.
/// </summary>
public sealed class PluginSettingsField : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// Clé unique du champ (utilisée pour stocker/récupérer la valeur).
    /// </summary>
    public required string Key { get; set; }

    /// <summary>
    /// Libellé affiché à l'utilisateur.
    /// </summary>
    public required string Label { get; set; }

    /// <summary>
    /// Type de champ.
    /// </summary>
    public SettingsFieldType Type { get; set; } = SettingsFieldType.Text;

    /// <summary>
    /// Si true, le plugin ne s'affiche pas dans le menu tant que ce champ n'est pas rempli.
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// Texte d'aide affiché dans le champ vide.
    /// </summary>
    public string? Placeholder { get; set; }

    /// <summary>
    /// Description ou aide supplémentaire.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Valeur par défaut.
    /// </summary>
    public string? DefaultValue { get; set; }

    /// <summary>
    /// Options pour les champs de type Select.
    /// </summary>
    public List<PluginSettingsOption>? Options { get; set; }

    private string? _value;
    /// <summary>
    /// Valeur actuelle du champ.
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
    /// ID du plugin propriétaire (pour la sauvegarde).
    /// </summary>
    public string? PluginId { get; set; }

    /// <summary>
    /// Action appelée quand la valeur change.
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
/// Option pour un champ Select.
/// </summary>
public sealed class PluginSettingsOption
{
    public required string Value { get; set; }
    public required string Label { get; set; }
}

/// <summary>
/// Types de champs de paramètres supportés.
/// </summary>
public enum SettingsFieldType
{
    /// <summary>Champ texte simple.</summary>
    Text,

    /// <summary>Champ mot de passe (masqué).</summary>
    Password,

    /// <summary>Case à cocher.</summary>
    Checkbox,

    /// <summary>Liste déroulante.</summary>
    Select,

    /// <summary>Champ numérique.</summary>
    Number,

    /// <summary>Note informative ou avertissement (pas de saisie).</summary>
    Info,

    /// <summary>Lien cliquable.</summary>
    Link
}
