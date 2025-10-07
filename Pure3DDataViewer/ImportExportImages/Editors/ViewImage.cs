using ImportExportImages.Editors.Controls;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Editors;
public class ViewImage : IChunkEditor
{
    public string Name => "View";
    public HashSet<Type> ChunkTypes =>
    [
        typeof(ImageChunk),
        typeof(TextureChunk),
        //typeof(SpriteChunk),
    ];
    public EditorControl Editor => new ViewImageEditor();
}
