using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using TimeOfDayTint.Extensions;
using TimeOfDayTint.Forms;

namespace TimeOfDayTint.Handlers;
public class TimeOfDayTint : IFileHandler
{
    public string Name => "Time Of Day Tint";

    public Image? Image => TimeOfDayTintPlugin.ColorScaleImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        using var tintOptions = new FrmTintOptions();
        if (tintOptions.ShowDialog() != DialogResult.OK)
            return FileCallbackResult.Unchanged;

        var currentTint = tintOptions.CurrentTint;
        var newTint = tintOptions.NewTint;
        var tint = currentTint.Lerp(newTint, tintOptions.Blend);
        var brightness = tintOptions.Brightness;        

        foreach (var meshChunk in p3dFile.AllChunks.OfType<MeshChunk>())
            ProcessMesh(meshChunk, tint, brightness);

        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => p3dFile.AllChunks.Any(x => x is MeshChunk);

    private static void ProcessMesh(MeshChunk meshChunk, Color tint, float brightness)
    {
        foreach (var oldPrimitiveGroupChunk in meshChunk.GetChunksOfType<OldPrimitiveGroupChunk>())
        {
            var colourListChunk = oldPrimitiveGroupChunk.GetFirstChunkOfType<ColourListChunk>();
            if (colourListChunk == null)
                continue;

            for (int i = 0; i < colourListChunk.NumColours; i++)
                colourListChunk.Colours[i] = colourListChunk.Colours[i].Multiply(tint).ApplyBrightness(brightness);
        }
        foreach (var primitiveGroupChunk in meshChunk.GetChunksOfType<PrimitiveGroupChunk>())
        {
            var colourListChunk = primitiveGroupChunk.GetFirstChunkOfType<ColourListChunk>();
            if (colourListChunk == null)
                continue;

            for (int i = 0; i < colourListChunk.NumColours; i++)
                colourListChunk.Colours[i] = colourListChunk.Colours[i].Multiply(tint).ApplyBrightness(brightness);
        }
    }
}

