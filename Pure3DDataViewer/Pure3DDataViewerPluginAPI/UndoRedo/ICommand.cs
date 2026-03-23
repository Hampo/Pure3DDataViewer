using NetP3DLib.P3D;

namespace Pure3DDataViewerPluginAPI.UndoRedo;

public interface ICommand
{
    public abstract string Change { get; }

    public abstract void Redo(P3DFile p3dFile);
    public abstract void Undo(P3DFile p3dFile);
}