using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Controls;

namespace Pure3DDataViewerPluginAPI.Interfaces;

public interface IChunkEditor
{
    public string Name { get; }
    public Type ChunkType { get; }
    public EditorControl Editor { get; }
}

public interface IChunkEditor<T> : IChunkEditor where T : Chunk
{
    Type IChunkEditor.ChunkType => typeof(T);

    EditorControl IChunkEditor.Editor => Editor;
    public new EditorControl<T> Editor { get; }
}
