using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using System.Reflection;

namespace ImportExportImages;

public class ImportExportImagesPlugin : IPlugin
{
    public string Name => "Import/Export Images";

    private static readonly List<IFileHandler> FileHandlers;
    private static readonly List<IChunkHandler> ChunkHandlers;

    internal static Image ImportImage;
    internal static Image ExportImage;

    internal static Guid ImportGuid = new("e7effd7e-405d-4056-9866-32f656a08ad2");
    internal static Guid ExportGuid = new("6d1b4f0e-9342-4b69-82e8-4d1509352b44");
    internal static Guid ExportAllGuid = new("3ba41a48-caea-47ec-86d8-6622da9a99ca");

    static ImportExportImagesPlugin()
    {
        FileHandlers = [
            new Handlers.ExportAllTextures(),
        ];

        ChunkHandlers = [
            new Handlers.ExportTexture(),
            new Handlers.ImportTexture(),

            new Handlers.ExportImage(),
            //new Handlers.ImportImage(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("ImportExportImages.ExportTheme_16x.png"))
            ExportImage = Image.FromStream(stream!);

        using (var stream = assembly.GetManifestResourceStream("ImportExportImages.ImportTheme_16x.png"))
            ImportImage = Image.FromStream(stream!);
    }
    
    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => ChunkHandlers;
}
