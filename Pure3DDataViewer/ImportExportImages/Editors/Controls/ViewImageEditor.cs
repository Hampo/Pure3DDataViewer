using ImportExportImages.Helpers;
using NetP3DLib.P3D.Chunks;

namespace ImportExportImages.Editors.Controls;
public partial class ViewImageEditor : UserControl
{
    public ViewImageEditor(ImageChunk imageChunk)
    {
        InitializeComponent();

        PBImage.Image = imageChunk?.GetImage();
    }
}
