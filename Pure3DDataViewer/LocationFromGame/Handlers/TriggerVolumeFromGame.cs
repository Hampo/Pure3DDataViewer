using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using Pure3DDataViewerPluginAPI.Utils;

namespace LocationFromGame.Handlers;
internal class TriggerVolumeFromGame : IChunkHandler<TriggerVolumeChunk>
{
    public string Name => "Set Location From Game";

    public Image? Image => LocationFromGamePlugin.FromGameImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(TriggerVolumeChunk chunk)
    {
        var pos = MemoryUtils.GetPosition();

        if (!pos.HasValue)
        {
            MessageBox.Show("Failed to retrieve position from game.\nEnsure the game is open and you're in gameplay.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        var (position, _) = pos.Value;

        var matrix = chunk.Matrix;
        matrix.M41 = position.X;
        matrix.M42 = position.Y;
        matrix.M43 = position.Z;
        chunk.Matrix = matrix;

        return ChunkCallbackResult.ModifiedData;
    }

}
