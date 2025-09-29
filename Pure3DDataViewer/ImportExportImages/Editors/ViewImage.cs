using ImportExportImages.Editors.Controls;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Events;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Editors;
public class ViewImage : IChunkEditor<ImageChunk>
{
    public string Name => "View";

    public event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;

    public UserControl GetEditor(ImageChunk chunk) => new ViewImageEditor(chunk);
}
