using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace Validate.Handlers;
public class ValidateChunk : IChunkHandler
{
    public string Name => "Validate Chunk";

    public Type? ChunkType => null;

    public Image? Image => ValidatePlugin.ValidateImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(Chunk chunk)
    {
        try
        {
            chunk.Validate();
            MessageBox.Show("No errors found in chunk.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Found error in chunk: {ex.Message}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        return ChunkCallbackResult.Unchanged;
    }
}
