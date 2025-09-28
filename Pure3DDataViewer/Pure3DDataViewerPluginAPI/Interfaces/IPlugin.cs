using NetP3DLib.P3D;

namespace Pure3DDataViewerPluginAPI.Interfaces;

public interface IPlugin
{
    public string Name { get; }

    public IEnumerable<IFileHandler>? GetFileHandlers();

    public IEnumerable<IChunkHandler>? GetChunkHandlers();

    public IEnumerable<IChunkHandler>? GetChunkHandlers(Type chunkType) => GetChunkHandlers()?.Where(x => x.GetType() == chunkType);

    public IEnumerable<IChunkHandler>? GetChunkHandlers<T>() where T : Chunk => GetChunkHandlers(typeof(T));
}
