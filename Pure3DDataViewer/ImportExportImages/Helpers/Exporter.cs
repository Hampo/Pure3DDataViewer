using ImportExportImages.Enums;
using ImportExportImages.Forms;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Extensions;

namespace ImportExportImages.Helpers;
internal static class Exporter
{
    internal static void ExportTexture(string dir, TextureChunk texture, ref FileExistsResult? fileExistsResult, ref long exportedTextures, ref long skippedTextures)
    {

        var image = texture.GetFirstChunkOfType<ImageChunk>();
        if (image == null)
        {
            skippedTextures++;
            return;
        }

        if (!image.CanExportFormat())
        {
            skippedTextures++;
            return;
        }

        try
        {
            var filePath = Path.Combine(dir, $"{texture.Name.SanitizeFileName()}.png");

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
                        skippedTextures++;
                        return;
                    case FileExistsResult.KeepBoth:
                        var count = 1;

                        do
                            filePath = Path.Combine(dir, $"{texture.Name.SanitizeFileName()} ({count++}).png");
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
            MessageBox.Show($"Failed to export texture \"{texture.Name}\": {ex}", "Export Texture", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    internal static void ExportSprite(string dir, SpriteChunk sprite, ref FileExistsResult? fileExistsResult, ref long exportedSprites, ref long skippedSprites)
    {
        if (!sprite.CanExport())
        {
            skippedSprites++;
            return;
        }

        try
        {
            var filePath = Path.Combine(dir, $"{sprite.Name.SanitizeFileName()}.png");

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
                        skippedSprites++;
                        return;
                    case FileExistsResult.KeepBoth:
                        var count = 1;

                        do
                            filePath = Path.Combine(dir, $"{sprite.Name.SanitizeFileName()} ({count++}).png");
                        while (File.Exists(filePath));

                        break;
                }
            }

            if (sprite.SaveImage(filePath))
                exportedSprites++;
            else
                skippedSprites++;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to export sprite \"{sprite.Name}\": {ex}", "Export Sprite", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
