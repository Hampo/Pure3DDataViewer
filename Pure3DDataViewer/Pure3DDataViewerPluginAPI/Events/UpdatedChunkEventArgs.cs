using NetP3DLib.P3D;

namespace Pure3DDataViewerPluginAPI.Events;
public class UpdatedChunkEventArgs : EventArgs
{
    public Chunk Chunk { get; }

    public UpdatedChunkEventArgs(Chunk chunk) => Chunk = chunk;
}

public class UpdatedChunkEventArgs<T> : UpdatedChunkEventArgs where T : Chunk
{
    public new T Chunk { get; }

    public UpdatedChunkEventArgs(T chunk) : base(chunk)
    {
        Chunk = chunk;
    }
}