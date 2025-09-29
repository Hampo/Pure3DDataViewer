namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmKnownStringEditor : Form
{
    public string Value
    {
        get => CBValue.Text;
        set => CBValue.Text = value;
    }

    public FrmKnownStringEditor(string propertyName, string? currentValue, string[] knownValues, int maxLength = -1)
    {
        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        var autocomplete = new AutoCompleteStringCollection();
        autocomplete.AddRange(knownValues);
        CBValue.AutoCompleteCustomSource = autocomplete;
        CBValue.Items.AddRange(knownValues);
        CBValue.Text = currentValue ?? string.Empty;

        if (maxLength > 0)
            CBValue.MaxLength = maxLength;
    }

    private void FrmStringEditor_Shown(object sender, EventArgs e)
    {
        CBValue.Focus();
    }
}
