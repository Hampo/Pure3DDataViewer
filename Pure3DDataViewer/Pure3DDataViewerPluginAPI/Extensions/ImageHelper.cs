using NetP3DLib.P3D.Chunks;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Pure3DDataViewerPluginAPI.Extensions;
public static class ImageHelper
{
    private readonly static HashSet<ImageChunk.Formats> SupportedFormats =
    [
        ImageChunk.Formats.PNG,
        ImageChunk.Formats.BMP,
        ImageChunk.Formats.DXT,
        ImageChunk.Formats.DXT1,
        ImageChunk.Formats.DXT2,
        ImageChunk.Formats.DXT3,
        ImageChunk.Formats.DXT4,
        ImageChunk.Formats.DXT5,
        ImageChunk.Formats.GCDXT1,
    ];
    public static bool CanExportFormat(this ImageChunk imageChunk) => SupportedFormats.Contains(imageChunk.Format);

    public static bool SaveImage(this ImageChunk imageChunk, string filePath)
    {
        using var image = imageChunk.GetImage();

        if (image == null)
            return false;

        image.Save(filePath, ImageFormat.Png);
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
            case ImageChunk.Formats.DXT:
            case ImageChunk.Formats.DXT1:
            case ImageChunk.Formats.DXT2:
            case ImageChunk.Formats.DXT3:
            case ImageChunk.Formats.DXT4:
            case ImageChunk.Formats.DXT5:
            case ImageChunk.Formats.GCDXT1:
                var handle = GCHandle.Alloc(imageData.ImageData, GCHandleType.Pinned);
                try
                {
                    var ptr = handle.AddrOfPinnedObject();

                    using var dxtImage = DirectXTexNet.TexHelper.Instance.LoadFromDDSMemory(ptr, imageData.ImageData.Length, DirectXTexNet.DDS_FLAGS.NONE);

                    var format = dxtImage.GetImage(0).Format;
                    if (DirectXTexNet.TexHelper.Instance.IsCompressed(format))
                    {
                        using var decompressedImage = dxtImage.Decompress(DirectXTexNet.DXGI_FORMAT.R8G8B8A8_UNORM);
                        return DXTToBitmap(decompressedImage);
                    }

                    return DXTToBitmap(dxtImage);

                }
                finally
                {
                    handle.Free();
                }
        }

        return image;
    }

    private static Image DXTToBitmap(DirectXTexNet.ScratchImage scratch)
    {
        var img = scratch.GetImage(0);
        int width = (int)img.Width;
        int height = (int)img.Height;

        var bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
        var bmpData = bmp.LockBits(
            new Rectangle(0, 0, width, height),
            ImageLockMode.WriteOnly,
            PixelFormat.Format32bppArgb);

        unsafe
        {
            Buffer.MemoryCopy(
                (void*)img.Pixels,
                (void*)bmpData.Scan0,
                bmpData.Stride * height,
                img.RowPitch * height);
        }

        bmp.UnlockBits(bmpData);
        return bmp;
    }
}
