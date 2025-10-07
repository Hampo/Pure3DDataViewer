using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Text;

namespace Validate.Handlers;
public class ValidateFile : IFileHandler
{
    public string Name => "Validate";

    public Image? Image => ValidatePlugin.ValidateImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        var errors = new StringBuilder();

        foreach (var chunk in p3dFile.Chunks)
        {
            try
            {
                chunk.Validate();
            }
            catch (Exception ex)
            {
                errors.AppendLine($"Error in \"{chunk}\": {ex.Message}");
            }
        }

        if (errors.Length == 0)
        {
            MessageBox.Show("No errors found in file.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        MessageBox.Show($"The following errors were found in the file:\n\n{errors}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return FileCallbackResult.Unchanged;
    }
}
