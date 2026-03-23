using NetP3DLib.P3D;

namespace Pure3DDataViewer.UndoRedo;

internal class CommandManager
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }

    public void Execute(ICommand command)
    {
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo(P3DFile file)
    {
        if (_undoStack.Count == 0)
            return;

        var command = _undoStack.Pop();
        command.Undo(file);
        _redoStack.Push(command);
    }

    public void Redo(P3DFile file)
    {
        if (_redoStack.Count == 0)
            return;

        var command = _redoStack.Pop();
        command.Redo(file);
        _undoStack.Push(command);
    }

    public string? UndoChange => _undoStack.TryPeek(out var undo) ? undo.Change : null;
    public string? RedoChange => _redoStack.TryPeek(out var redo) ? redo.Change : null;
}
