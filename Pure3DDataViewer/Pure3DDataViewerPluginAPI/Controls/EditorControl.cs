using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Events;

namespace Pure3DDataViewerPluginAPI.Controls;
public abstract class EditorControl : UserControl
{
    public abstract Type ChunkType { get; }

    public abstract void LoadChunk(Chunk chunk);

    public event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;

    protected void OnUpdatedChunk(Chunk chunk) => UpdatedChunk?.Invoke(this, new(chunk));
}

public abstract class EditorControl<T> : EditorControl where T : Chunk
{
    public override Type ChunkType => typeof(T);

    public override void LoadChunk(Chunk chunk)
    {
        ArgumentNullException.ThrowIfNull(chunk, nameof(chunk));
        var chunkType = chunk.GetType();
        if (chunkType != typeof(T))
            throw new ArgumentException($"{nameof(chunk)}'s type {chunkType} does not match required type {typeof(T)}.", nameof(chunk));

        LoadChunk((T)chunk);
    }

    public abstract void LoadChunk(T chunk);

    public new event EventHandler<UpdatedChunkEventArgs<T>>? UpdatedChunk;

    protected void OnUpdatedChunk(T chunk)
    {
        UpdatedChunk?.Invoke(this, new UpdatedChunkEventArgs<T>(chunk));

        base.OnUpdatedChunk(chunk);
    }
}