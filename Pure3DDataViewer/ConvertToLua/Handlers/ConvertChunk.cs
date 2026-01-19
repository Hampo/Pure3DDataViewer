using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;
using NetP3DLib.P3D;
using ConvertToLua.Helpers;

namespace ConvertToLua.Handlers;
public class ConvertChunk : IChunkHandler
{
    public string Name => "Convert Chunk to Lua";

    public Type? ChunkType => null;

    public Image? Image => ConvertToLuaPlugin.ConvertImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public ChunkCallbackResult Handle(Chunk chunk)
    {
        try
        {
            var constructor = ChunkMap.GetLuaConstructor(chunk);
            Clipboard.SetText(constructor);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to convert chunk: {ex.Message}", "Error converting chunk", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }
}
