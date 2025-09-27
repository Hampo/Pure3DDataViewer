namespace Pure3DDataViewer.Editors;
public partial class FrmStringEditor : Form
{
    public string Value
    {
        get => TxtValue.Text;
        set => TxtValue.Text = value;
    }

    public FrmStringEditor(string propertyName, string? currentValue, int maxLength = -1)
    {
        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        TxtValue.Text = currentValue ?? string.Empty;

        if (maxLength > 0 )
            TxtValue.MaxLength = maxLength;
    }

    private void FrmStringEditor_Shown(object sender, EventArgs e)
    {
        TxtValue.Focus();
    }
}
