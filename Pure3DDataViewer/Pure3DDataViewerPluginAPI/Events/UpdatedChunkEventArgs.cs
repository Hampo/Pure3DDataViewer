using NetP3DLib.P3D;

namespace Pure3DDataViewerPluginAPI.Events;
public class UpdatedChunkEventArgs : EventArgs
{
    public Chunk Chunk { get; }

    public UpdatedChunkEventArgs(Chunk chunk) => Chunk = chunk;
}
