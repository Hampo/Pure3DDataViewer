namespace Pure3DDataViewer.Controls;
public partial class ColorPicker : UserControl
{
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
            CustomColors = Settings.CustomColours,
            FullOpen = true,
            SolidColorOnly = true,
        };

        if (colourDialog.ShowDialog() == DialogResult.OK)
            PnlColour.BackColor = colourDialog.Color;

        Settings.CustomColours = colourDialog.CustomColors;
    }
}
