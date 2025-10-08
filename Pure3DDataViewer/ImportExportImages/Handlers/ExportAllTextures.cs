using ImportExportImages.Enums;
using ImportExportImages.Forms;
using ImportExportImages.Helpers;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ExportAllTextures : IFileHandler
{
    public string Name => "Export All Textures";

    public Image? Image => ImportExportImagesPlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        var textures = p3dFile.GetChunksOfType<TextureChunk>();
        var sets = p3dFile.GetChunksOfType<SetChunk>();
        if (textures.Count == 0 && sets.Count == 0)
        {
            MessageBox.Show("No textures to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        using var fbd = new FolderBrowserDialog()
        {
            ClientGuid = ImportExportImagesPlugin.ExportAllGuid,
            Description = "Choose folder to export all textures to",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true,
        };
        if (fbd.ShowDialog() != DialogResult.OK)
            return FileCallbackResult.Unchanged;

        long exportedTextures = 0;
        long skippedTextures = 0;
        FileExistsResult? fileExistsResult = null;
        foreach (var texture in textures)
            Exporter.ExportTexture(fbd.SelectedPath, texture, ref fileExistsResult, ref exportedTextures, ref skippedTextures);
        foreach (var set in sets)
        {
            var dir = Path.Combine(fbd.SelectedPath, set.Name.SanitizeDirectoryName());
            Directory.CreateDirectory(dir);
            foreach (var texture in set.GetChunksOfType<TextureChunk>())
                Exporter.ExportTexture(dir, texture, ref fileExistsResult, ref exportedTextures, ref skippedTextures);
        }

        MessageBox.Show($"Exported textures: {exportedTextures}\nSkipped textures: {skippedTextures}\nOutput path: {fbd.SelectedPath}", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile)
    {
        var textures = p3dFile.GetChunksOfType<TextureChunk>();
        if (textures.Count != 0)
            return true;

        var sets = p3dFile.GetChunksOfType<SetChunk>();
        if (sets.Count != 0)
            return true;

        return false;
    }
}
