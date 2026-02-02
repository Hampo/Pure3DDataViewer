using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using SHARMemory.Memory;
using System.Text;

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
        var errors = new StringBuilder();

        foreach (var error in chunk.ValidateChunks())
            errors.AppendLine($"Error in \"{error.Chunk}\": {error.Message}");

        if (errors.Length == 0)
        {
            MessageBox.Show("No errors found in file.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return ChunkCallbackResult.Unchanged;
        }

        MessageBox.Show($"The following errors were found in the file:\n\n{errors}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return ChunkCallbackResult.Unchanged;
    }
}
