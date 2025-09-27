using Pure3DDataViewerPluginAPI;
using System.Reflection;

namespace Sort;

public class SortPlugin : IPlugin
{
    public string Name => "Sort";

    private static readonly List<IFileHandler> FileHandlers;

    internal static Image SortImage;

    static SortPlugin()
    {
        FileHandlers = [

            new Handlers.SortChunks(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("Sort.Sort_16x.png"))
            SortImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;
}
