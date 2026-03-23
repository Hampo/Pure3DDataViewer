using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace CompositeDrawableEditor.Editors;

public class CompositeDrawable : IChunkEditor<CompositeDrawableChunk>
{
    public string Name => "Composite Drawable Editor";

    public EditorControl Editor => new Controls.CompositeDrawableEditor();
}
