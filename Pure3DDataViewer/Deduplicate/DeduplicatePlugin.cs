using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace Deduplicate;

public class DeduplicatePlugin : IPlugin
{
    public string Name => "Deduplicate";

    private static readonly List<IFileHandler> FileHandlers;

    internal static Image DeduplicateImage;
    internal static Image FindDuplicateNamedImage;

    static DeduplicatePlugin()
    {
        FileHandlers = [

            new Handlers.FindDuplicateNamedChunks(),
            new Handlers.DeduplicateChunks(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("Deduplicate.RemoveFromCollection_16x.png"))
            DeduplicateImage = Image.FromStream(stream!);

        using (var stream = assembly.GetManifestResourceStream("Deduplicate.SearchProperty_16x.png"))
            FindDuplicateNamedImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
