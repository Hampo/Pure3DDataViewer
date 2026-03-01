using ImportExportImages.Helpers;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Helpers;
using Pure3DDataViewerPluginAPI.Interfaces;
using Pure3DDataViewerPluginAPI.Utils;
using System.Reflection;

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

        var imported = 0;
        try
        {
            var files = Directory.GetFiles(fbd.SelectedPath, "*.*", IncludeSubDirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly).Where(x => SupportedImageFormats.Contains(Path.GetExtension(x).ToLower())).ToArray();

            if (files.Length == 0)
            {
                MessageBox.Show($"No supported images found in top level of folder: \"{fbd.SelectedPath}\".", "No Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FileCallbackResult.Unchanged;
            }

            var (cancelled, textureChunks) = ProgressHelper.Run("Reading textures", (reportProgress, isCancellationRequested) =>
            {
                var chunks = new List<TextureChunk>(files.Length);
                double index = 0;

                for (int i = files.Length - 1; i >= 0; i--)
                {
                    if (isCancellationRequested())
                        break;

                    var file = files[i];
                    try
                    {
                        chunks.Add(Importer.ImportTexture(file));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error importing \"{file}\": {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    reportProgress((int)(index++ / files.Length * 100));
                }

                return chunks;
            });

            if (cancelled)
            {
                MessageBox.Show("Import cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FileCallbackResult.Unchanged;
            }
            else if (textureChunks == null || textureChunks.Count == 0)
            {
                MessageBox.Show($"No supported images found in top level of folder: \"{fbd.SelectedPath}\".", "No Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return FileCallbackResult.Unchanged;
            }

            var (cancelled2, imported2) = ProgressHelper.Run("Adding textures", (reportProgress, isCancellationRequested) =>
            {
                int imported = 0;

                try
                {
                    foreach (var textureChunk in textureChunks)
                    {
                        imported++;
                        p3dFile.Chunks.Insert(0, textureChunk);

                        reportProgress((int)((double)imported / textureChunks.Count * 100));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"There was an error importing textures from folder: \"{fbd.SelectedPath}\": {ex.Message}", "Error Importing Textures", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return imported;
            });

            if (cancelled2)
            {
                MessageBox.Show($"Imported {imported2} textures from folder: \"{fbd.SelectedPath}\".", "Import Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return imported2 == 0 ? FileCallbackResult.Unchanged : FileCallbackResult.Modified;
            }

            imported = imported2;
        }
        catch (TargetInvocationException)
        {
            MessageBox.Show("Import cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error importing textures from folder: \"{fbd.SelectedPath}\": {ex}", "Error Importing Textures", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        if (imported == 0)
        {
            MessageBox.Show($"No textures imported from folder: \"{fbd.SelectedPath}\".", "Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        MessageBox.Show($"Imported {imported} textures from folder: \"{fbd.SelectedPath}\".", "Textures Imported", MessageBoxButtons.OK, MessageBoxIcon.Information);
        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
