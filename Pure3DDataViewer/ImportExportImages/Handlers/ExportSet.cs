using ImportExportImages.Enums;
using ImportExportImages.Helpers;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ExportSet : IChunkHandler<SetChunk>
{
    public string Name => "Export Set";

    public Image? Image => ImportExportImagesPlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(SetChunk set)
    {
        var textures = set.GetChunksOfType<TextureChunk>();
        if (textures.Count == 0)
        {
            MessageBox.Show("No textures to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return ChunkCallbackResult.Unchanged;
        }

        using var fbd = new FolderBrowserDialog()
        {
            ClientGuid = ImportExportImagesPlugin.ExportAllGuid,
            Description = "Choose folder to export set textures to",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true,
        };
        if (fbd.ShowDialog() != DialogResult.OK)
            return ChunkCallbackResult.Unchanged;

        long exportedTextures = 0;
        long skippedTextures = 0;
        FileExistsResult? fileExistsResult = null;
        foreach (var texture in textures)
            Exporter.ExportTexture(fbd.SelectedPath, texture, ref fileExistsResult, ref exportedTextures, ref skippedTextures);

        MessageBox.Show($"Exported textures: {exportedTextures}\nSkipped textures: {skippedTextures}\nOutput path: {fbd.SelectedPath}", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return ChunkCallbackResult.Unchanged;
    }
}
