using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace Validate;

public class ValidatePlugin : IPlugin
{
    public string Name => "Validate";

    private static readonly List<IFileHandler> FileHandlers;

    private static readonly List<IChunkHandler> ChunkHandlers;

    internal static Image ValidateImage;

    static ValidatePlugin()
    {
        FileHandlers = [
            new Handlers.ValidateFile(),
        ];

        ChunkHandlers = [
            new Handlers.ValidateChunk(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("Validate.ValidateDocument_16x.png"))
            ValidateImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => ChunkHandlers;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
