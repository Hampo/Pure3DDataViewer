using NetP3DLib.P3D.Chunks;

namespace ImportExportImages.Editors.Controls;
public class ViewTextureEditor(TextureChunk textureChunk) : ViewImageEditor(textureChunk.GetFirstChunkOfType<ImageChunk>())
{
}