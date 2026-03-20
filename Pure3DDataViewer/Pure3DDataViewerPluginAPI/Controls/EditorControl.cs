using NetP3DLib.P3D;

namespace Pure3DDataViewerPluginAPI.Controls;
public class EditorControl : UserControl
{
    public virtual bool NoTheming => false;

    public virtual void LoadChunk(Chunk chunk) => throw new NotImplementedException();
}