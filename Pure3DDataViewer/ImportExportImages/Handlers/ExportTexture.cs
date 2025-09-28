using ImportExportImages.Helpers;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ExportTexture : IChunkHandler<TextureChunk>
{
    public string Name => "Export Texture";

    public Image? Image => ImportExportImagesPlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(TextureChunk texture)
    {
        var image = texture.GetFirstChunkOfType<ImageChunk>();
        if (image == null)
        {
            MessageBox.Show("Texture contains no child image to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        if (!image.CanExportFormat())
        {
            MessageBox.Show($"Texture contains a currently unsupported format {image.Format}.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        var imageData = image.GetFirstChunkOfType<ImageDataChunk>();
        if (imageData == null)
        {
            MessageBox.Show("Texture contains no image data to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        using var sfd = new SaveFileDialog()
        {
            ClientGuid = ImportExportImagesPlugin.ExportGuid,
            CheckWriteAccess = true,
            OverwritePrompt = true,
            Filter = "PNG Files (*.png)|*.png",
            FileName = $"{texture.Name.SanitizeFileName()}.png",
        };
        if (sfd.ShowDialog() != DialogResult.OK)
            return ChunkCallbackResult.Unchanged;

        try
        {
            if (image.SaveImage(sfd.FileName))
            {
                MessageBox.Show($"Saved texture \"{texture.Name}\" to \"{sfd.FileName}\".", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Unable to save texture \"{texture.Name}\".", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving texture \"{texture.Name}\" to \"{sfd.FileName}\": {ex}", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }
}
