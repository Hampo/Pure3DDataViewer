using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace LocationFromGame;

public class LocationFromGamePlugin : IPlugin
{
    public string Name => "Location From Game";

    private static readonly List<IChunkHandler> ChunkHandlers;

    internal static Image FromGameImage;

    static LocationFromGamePlugin()
    {
        ChunkHandlers = [
            new Handlers.LocatorFromGame(),
            new Handlers.TriggerVolumeFromGame(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("LocationFromGame.CubeDimension_16x.png"))
            FromGameImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => null;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => ChunkHandlers;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
