using NetP3DLib.P3D.Chunks;
using NetP3DLib.P3D.Enums;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Drawing.Imaging;

namespace ImportExportImages.Handlers;
public class ImportSprite : IChunkHandler<SpriteChunk>
{
    public string Name => "Import Sprite";

    public Image? Image => ImportExportImagesPlugin.ImportImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(SpriteChunk spriteChunk)
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
            using var fs = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var img = Image.FromStream(fs);

            var width = (uint)img.Width;
            var height = (uint)img.Height;
            var bpp = (uint)Image.GetPixelFormatSize(img.PixelFormat);
            bool hasAlpha = (img.PixelFormat & PixelFormat.Alpha) != 0 || (img.PixelFormat & PixelFormat.PAlpha) != 0;
            bool isPalettized = (img.PixelFormat & PixelFormat.Indexed) != 0;

            using var ms = new MemoryStream();
            img.Save(ms, ImageFormat.Png);
            var pngBytes = ms.GetBuffer().AsSpan(0, (int)ms.Length).ToArray();

            for (int i = spriteChunk.Children.Count - 1; i >= 0; i--)
                if (spriteChunk.Children[i].ID == (uint)ChunkIdentifier.Image)
                    spriteChunk.Children.RemoveAt(i);

            var imageChunk = new ImageChunk(spriteChunk.Name, 14000, width, height, bpp, isPalettized, hasAlpha, ImageChunk.Formats.PNG);
            spriteChunk.Children.Add(imageChunk);

            var imageDataChunk = new ImageDataChunk(pngBytes);
            imageChunk.Children.Add(imageDataChunk);

            spriteChunk.BlitBorder = 0;
            if (spriteChunk.ImageHeight != 0 || spriteChunk.ImageWidth != 0)
            {
                spriteChunk.ImageHeight = (uint)img.Height;
                spriteChunk.ImageWidth = (uint)img.Width;
            }

            return ChunkCallbackResult.ModifiedChildren;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error importing sprite \"{ofd.FileName}\": {ex}", "Error Importing Sprite", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }
}
