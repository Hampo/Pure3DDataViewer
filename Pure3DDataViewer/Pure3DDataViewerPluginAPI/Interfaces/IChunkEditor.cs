using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Events;

namespace Pure3DDataViewerPluginAPI.Interfaces;

public interface IChunkEditor
{
    public string Name { get; }
    public Type ChunkType { get; }
    public UserControl GetEditor(Chunk chunk);
    event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;
}

public interface IChunkEditor<T> : IChunkEditor where T : Chunk
{
    Type IChunkEditor.ChunkType => typeof(T);

    UserControl IChunkEditor.GetEditor(Chunk chunk)
    {
        ArgumentNullException.ThrowIfNull(chunk, nameof(chunk));
        var chunkType = chunk.GetType();
        if (chunkType != typeof(T))
            throw new ArgumentException($"{nameof(chunk)}'s type {chunkType} does not match required type {typeof(T)}.", nameof(chunk));

        return GetEditor((T)chunk);
    }
    public UserControl GetEditor(T chunk);
}
