using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using Pure3DDataViewerPluginAPI.Utils;

namespace LocationFromGame.Handlers;

internal class TeleportToInGame : IChunkHandler<LocatorChunk>
{
    public string Name => "Teleport To In Game";

    public Image? Image => LocationFromGamePlugin.FromGameImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(LocatorChunk chunk)
    {
        var rot = 0f;
        if (chunk.TypeData is LocatorChunk.CarStartLocatorData carStartLocatorData)
            rot = carStartLocatorData.Rotation;

        try
        {
            MemoryUtils.Teleport(chunk.Position, rot);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not teleport: {ex.Message}", "Error teleporting", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }

}
