using NetP3DLib.P3D;
using System.Collections.ObjectModel;

namespace Pure3DDataViewer.UndoRedo.Commands;

internal class AddChunkCommand(string change, IList<int> hierarchy, Chunk chunk) : ICommand
{
    public string Change { get; } = change;

    private readonly ReadOnlyCollection<int> _hierarchy = hierarchy.AsReadOnly();
    private readonly Chunk _chunk = chunk.Clone();

    public void Redo(P3DFile p3dFile) => GetParent(p3dFile).Insert(GetIndex(), _chunk.Clone());

    public void Undo(P3DFile p3dFile) => GetParent(p3dFile).RemoveAt(GetIndex());

    private Collection<Chunk> GetParent(P3DFile file)
    {
        if (_hierarchy.Count == 1)
            return file.Chunks;

        var chunk = file.Chunks[_hierarchy[^1]];
        for (int i = _hierarchy.Count - 2; i > 0; i--)
            chunk = chunk.Children[_hierarchy[i]];

        return chunk.Children;
    }

    private int GetIndex() => _hierarchy[0];
}
