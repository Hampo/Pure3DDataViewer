using Pure3DDataViewerPluginAPI.Interfaces;

namespace FrontendTextBibleEditor;

public class FrontendTextBibleEditorPlugin : IPlugin
{
    public string Name => "Frontend Bible Editor";

    private static readonly List<IChunkEditor> ChunkEditors;

    static FrontendTextBibleEditorPlugin()
    {
        ChunkEditors = [
            new Editors.FrontendTextBible(),
        ];
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => ChunkEditors;
}
