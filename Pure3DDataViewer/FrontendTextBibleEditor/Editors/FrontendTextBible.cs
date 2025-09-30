using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace FrontendTextBibleEditor.Editors;
public class FrontendTextBible : IChunkEditor<FrontendTextBibleChunk>
{
    public string Name => "Text Bible Editor";

    public EditorControl Editor => new Controls.FrontendTextBibleEditor();
}
