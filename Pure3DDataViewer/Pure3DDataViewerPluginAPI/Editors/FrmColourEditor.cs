namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmColourEditor : Form
{
    public Color Value
    {
        get => CPValue.Value;
        set => CPValue.Value = value;
    }

    public FrmColourEditor(string propertyName, Color? currentValue)
    {
        ArgumentNullException.ThrowIfNull(currentValue, nameof(currentValue));

        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        CPValue.Value = currentValue.Value;
    }

    private void FrmColourEditor_Shown(object sender, EventArgs e)
    {
        CPValue.Focus();
    }
}
