using NetP3DLib.P3D.Chunks;
using NetP3DLib.P3D.Enums;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Drawing.Imaging;

namespace ImportExportImages.Handlers;
public class ImportImage : IChunkHandler<ImageChunk>
{
    public string Name => "Import Image";

    public Image? Image => ImportExportImagesPlugin.ImportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(ImageChunk imageChunk)
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
            return ChunkCallbackResult.Unchanged;

        try
        {
            using var img = Image.FromFile(ofd.FileName);

            var width = (uint)img.Width;
            var height = (uint)img.Height;
            var bpp = (uint)Image.GetPixelFormatSize(img.PixelFormat);
            var alphaDepth = GetAlphaDepth(img.PixelFormat);
            bool hasAlpha = (img.PixelFormat & PixelFormat.Alpha) != 0 || (img.PixelFormat & PixelFormat.PAlpha) != 0;
            bool isPalettized = (img.PixelFormat & PixelFormat.Indexed) != 0;

            byte[] pngBytes;
            using var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            pngBytes = ms.ToArray();

            for (int i = imageChunk.Children.Count - 1; i >= 0; i--)
                if (imageChunk.Children[i].ID == (uint)ChunkIdentifier.Image_Data)
                    imageChunk.Children.RemoveAt(i);

            imageChunk.Width = width;
            imageChunk.Height = height;
            imageChunk.Bpp = bpp;
            imageChunk.HasAlpha = hasAlpha;
            imageChunk.Palettized = isPalettized;

            var imageDataChunk = new ImageDataChunk(pngBytes);
            imageChunk.Children.Add(imageDataChunk);

            return ChunkCallbackResult.ModifiedChildren;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error importing image \"{ofd.FileName}\": {ex}", "Error Importing Image", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }

    private static uint GetAlphaDepth(PixelFormat format) => format switch
    {
        PixelFormat.Format32bppArgb => 8,
        PixelFormat.Format32bppPArgb => 8,
        PixelFormat.Format64bppArgb => 16,
        PixelFormat.Format64bppPArgb => 16,
        PixelFormat.Format16bppArgb1555 => 1,
        _ => 0
    };
}
