using ImportExportImages.Helpers;
using NetP3DLib.P3D.Chunks;

namespace ImportExportImages.Editors.Controls;
public partial class ViewTextureEditor : UserControl
{
    public ViewTextureEditor(TextureChunk textureChunk)
    {
        InitializeComponent();

        var imageChunk = textureChunk.GetFirstChunkOfType<ImageChunk>();
        PBImage.Image = imageChunk?.GetImage();
    }
}
