using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace MonstersIncScreamArenaChunks;

public class MonstersIncScreamArenaChunksPlugin : IPlugin
{
    public string Name => "Monsters Inc Scream Arena Chunks";

    public MonstersIncScreamArenaChunksPlugin()
    {
        ChunkLoader.LoadChunkTypes("MonstersIncScreamArenaChunks.Chunks", false, true, Assembly.GetExecutingAssembly());
    }

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;
}
