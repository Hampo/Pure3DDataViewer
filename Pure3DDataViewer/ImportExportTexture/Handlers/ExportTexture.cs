using ImportExportTexture.Helpers;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;

namespace ImportExportTexture.Handlers;
public class ExportTexture : IChunkHandler<TextureChunk>
{
    public string Name => "Export Texture";

    public Image? Image => ImportExportTexturePlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(TextureChunk texture)
    {
        var image = texture.GetFirstChunkOfType<ImageChunk>();
        if (image == null)
        {
            MessageBox.Show("Texture contains no child image to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return ChunkCallbackResult.Unchanged;
        }

        var imageData = image.GetFirstChunkOfType<ImageDataChunk>();
        if (imageData == null)
        {
            MessageBox.Show("Texture contains no image data to export.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return ChunkCallbackResult.Unchanged;
        }

        using var sfd = new SaveFileDialog()
        {
            ClientGuid = new("6d1b4f0e-9342-4b69-82e8-4d1509352b44"),
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
