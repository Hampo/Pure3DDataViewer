using ImportExportImages.Helpers;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ImportExportImages.Handlers;
public class ImportImageFolder : IFileHandler
{
    public string Name => "Import Image Folder";

    public Image? Image => ImportExportImagesPlugin.ImportImage;

    public static bool IncludeSubDirectories
    {
        get => RegistryUtils.GetBoolean("ImportImageFolderIncludeSubDirectories", false)!.Value;
        set => RegistryUtils.SetBoolean("ImportImageFolderIncludeSubDirectories", value);
    }

    public IList<(string Name, bool Value)>? GetSettings() => [
        ( "Include Sub Directories", IncludeSubDirectories ),
    ];

    public void SetSetting(string name, bool value)
    {
        switch (name)
        {
            case "Include Sub Directories":
                IncludeSubDirectories = value;
                break;
            default:
                MessageBox.Show($"Unsupported setting name: {name}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
        }
    }

    private static readonly HashSet<string> SupportedImageFormats = [".bmp", ".gif", ".jpg", ".jpeg", ".jpe", ".jfif", ".png", ".tif", ".tiff", ".ico"];
    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        using var fbd = new FolderBrowserDialog()
        {
            Description = "Choose folder to import",
            ShowNewFolderButton = false,
            ClientGuid = ImportExportImagesPlugin.ImportGuid,
        };
        if (fbd.ShowDialog() != DialogResult.OK)
            return FileCallbackResult.Unchanged;

        try
        {
            var files = Directory.GetFiles(fbd.SelectedPath, "*.*", IncludeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
            List<TextureChunk> textureChunks = new(files.Length);

            for (int i = files.Length - 1; i >= 0; i--)
            {
                var file = files[i];
                if (!SupportedImageFormats.Contains(Path.GetExtension(file).ToLower()))
                    continue;

                try
                {
                    textureChunks.Add(Importer.ImportTexture(file));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error importing: \"{file}\": {ex}", "Error Importing Texture", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (textureChunks.Count == 0)
            {
                MessageBox.Show($"No supported images found in top level of folder: \"{fbd.SelectedPath}\".", "No Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FileCallbackResult.Unchanged;
            }

            foreach (var textureChunk in textureChunks)
                p3dFile.Chunks.Insert(0, textureChunk);

            MessageBox.Show($"Imported {textureChunks.Count} textures from folder: \"{fbd.SelectedPath}\".", "Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return FileCallbackResult.Modified;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error importing textures from folder: \"{fbd.SelectedPath}\": {ex}", "Error Importing Textures", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
