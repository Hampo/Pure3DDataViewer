using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace TimeOfDayTint;

public class TimeOfDayTintPlugin : IPlugin
{
    public string Name => "Time of Day Tint";

    private static readonly List<IFileHandler> FileHandlers;

    internal static Image ColorScaleImage;

    static TimeOfDayTintPlugin()
    {
        FileHandlers = [
            new Handlers.TimeOfDayTint(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("TimeOfDayTint.ColorScale_16x.png"))
            ColorScaleImage = Image.FromStream(stream!);
    }

    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => null;

    public IEnumerable<IChunkEditor>? GetChunkEditors() => null;
}
