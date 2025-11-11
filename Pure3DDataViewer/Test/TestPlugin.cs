using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace Test;

public class TestPlugin : IPlugin
{
    public string Name => "Test";

    private static readonly List<IFileHandler> FileHandlers;

    static TestPlugin()
    {
        FileHandlers = [
            
        ];

        var assembly = Assembly.GetExecutingAssembly();
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
