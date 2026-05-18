using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.UndoRedo;
using Pure3DDataViewerPluginAPI.UndoRedo.Commands;

namespace LocatorEditor.Editors.Controls.TypeData;

public partial class EventEditor : UserControl
{
    private readonly LocatorChunk _locatorChunk;
    private bool _updating = false;

    public EventEditor(LocatorChunk locatorChunk)
    {
        InitializeComponent();

        if (locatorChunk.TypeData is not LocatorChunk.EventLocatorData eventData)
            throw new NotSupportedException($"{typeof(EventEditor)} only supports Event (Type 0) locators.");

        _locatorChunk = locatorChunk;
        _updating = true;

        CBEvent.DataSource = Enum.GetValues(typeof(LocatorChunk.EventLocatorData.Events));
        CBEvent.SelectedItem = eventData.Event;

        CBParameter.Checked = eventData.HasParameter;
        NTBParameterUint.Value = eventData.Parameter;

        NTBParameterUint.Enabled = eventData.HasParameter;
        NTBParameterInt.Enabled = eventData.HasParameter;
        NTBParameterFloat.Enabled = eventData.HasParameter;
        CPParameter.Enabled = eventData.HasParameter;
        CBParameterValue.Enabled = eventData.HasParameter;

        _updating = false;
    }

    private void CBEvent_SelectedValueChanged(object sender, EventArgs e)
    {
        var newEvent = (LocatorChunk.EventLocatorData.Events)CBEvent.SelectedItem!;

        switch (newEvent)
        {
            case LocatorChunk.EventLocatorData.Events.FarPlane:
                LblNoParameter.Visible = false;
                CBParameter.Visible = true;
                NTBParameterUint.Visible = false;
                NTBParameterInt.Visible = false;
                NTBParameterFloat.Visible = true;
                CPParameter.Visible = false;
                CBParameterValue.Visible = false;
                break;
            case LocatorChunk.EventLocatorData.Events.CheckPoint:
                LblNoParameter.Visible = false;
                CBParameter.Visible = true;
                NTBParameterUint.Visible = false;
                NTBParameterInt.Visible = true;
                NTBParameterFloat.Visible = false;
                CPParameter.Visible = false;
                CBParameterValue.Visible = false;
                break;
            case LocatorChunk.EventLocatorData.Events.LightChange:
                LblNoParameter.Visible = false;
                CBParameter.Visible = true;
                NTBParameterUint.Visible = false;
                NTBParameterInt.Visible = false;
                NTBParameterFloat.Visible = false;
                CPParameter.Visible = true;
                CBParameterValue.Visible = false;
                break;
            case LocatorChunk.EventLocatorData.Events.GooDamage:
                LblNoParameter.Visible = false;
                CBParameter.Visible = true;
                NTBParameterUint.Visible = true;
                NTBParameterInt.Visible = false;
                NTBParameterFloat.Visible = false;
                CPParameter.Visible = false;
                CBParameterValue.Visible = false;
                break;
            case LocatorChunk.EventLocatorData.Events.Trap:
                LblNoParameter.Visible = false;
                CBParameter.Visible = true;
                NTBParameterUint.Visible = false;
                NTBParameterInt.Visible = false;
                NTBParameterFloat.Visible = false;
                CPParameter.Visible = false;
                CBParameterValue.Visible = true;
                break;
            default:
                LblNoParameter.Visible = true;
                CBParameter.Visible = false;
                NTBParameterUint.Visible = false;
                NTBParameterInt.Visible = false;
                NTBParameterFloat.Visible = false;
                CPParameter.Visible = false;
                CBParameterValue.Visible = false;
                break;
        }

        if (_updating)
            return;

        var beforeChunk = _locatorChunk.Clone();
        ((LocatorChunk.EventLocatorData)_locatorChunk.TypeData).Event = newEvent;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Event", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }

    private void CBParameter_CheckedChanged(object sender, EventArgs e)
    {
        if (_updating)
            return;

        var eventData = (LocatorChunk.EventLocatorData)_locatorChunk.TypeData;
        if (eventData.HasParameter == CBParameter.Checked)
            return;

        var beforeChunk = _locatorChunk.Clone();
        eventData.HasParameter = CBParameter.Checked;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Toggle Locator Parameter", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }

    private void UpdateParameterValue()
    {
        var value = NTBParameterUint.Value;
        if (value is not uint uintValue)
            return;

        var eventData = (LocatorChunk.EventLocatorData)_locatorChunk.TypeData;
        if (_updating || eventData.Parameter == uintValue)
            return;

        var beforeChunk = _locatorChunk.Clone();
        eventData.Parameter = uintValue;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Parameter", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }

    private void NTBParameterUint_TextChanged(object sender, EventArgs e)
    {
        var value = NTBParameterUint.Value;
        if (value is not uint uintValue)
            return;

        var intValue = (int)uintValue;

        NTBParameterInt.Value = intValue;
        NTBParameterFloat.Value = BitConverter.UInt32BitsToSingle(uintValue);
        CPParameter.Value = Color.FromArgb(intValue);
        CBParameterValue.Checked = uintValue != 0;
    }

    private void NTBParameterFloat_TextChanged(object sender, EventArgs e)
    {
        if (_updating)
            return;

        var value = (float?)NTBParameterFloat.Value ?? 0f;
        NTBParameterUint.Value = BitConverter.SingleToUInt32Bits(value);
    }

    private void NTBParameterInt_TextChanged(object sender, EventArgs e)
    {
        if (_updating)
            return;

        var value = (int?)NTBParameterInt.Value ?? 0;
        NTBParameterUint.Value = (uint)value;
    }

    private void CBParameterValue_CheckedChanged(object sender, EventArgs e)
    {
        if (_updating)
            return;

        NTBParameterUint.Value = CBParameterValue.Checked ? 1u : 0u;

        if (CBParameterValue.Focused)
            UpdateParameterValue();
    }

    private void CPParameter_ValueChanged(object sender, EventArgs e)
    {
        if (_updating)
            return;

        NTBParameterUint.Value = (uint)CPParameter.Value.ToArgb();

        if (CPParameter.Focused)
            UpdateParameterValue();
    }

    private void NTBParameter_Leave(object sender, EventArgs e) => UpdateParameterValue();
}
