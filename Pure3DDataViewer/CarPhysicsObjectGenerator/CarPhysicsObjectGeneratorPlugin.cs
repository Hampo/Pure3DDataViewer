using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace CarPhysicsObjectGenerator;

public class CarPhysicsObjectGeneratorPlugin : IPlugin
{
    public string Name => "Car Physics Object Generator";

    private static readonly List<IFileHandler> FileHandlers;

    internal static Image CreateImage;

    static CarPhysicsObjectGeneratorPlugin()
    {
        FileHandlers = [
            new Handlers.GeneratePhysicsObject(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("CarPhysicsObjectGenerator.Create_16x.png"))
            CreateImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
