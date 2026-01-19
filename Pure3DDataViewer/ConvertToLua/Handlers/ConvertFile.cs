using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace ConvertToLua.Handlers;
public class ConvertFile : IFileHandler
{
    public string Name => "Convert File to Lua";

    public Image? Image => ConvertToLuaPlugin.ConvertImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile) => p3dFile.Chunks.Count > 0;
}
