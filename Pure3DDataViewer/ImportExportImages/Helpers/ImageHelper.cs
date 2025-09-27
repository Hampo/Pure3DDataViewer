using NetP3DLib.P3D.Chunks;

namespace ImportExportImages.Helpers;
internal static class ImageHelper
{
    private readonly static HashSet<ImageChunk.Formats> SupportedFormats =
    [
        ImageChunk.Formats.PNG,
        ImageChunk.Formats.BMP,
    ];
    public static bool CanExportFormat(this ImageChunk imageChunk) => SupportedFormats.Contains(imageChunk.Format);

    public static bool SaveImage(this ImageChunk imageChunk, string filePath)
    {
        using var image = imageChunk.GetImage();

        if (image == null)
            return false;

        image.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
        return true;
    }

    public static Image? GetImage(this ImageChunk imageChunk)
    {
        if (!imageChunk.CanExportFormat())
            return null;
        var imageData = imageChunk.GetFirstChunkOfType<ImageDataChunk>();
        if (imageData == null)
            return null;

        Image? image = null;

        switch (imageChunk.Format)
        {
            case ImageChunk.Formats.PNG:
            case ImageChunk.Formats.BMP:
                using (var ms = new MemoryStream(imageData.ImageData))
                    image = Image.FromStream(ms);
                break;
        }

        return image;
    }
}
