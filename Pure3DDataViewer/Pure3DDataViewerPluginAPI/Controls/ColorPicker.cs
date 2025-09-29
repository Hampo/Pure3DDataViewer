namespace Pure3DDataViewerPluginAPI.Controls;
public partial class ColorPicker : UserControl
{
    public static int[]? CustomColours
    {
        get
        {
            var values = RegistryUtils.GetStringArray("CustomColours");
            if (values == null)
                return null;

            var registryColours = new List<int>(16);
            foreach (var value in values)
            {
                if (!int.TryParse(value, out int colour))
                    continue;
                registryColours.Add(colour);
                if (registryColours.Count >= 16)
                    break;
            }

            return [.. registryColours];
        }
        set
        {
            RegistryUtils.SetStringArray("CustomColours", value?.Select(x => x.ToString()).ToArray());
        }
    }

    public Color Value
    {
        get => Color.FromArgb((int)NUDAlpha.Value, PnlColour.BackColor);
        set
        {
            PnlColour.BackColor = Color.FromArgb(255, value);
            NUDAlpha.Value = value.A;
        }
    }

    public ColorPicker()
    {
        InitializeComponent();
    }

    private void PnlColour_Click(object sender, EventArgs e)
    {
        using var colourDialog = new ColorDialog()
        {
            AllowFullOpen = true,
            AnyColor = true,
            Color = PnlColour.BackColor,
            CustomColors = CustomColours,
            FullOpen = true,
            SolidColorOnly = true,
        };

        if (colourDialog.ShowDialog() == DialogResult.OK)
            PnlColour.BackColor = colourDialog.Color;

        CustomColours = colourDialog.CustomColors;
    }
}
