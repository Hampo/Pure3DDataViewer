using ImportExportImages.Helpers;
using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ImportTextureFromFile : IFileHandler
{
    public string Name => "Import Texture From File";

    public Image? Image => ImportExportImagesPlugin.ImportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        using var ofd = new OpenFileDialog()
        {
            CheckFileExists = true,
            ClientGuid = ImportExportImagesPlugin.ImportGuid,
            Filter = "Image Files|*.bmp;*.gif;*.jpg;*.jpeg;*.jpe;*.jfif;*.png;*.tif;*.tiff;*.ico|Bitmap (*.bmp)|*.bmp|GIF (*.gif)|*.gif|JPEG (*.jpg;*.jpeg;*.jpe;*.jfif)|*.jpg;*.jpeg;*.jpe;*.jfif|PNG (*.png)|*.png|TIFF (*.tif;*.tiff)|*.tif;*.tiff|Icon (*.ico)|*.ico|All Files (*.*)|*.*",
            Multiselect = false,
            Title = "Choose image to import",
        };
        if (ofd.ShowDialog() != DialogResult.OK)
            return FileCallbackResult.Unchanged;

        try
        {
            var textureChunk = Importer.ImportTexture(ofd.FileName);

            p3dFile.Chunks.Insert(0, textureChunk);

            return FileCallbackResult.Modified;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error importing texture \"{ofd.FileName}\": {ex}", "Error Importing Texture", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
