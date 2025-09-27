using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using System.Reflection;

namespace ImportExportImages;

public class ImportExportImagesPlugin : IPlugin
{
    public string Name => "Import/Export Images";

    private static readonly List<IChunkHandler<TextureChunk>> TextureChunkHandlers;
    private static readonly List<IFileHandler> FileHandlers;

    internal static Image ImportImage;
    internal static Image ExportImage;

    static ImportExportImagesPlugin()
    {
        TextureChunkHandlers = [
            new Handlers.ExportTexture(),
            new Handlers.ImportTexture(),
        ];

        FileHandlers = [

            new Handlers.ExportAllTextures(),
        ];

        var assembly = Assembly.GetExecutingAssembly();

        using (var stream = assembly.GetManifestResourceStream("ImportExportImages.ExportTheme_16x.png"))
            ExportImage = Image.FromStream(stream!);

        using (var stream = assembly.GetManifestResourceStream("ImportExportImages.ImportTheme_16x.png"))
            ImportImage = Image.FromStream(stream!);
    }
    
    public IEnumerable<IFileHandler>? GetFileHandlers() => FileHandlers;

    public IEnumerable<IChunkHandler>? GetChunkHandlers() => TextureChunkHandlers;
}
