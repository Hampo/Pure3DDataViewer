using TimeOfDayTint.Enums;
using TimeOfDayTint.Extensions;

namespace TimeOfDayTint.Forms;
public partial class FrmTintOptions : Form
{
    public Color CurrentTint => PnlCurrent.BackColor;

    public Color NewTint => PnlNew.BackColor;

    public float Blend => (float)NUDBlend.Value;

    public float Brightness => (float)NUDBrightness.Value;

    public FrmTintOptions()
    {
        InitializeComponent();
    }

    private void FrmTintOptions_Load(object sender, EventArgs e)
    {
        CBCurrentTimeOfDay.DataSource = Enum.GetValues(typeof(TimeOfDay));
        CBCurrentTimeOfDay.SelectedItem = TimeOfDay.Day;
        CBNewTimeOfDay.DataSource = Enum.GetValues(typeof(TimeOfDay));
        CBNewTimeOfDay.SelectedItem = TimeOfDay.Night;
    }

    private void CBCurrentTimeOfDay_SelectedValueChanged(object sender, EventArgs e)
    {
        if (CBCurrentTimeOfDay.SelectedValue is not TimeOfDay timeOfDay)
            return;

        PnlCurrent.BackColor = timeOfDay.GetTint();
    }

    private void CBCurrentUseCustom_CheckedChanged(object sender, EventArgs e)
    {
        var custom = CBCurrentUseCustom.Checked;
        CBCurrentTimeOfDay.Enabled = !custom;
        PnlCurrent.Enabled = custom;

        if (!custom && CBCurrentTimeOfDay.SelectedValue is TimeOfDay timeOfDay)
            PnlCurrent.BackColor = timeOfDay.GetTint();
    }

    private void PnlCurrent_Click(object sender, EventArgs e)
    {
        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = PnlCurrent.BackColor,
            ShowAlphaChannel = false,
            Text = $"Edit Current Tint",
        };
        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        PnlCurrent.BackColor = colorPicker.Color;
    }

    private void CBNewTimeOfDay_SelectedValueChanged(object sender, EventArgs e)
    {
        if (CBNewTimeOfDay.SelectedValue is not TimeOfDay timeOfDay)
            return;

        PnlNew.BackColor = timeOfDay.GetTint();
    }

    private void CBNewUseCustom_CheckedChanged(object sender, EventArgs e)
    {
        var custom = CBNewUseCustom.Checked;
        CBNewTimeOfDay.Enabled = !custom;
        PnlNew.Enabled = custom;

        if (!custom && CBNewTimeOfDay.SelectedValue is TimeOfDay timeOfDay)
            PnlNew.BackColor = timeOfDay.GetTint();
    }

    private void PnlNew_Click(object sender, EventArgs e)
    {
        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = PnlNew.BackColor,
            ShowAlphaChannel = false,
            Text = $"Edit New Tint",
        };
        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        PnlNew.BackColor = colorPicker.Color;
    }
}
