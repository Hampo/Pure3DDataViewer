using Pure3DDataViewerPluginAPI.Interfaces;

namespace CompositeDrawableEditor;

public class CompositeDrawableEditorPlugin : IPlugin
{
    public string Name => "Composite Drawable Editor";

    private static readonly List<IChunkEditor> ChunkEditors;

    static CompositeDrawableEditorPlugin()
    {
        ChunkEditors = [
            new Editors.CompositeDrawable(),
        ];
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => ChunkEditors;
}
