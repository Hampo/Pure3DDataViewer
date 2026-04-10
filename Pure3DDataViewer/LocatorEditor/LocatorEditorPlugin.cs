using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocatorEditor;

public class LocatorEditorPlugin : IPlugin
{
    public string Name => "Locator Editor";

    private static readonly List<IChunkEditor> ChunkEditors;

    static LocatorEditorPlugin()
    {
        ChunkEditors = [
            new Editors.Locator(),
        ];
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => ChunkEditors;
}
