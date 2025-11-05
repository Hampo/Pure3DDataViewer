using ImportExportImages.Enums;
using ImportExportImages.Helpers;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ExportAllSprites : IFileHandler
{
    public string Name => "Export All Sprites";

    public Image? Image => ImportExportImagesPlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        var sprites = p3dFile.GetChunksOfType<SpriteChunk>();
        if (sprites.Count == 0)
        {
            MessageBox.Show("No sprites to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        long exportedSprites = 0;
        long skippedSprites = 0;
        FileExistsResult? fileExistsResult = null;
        foreach (var sprite in sprites)
            Exporter.ExportSprite(fbd.SelectedPath, sprite, ref fileExistsResult, ref exportedSprites, ref skippedSprites);

        MessageBox.Show($"Exported sprites: {exportedSprites}\nSkipped sprites: {skippedSprites}\nOutput path: {fbd.SelectedPath}", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile)
    {
        var sprites = p3dFile.GetChunksOfType<SpriteChunk>();
        if (sprites.Count != 0)
            return true;

        return false;
    }
}
