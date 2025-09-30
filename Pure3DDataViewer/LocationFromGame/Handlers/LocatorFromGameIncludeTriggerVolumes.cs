using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocationFromGame.Handlers;
internal class LocatorFromGameIncludeTriggerVolumes : IChunkHandler<LocatorChunk>
{
    public string Name => "Set Location From Game (Include Trigger Volumes)";

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

        chunk.Position = pos.Value;
        foreach (var triggerVolume in chunk.GetChunksOfType<TriggerVolumeChunk>())
        {
            var matrix = triggerVolume.Matrix;
            matrix.M41 = pos.Value.X;
            matrix.M42 = pos.Value.Y;
            matrix.M43 = pos.Value.Z;
            triggerVolume.Matrix = matrix;
        }    
        return ChunkCallbackResult.ModifiedChildren;
    }

}
