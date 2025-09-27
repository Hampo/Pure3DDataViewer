namespace Pure3DDataViewer.Editors;
public partial class FrmEnumFlagsEditor : Form
{
    private readonly Type _type;

    public object? Value
    {
        get
        {
            long result = 0;
            foreach (var item in CLBValues.CheckedItems)
                result |= Convert.ToInt64(item);

            return Enum.ToObject(_type, result);
        }
        set
        {
            if (value == null || value.GetType() != _type)
                throw new ArgumentException("Value must be of the correct enum type.", nameof(value));

            long longValue = Convert.ToInt64(value);

            for (int i = 0; i < CLBValues.Items.Count; i++)
            {
                var item = CLBValues.Items[i];
                long itemValue = Convert.ToInt64(item);
                bool isSet = (longValue & itemValue) == itemValue;
                CLBValues.SetItemChecked(i, isSet);
            }
        }
    }

    public FrmEnumFlagsEditor(Type propertyType, string propertyName, object? currentValue)
    {
        ArgumentNullException.ThrowIfNull(currentValue, nameof(currentValue));

        if (!propertyType.HasFlagsAttribute())
            throw new ArgumentException("Property type must be a Flags Enum.", nameof(propertyType));
        if (currentValue != null && currentValue.GetType() != propertyType)
            throw new ArgumentException("Current value must match Property type.", nameof(currentValue));

        _type = propertyType;
        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";

        var currentValue64 = Convert.ToInt64(currentValue);
        foreach (var value in Enum.GetValues(propertyType))
        {
            var value64 = Convert.ToInt64(value);
            if (value64 == 0)
                continue;

            CLBValues.Items.Add(value, (currentValue64 & value64) == value64);
        }
    }

    private void FrmEnumFlagsEditor_Shown(object sender, EventArgs e)
    {
        CLBValues.Focus();
    }
}
