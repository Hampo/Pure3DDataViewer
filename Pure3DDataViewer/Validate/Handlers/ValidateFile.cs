using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Text;

namespace Validate.Handlers;
public class ValidateFile : IFileHandler
{
    public string Name => "Validate File";

    public Image? Image => ValidatePlugin.ValidateImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        var errors = new StringBuilder();

        foreach (var chunk in p3dFile.Chunks)
            foreach (var error in chunk.ValidateChunks())
                errors.AppendLine($"Error in \"{error.Chunk}\": {error.Message}");

        if (errors.Length == 0)
        {
            MessageBox.Show("No errors found in file.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        MessageBox.Show($"The following errors were found in the file:\n\n{errors}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
