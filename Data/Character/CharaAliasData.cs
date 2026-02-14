using MessagePack;
using System.Diagnostics.Contracts;

namespace GagspeakAPI.Data;

// This can double as a use for a GlobalAliasStorage.
[MessagePackObject(keyAsPropertyName: true)]
public class AliasStorage : IEditableStorage<AliasTrigger>
{
    public List<AliasTrigger> Items { get; set; } = new();

    public AliasStorage()
    { }

    public AliasStorage(List<AliasTrigger> init)
    {
        Items = init;
    }

    public bool TryApplyChanges(AliasTrigger oldItem, AliasTrigger changedItem)
    {
        if (changedItem is null)
            return false;
        
        oldItem.ApplyChanges(changedItem);
        return true;
    }
}

[MessagePackObject(keyAsPropertyName: true)]
public class AliasTrigger : IEditableStorageItem<AliasTrigger>
{
    /// <summary> Unique identifier for the trigger. </summary>
    public Guid Identifier { get; set; } = Guid.NewGuid();

    /// <summary> Whether the trigger is enabled or not. </summary>
    public bool Enabled { get; set; } = false;

    /// <summary> The label for the trigger. </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    ///     If the trigger should ignore case sensativity or respect it.
    /// </summary>
    public bool IgnoreCase { get; set; } = false;

    /// <summary>
    ///     The input command that triggers the output command
    /// </summary>
    public string InputCommand { get; set; } = string.Empty;

    /// <summary> Stores Actions with unique types. </summary>
    public HashSet<InvokableGsAction> Actions { get; set; } = new HashSet<InvokableGsAction>() { new TextAction() };
   
    /// <summary>
    ///     The Kinksters allowed to view and use this Alias.
    /// </summary>
    public HashSet<string> WhitelistedUIDs { get; set; } = new HashSet<string>();
    
    public AliasTrigger() 
    { }

    public AliasTrigger(AliasTrigger other, bool keepId)
    {
        Identifier = keepId ? other.Identifier : Guid.NewGuid();
        ApplyChanges(other);
    }

    public AliasTrigger Clone(bool keepId = false)
        => new AliasTrigger(this, keepId);

    public void ApplyChanges(AliasTrigger changedItem)
    {
        Enabled = changedItem.Enabled;
        Label = changedItem.Label;
        IgnoreCase = changedItem.IgnoreCase;
        InputCommand = changedItem.InputCommand;
        Actions = changedItem.Actions.ToHashSet();
        WhitelistedUIDs = changedItem.WhitelistedUIDs.ToHashSet();
    }

    public bool CanView(string uid)
        => WhitelistedUIDs.Count is 0 || WhitelistedUIDs.Contains(uid);

    public bool ValidAlias()
        => !string.IsNullOrWhiteSpace(Label)
        && !string.IsNullOrWhiteSpace(InputCommand)
        && Actions.Count > 0
        && Actions.All(a => a.IsValid());
}
