using System.Drawing.Imaging;

namespace ImportExportImages.Extensions;

internal static class PixelFormatExtensions
{
    public static uint GetAlphaDepth(this PixelFormat format) => format switch
    {
        PixelFormat.Format32bppArgb => 8,
        PixelFormat.Format32bppPArgb => 8,
        PixelFormat.Format64bppArgb => 16,
        PixelFormat.Format64bppPArgb => 16,
        PixelFormat.Format16bppArgb1555 => 1,
        _ => 0
    };
}
