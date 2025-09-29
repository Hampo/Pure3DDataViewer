using ImportExportImages.Editors.Controls;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Events;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Editors;
public class ViewTexture : IChunkEditor<TextureChunk>
{
    public string Name => "View";

    public event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;

    public UserControl GetEditor(TextureChunk chunk) => new ViewTextureEditor(chunk);
}
