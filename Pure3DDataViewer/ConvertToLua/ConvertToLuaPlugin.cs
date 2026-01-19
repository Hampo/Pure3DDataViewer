using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace ConvertToLua;

public class ConvertToLuaPlugin : IPlugin
{
    public string Name => "Convert to Lua";

    private static readonly List<IFileHandler> FileHandlers;
    private static readonly List<IChunkHandler> ChunkHandlers;

    internal static Image ConvertImage;

    static ConvertToLuaPlugin()
    {
        FileHandlers = [
            new Handlers.ConvertFile(),
        ];

        ChunkHandlers = [
            new Handlers.ConvertChunk(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("ConvertToLua.ConvertPartition_16x.png"))
            ConvertImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => ChunkHandlers;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
