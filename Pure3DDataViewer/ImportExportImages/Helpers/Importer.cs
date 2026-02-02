using ImportExportImages.Extensions;
using NetP3DLib.P3D.Chunks;
using System.Drawing.Imaging;

namespace ImportExportImages.Helpers;

internal static class Importer
{
    internal static TextureChunk ImportTexture(string imagePath)
    {
        using var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var img = Image.FromStream(fs);

        var width = (uint)img.Width;
        var height = (uint)img.Height;
        var bpp = (uint)Image.GetPixelFormatSize(img.PixelFormat);
        var alphaDepth = img.PixelFormat.GetAlphaDepth();
        bool hasAlpha = (img.PixelFormat & PixelFormat.Alpha) != 0 || (img.PixelFormat & PixelFormat.PAlpha) != 0;
        bool isPalettized = (img.PixelFormat & PixelFormat.Indexed) != 0;

        using var ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        var pngBytes = ms.GetBuffer().AsSpan(0, (int)ms.Length).ToArray();

        var textureChunk = new TextureChunk(Path.GetFileName(imagePath), 14000, width, height, bpp, alphaDepth, 0, isPalettized ? TextureChunk.TextureTypes.Palettized : TextureChunk.TextureTypes.RGB, TextureChunk.UsageHints.Static, 0);

        var imageChunk = new ImageChunk(textureChunk.Name, 14000, width, height, bpp, isPalettized, hasAlpha, ImageChunk.Formats.PNG);
        textureChunk.Children.Add(imageChunk);

        var imageDataChunk = new ImageDataChunk(pngBytes);
        imageChunk.Children.Add(imageDataChunk);

        return textureChunk;
    }
}
