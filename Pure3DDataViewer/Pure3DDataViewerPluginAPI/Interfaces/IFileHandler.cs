using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;

namespace Pure3DDataViewerPluginAPI.Interfaces;

public interface IFileHandler
{
    public string Name { get; }
    public Image? Image { get; }
    public IList<(string Name, bool Value)>? GetSettings();
    public void SetSetting(string name, bool value);
    public FileCallbackResult Handle(P3DFile p3dFile);
    public bool IsFileSupported(P3DFile p3dFile);
}