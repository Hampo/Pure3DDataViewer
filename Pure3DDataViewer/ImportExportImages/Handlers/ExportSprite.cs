using Pure3DDataViewerPluginAPI.Extensions;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ExportSprite : IChunkHandler<SpriteChunk>
{
    public string Name => "Export Sprite";

    public Image? Image => ImportExportImagesPlugin.ExportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(SpriteChunk sprite)
    {
        if (!sprite.CanExport())
        {
            MessageBox.Show($"Sprite is unsupported. Either invalid image or unsupported format.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        using var sfd = new SaveFileDialog()
        {
            ClientGuid = ImportExportImagesPlugin.ExportGuid,
            CheckWriteAccess = true,
            OverwritePrompt = true,
            Filter = "PNG Files (*.png)|*.png",
            FileName = $"{sprite.Name.SanitizeFileName()}.png",
        };
        if (sfd.ShowDialog() != DialogResult.OK)
            return ChunkCallbackResult.Unchanged;

        try
        {
            if (sprite.SaveImage(sfd.FileName))
            {
                MessageBox.Show($"Saved sprite \"{sprite.Name}\" to \"{sfd.FileName}\".", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Unable to save sprite \"{sprite.Name}\".", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving sprite \"{sprite.Name}\" to \"{sfd.FileName}\": {ex}", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }
}
