using ImportExportTexture.Helpers;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;

namespace ImportExportTexture.Handlers;
public class ExportAllTextures : IFileHandler
{
    public string Name => "Export All Textures";

    public Image? Image => ImportExportTexturePlugin.ExportImage;

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
            ClientGuid = new("3ba41a48-caea-47ec-86d8-6622da9a99ca"),
            Description = "Choose folder to export all textures to",
            ShowNewFolderButton = true,
            UseDescriptionForTitle = true,
        };
        if (fbd.ShowDialog() != DialogResult.OK)
            return FileCallbackResult.Unchanged;

        long exportedTextures = 0;
        long skippedTextures = 0;
        foreach (var texture in textures)
        {
            var image = texture.GetFirstChunkOfType<ImageChunk>();
            if (image == null)
            {
                skippedTextures++;
                continue;
            }

            try
            {
                if (image.SaveImage(Path.Combine(fbd.SelectedPath, $"{texture.Name.SanitizeFileName()}.png")))
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
