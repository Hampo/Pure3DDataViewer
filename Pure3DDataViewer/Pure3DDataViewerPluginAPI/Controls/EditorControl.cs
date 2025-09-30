using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Events;

namespace Pure3DDataViewerPluginAPI.Controls;
public class EditorControl : UserControl
{
    public virtual void LoadChunk(Chunk chunk) => throw new NotImplementedException();

    public event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;

    protected void OnUpdatedChunk(Chunk chunk) => UpdatedChunk?.Invoke(this, new(chunk));
}