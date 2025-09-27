using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;

namespace Pure3DDataViewerPluginAPI;

public interface IChunkHandler
{
    public string Name { get; }
    public Type? ChunkType { get; }
    public Image? Image { get; }
    public IList<(string Name, bool Value)>? GetSettings();
    public void SetSetting(string name, bool value);
    public ChunkCallbackResult Handle(Chunk chunk);
}

public interface IChunkHandler<T> : IChunkHandler where T : Chunk
{
    Type IChunkHandler.ChunkType => typeof(T);

    ChunkCallbackResult IChunkHandler.Handle(Chunk chunk)
    {
        ArgumentNullException.ThrowIfNull(chunk, nameof(chunk));
        var chunkType = chunk.GetType();
        if (chunkType != typeof(T))
            throw new ArgumentException($"{nameof(chunk)}'s type {chunkType} does not match required type {typeof(T)}.", nameof(chunk));

        return Handle((T)chunk);
    }

    ChunkCallbackResult Handle(T chunk);
}
