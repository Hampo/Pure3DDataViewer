using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocatorEditor.Editors;

public class Locator : IChunkEditor<LocatorChunk>
{
    public string Name => "Locator Editor";

    public EditorControl Editor => new Controls.LocatorEditor();
}
