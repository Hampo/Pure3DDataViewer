using Pure3DDataViewerPluginAPI.Controls;

namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmNumericEditor : Form
{
    public object? Value
    {
        get => TxtValue.Value;
        set => TxtValue.Value = value;
    }

    public FrmNumericEditor(string propertyName, object? currentValue)
    {
        ArgumentNullException.ThrowIfNull(currentValue);

        NumericTextBox.NumericTypes? numericType = NumericTextBox.GetNumericType(currentValue.GetType());
        if (!numericType.HasValue)
            throw new NotSupportedException($"Type {currentValue.GetType().Name} of {nameof(currentValue)} is invalid.");

        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        TxtValue.NumericType = numericType.Value;
        TxtValue.Value = currentValue;
    }

    private void FrmNumericEditor_Shown(object sender, EventArgs e)
    {
        TxtValue.Focus();
    }
}
