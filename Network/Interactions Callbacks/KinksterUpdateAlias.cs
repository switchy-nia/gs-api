using GagspeakAPI.Data;
using MessagePack;

namespace GagspeakAPI.Network;

/// <summary>
///     An update for <paramref name="User"/>, one of the client's Paired Kinksters Global Alias Data.
///     This is received whenever a Global Alias item is modified, created, or deleted.
/// </summary>
/// <remarks> If <paramref name="NewData"/> is null, implies the <paramref name="AliasId"/> was removed. </remarks>
[MessagePackObject(keyAsPropertyName: true)]
public record KinksterNewAliasData(UserData User, Guid AliasId, AliasTrigger? NewData) : KinksterBase(User)
{
    public override string ToString()
        => $"KinksterNewAliasData: [Target -> {User.AliasOrUID}, AliasId -> {AliasId}, Updated Trigger [{(NewData is null ? "UNK" : NewData.Label)}";
}

// Leave in enactor if we want to allow pairs to control state changing?
// (Dont see how this would be possible though since they wouldnt be able to see unshared ones, but for shared ones they could?)
[MessagePackObject(keyAsPropertyName: true)]
public record KinksterUpdateAliasState(UserData User, UserData Enactor, Guid Alias, bool NewState);

[MessagePackObject(keyAsPropertyName: true)]
public record KinksterUpdateActiveAliases(UserData User, UserData Enactor, List<Guid> ActiveAliases);
