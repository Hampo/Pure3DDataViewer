using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Events;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace FrontendTextBibleEditor.Editors;
public class FrontendTextBible : IChunkEditor<FrontendTextBibleChunk>
{
    public string Name => "Text Bible Editor";

    public event EventHandler<UpdatedChunkEventArgs>? UpdatedChunk;

    public UserControl GetEditor(FrontendTextBibleChunk chunk)
    {
        var control = new Controls.FrontendTextBibleEditor(chunk);
        control.Updated += (s, e) => UpdatedChunk?.Invoke(this, new(chunk));
        return control;
    }
}
