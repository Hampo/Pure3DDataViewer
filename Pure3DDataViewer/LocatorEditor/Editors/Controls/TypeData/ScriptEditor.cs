using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.UndoRedo;
using Pure3DDataViewerPluginAPI.UndoRedo.Commands;

namespace LocatorEditor.Editors.Controls.TypeData;

public partial class ScriptEditor : UserControl
{
    private readonly LocatorChunk _locatorChunk;
    private bool _updating = false;

    public ScriptEditor(LocatorChunk locatorChunk)
    {
        InitializeComponent();

        if (locatorChunk.TypeData is not LocatorChunk.ScriptLocatorData eventData)
            throw new NotSupportedException($"{typeof(ScriptEditor)} only supports Script (Type 1) locators.");

        _locatorChunk = locatorChunk;
        _updating = true;

        TxtKey.Text = eventData.Key;

        _updating = false;
    }

    private void TxtKey_TextChanged(object sender, EventArgs e)
    {
        var eventData = (LocatorChunk.ScriptLocatorData)_locatorChunk.TypeData;
        if (_updating || eventData.Key == TxtKey.Text)
            return;

        var beforeChunk = _locatorChunk.Clone();
        eventData.Key = TxtKey.Text;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Key", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }
}
