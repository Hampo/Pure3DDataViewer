namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmBooleanEditor : Form
{
    public bool Value
    {
        get => CBValue.Checked;
        set => CBValue.Checked = value;
    }

    public FrmBooleanEditor(string propertyName, bool? currentValue)
    {
        ArgumentNullException.ThrowIfNull(currentValue, nameof(currentValue));

        InitializeComponent();
        CBValue.Text = $"{propertyName}";
        CBValue.Checked = currentValue.Value;
    }

    private void FrmBooleanEditor_Shown(object sender, EventArgs e)
    {
        CBValue.Focus();
    }
}
