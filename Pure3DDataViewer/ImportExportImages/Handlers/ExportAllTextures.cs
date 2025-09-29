using ImportExportImages.Enums;
using ImportExportImages.Forms;
using Pure3DDataViewerPluginAPI.Extensions;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
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
        if (textures.Count == 0)
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
        {
            var image = texture.GetFirstChunkOfType<ImageChunk>();
            if (image == null)
            {
                skippedTextures++;
                continue;
            }

            if (!image.CanExportFormat())
            {
                skippedTextures++;
                continue;
            }

            try
            {
                var filePath = Path.Combine(fbd.SelectedPath, $"{texture.Name.SanitizeFileName()}.png");

                if (File.Exists(filePath))
                {
                    FileExistsResult result;
                    if (fileExistsResult.HasValue)
                    {
                        result = fileExistsResult.Value;
                    }
                    else
                    {
                        using var fileExistsPrompt = new FrmFileExistsPrompt(filePath);
                        fileExistsPrompt.ShowDialog();

                        if (fileExistsPrompt.ApplyToAll)
                            fileExistsResult = fileExistsPrompt.Result;

                        result = fileExistsPrompt.Result;
                    }

                    switch (result)
                    {
                        case FileExistsResult.KeepOriginal:
                            continue;
                        case FileExistsResult.KeepBoth:
                            var count = 1;

                            do
                                filePath = Path.Combine(fbd.SelectedPath, $"{texture.Name.SanitizeFileName()} ({count++}).png");
                            while (File.Exists(filePath));

                            break;
                    }
                }

                if (image.SaveImage(filePath))
                    exportedTextures++;
                else
                    skippedTextures++;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to export texture \"{texture.Name}\": {ex}", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        MessageBox.Show($"Exported textures: {exportedTextures}\nSkipped textures: {skippedTextures}\nOutput path: {fbd.SelectedPath}", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
        return FileCallbackResult.Unchanged;
    }
}
