using ConvertToLua.Helpers;
using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Text;

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
        var sb = new StringBuilder();

        sb.AppendLine("local P3DFile = P3D.P3DFile()");
        sb.AppendLine();

        foreach (var chunk in p3dFile.Chunks)
        {
            try
            {
                var constructor = ChunkMap.GetLuaConstructor(chunk);
                sb.AppendLine($"P3DFile:AddChunk({constructor})");
            }
            catch (Exception ex)
            {
                sb.AppendLine($"-- Error in {chunk}: {ex.Message}");
            }
        }

        sb.AppendLine();
        sb.AppendLine("P3DFile:Output()");

        Clipboard.SetText(sb.ToString());

        return FileCallbackResult.Unchanged;
    }

    public bool IsFileSupported(P3DFile p3dFile) => p3dFile.Chunks.Count > 0;
}
