using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace CustomChunkTest;

public class CustomChunkTestPlugin : IPlugin
{
    public string Name => "Custom Chunks Test";

    public CustomChunkTestPlugin()
    {
        ChunkLoader.LoadChunkTypes("CustomChunkTest.Chunks", true, true, Assembly.GetExecutingAssembly());
    }

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;
}
