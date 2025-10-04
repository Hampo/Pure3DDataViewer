using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocationFromGame.Handlers;
internal class TriggerVolumeFromLocator : IChunkHandler<TriggerVolumeChunk>
{
    public string Name => "Set Location From Locator";

    public Image? Image => LocationFromGamePlugin.FromGameImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(TriggerVolumeChunk chunk)
    {
        if (chunk.ParentChunk is not LocatorChunk locatorChunk)
        {
            MessageBox.Show("Parent chunk is not a Locator Chunk.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        var pos = locatorChunk.Position;

        var matrix = chunk.Matrix;
        matrix.M41 = pos.X;
        matrix.M42 = pos.Y;
        matrix.M43 = pos.Z;
        chunk.Matrix = matrix;

        return ChunkCallbackResult.ModifiedData;
    }

}
