using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocationFromGame.Handlers;
internal class LocatorFromGameExcludeTriggerVolumes : IChunkHandler<LocatorChunk>
{
    public string Name => "Set Location From Game (Exclude Trigger Volumes)";

    public Image? Image => LocationFromGamePlugin.FromGameImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(LocatorChunk chunk)
    {
        var pos = MemoryUtils.GetPosition();

        if (!pos.HasValue)
        {
            MessageBox.Show("Failed to retrieve position from game.\nEnsure the game is open and you're in gameplay.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        var (position, rotation) = pos.Value;

        chunk.Position = position;
        if (chunk.TypeData is LocatorChunk.CarStartLocatorData carStartLocatorData)
            carStartLocatorData.Rotation = (float)(rotation * (180.0 / Math.PI));
        return ChunkCallbackResult.ModifiedData;
    }

}
