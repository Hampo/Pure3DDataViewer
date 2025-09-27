using Pure3DDataViewerPluginAPI;
using System.Reflection;

namespace Deduplicate;

public class DeduplicatePlugin : IPlugin
{
    public string Name => "Deduplicate";

    private static readonly List<IFileHandler> FileHandlers;

    internal static Image DeduplicateImage;

    static DeduplicatePlugin()
    {
        FileHandlers = [

            new Handlers.DeduplicateChunks(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("Deduplicate.RemoveFromCollection_16x.png"))
            DeduplicateImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;
}
