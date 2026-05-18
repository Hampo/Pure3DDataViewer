using Pure3DDataViewerPluginAPI.Utils;

namespace Pure3DDataViewerPluginAPI.Controls;
public partial class ColorPicker : UserControl
{
    public event EventHandler? ValueChanged;
    protected virtual void OnValueChanged() => ValueChanged?.Invoke(this, EventArgs.Empty);

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
            OnValueChanged();
        }
    }

    public ColorPicker()
    {
        InitializeComponent();
        NUDAlpha.ValueChanged += (s, e) => OnValueChanged();
    }

    private void PnlColour_Click(object sender, EventArgs e)
    {
        /*using var colourDialog = new ColorDialog()
        {
            AllowFullOpen = true,
            AnyColor = true,
            Color = PnlColour.BackColor,
            CustomColors = CustomColours,
            FullOpen = true,
            SolidColorOnly = true,
        };

        if (colourDialog.ShowDialog() == DialogResult.OK)
        {
            PnlColour.BackColor = colourDialog.Color;
            OnValueChanged();
        }

        CustomColours = colourDialog.CustomColors;*/

        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = Color.FromArgb((int)NUDAlpha.Value, PnlColour.BackColor),
            ShowAlphaChannel = true,
            Text = "Edit Value",
        };
        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        PnlColour.BackColor = Color.FromArgb(255, colorPicker.Color);
        NUDAlpha.Value = colorPicker.Color.A;
        OnValueChanged();
    }
}
