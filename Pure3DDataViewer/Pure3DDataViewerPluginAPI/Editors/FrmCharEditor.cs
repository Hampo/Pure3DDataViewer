namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmCharEditor : Form
{
    public char? Value
    {
        get => TxtValue.Value;
        set => TxtValue.Value = value;
    }

    public FrmCharEditor(string propertyName, char? currentValue)
    {
        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        TxtValue.Text = currentValue?.ToString() ?? string.Empty;
        TxtValue.SelectionStart = 1;
    }

    private void FrmStringEditor_Shown(object sender, EventArgs e) => TxtValue.Focus();

    private void TxtValue_TextChanged(object sender, EventArgs e) => BtnOK.Enabled = !string.IsNullOrEmpty(TxtValue.Text);
}
