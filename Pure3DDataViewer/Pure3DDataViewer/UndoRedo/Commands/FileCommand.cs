using NetP3DLib.P3D;

namespace Pure3DDataViewer.UndoRedo.Commands;

internal class FileCommand(string change, P3DFile fileBefore, P3DFile fileAfter) : ICommand
{
    public string Change { get; } = change;

    private readonly List<Chunk> _before = CloneChunks(fileBefore);
    private readonly List<Chunk> _after = CloneChunks(fileAfter);

    private static List<Chunk> CloneChunks(P3DFile file) => [.. file.Chunks.Select(c => c.Clone())];

    public void Redo(P3DFile file) => Restore(file, _after);

    public void Undo(P3DFile file) => Restore(file, _before);

    private static void Restore(P3DFile file, List<Chunk> chunks)
    {
        file.Chunks.Clear();
        file.Chunks.AddRange(chunks.Select(c => c.Clone()));
    }
}
