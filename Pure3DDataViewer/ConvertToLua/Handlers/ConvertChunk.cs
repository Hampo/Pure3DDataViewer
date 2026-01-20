using ConvertToLua.Forms;
using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

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
        using var frmViewLua = new FrmViewLua(chunk);
        frmViewLua.ShowDialog();

        return ChunkCallbackResult.Unchanged;
    }
}
