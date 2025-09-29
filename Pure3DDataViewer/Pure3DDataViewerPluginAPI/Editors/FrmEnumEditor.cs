namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmEnumEditor : Form
{
    public object? Value
    {
        get => CBValue.SelectedItem;
        set => CBValue.SelectedItem = value;
    }

    public FrmEnumEditor(Type propertyType, string propertyName, object? currentValue)
    {
        if (!propertyType.IsEnum)
            throw new ArgumentException("Property type must be an Enum.", nameof(propertyType));
        if (currentValue != null && currentValue.GetType() != propertyType)
            throw new ArgumentException("Current value must match Property type.", nameof(currentValue));

        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";

        CBValue.DataSource = Enum.GetValues(propertyType);
        CBValue.SelectedItem = currentValue;
    }

    private void FrmEnumEditor_Shown(object sender, EventArgs e)
    {
        CBValue.Focus();
    }
}
