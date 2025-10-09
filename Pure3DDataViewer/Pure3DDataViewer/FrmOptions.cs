using NetP3DLib.P3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Pure3DDataViewer;
public partial class FrmOptions : Form
{
    public FrmOptions()
    {
        InitializeComponent();
    }

    private void FrmOptions_Load(object sender, EventArgs e)
    {
        LVChunkColours.BeginUpdate();
        foreach (var type in ChunkLoader.ChunkTypes.Values.Select(x => x.Item1))
        {
            var lvi = LVChunkColours.Items.Add($"{type}");
            lvi.Tag = type;
            var (backColour, foreColour) = Settings.GetChunkColour(type);
            lvi.SubItems.Add(backColour.IsEmpty ? "<Default>" : $"#{backColour.R:X2}{backColour.G:X2}{backColour.B:X2}");
            lvi.SubItems.Add(foreColour.IsEmpty ? "<Default>" : $"#{foreColour.R:X2}{foreColour.G:X2}{foreColour.B:X2}");
            lvi.BackColor = backColour;
            lvi.ForeColor = foreColour;
        }

        foreach (ColumnHeader column in LVChunkColours.Columns)
            column.Width = -2;
        LVChunkColours.EndUpdate();
    }

    private void LVChunkColours_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        var info = LVChunkColours.HitTest(e.Location);
        var item = info.Item;
        var subItem = info.SubItem;

        if (item == null || subItem == null || item.Tag is not Type type)
            return;

        var columnIndex = item.SubItems.IndexOf(subItem);
        switch (columnIndex)
        {
            case 1:
                TSMISetBackColour.PerformClick();
                break;
            case 2:
                TSMISetForeColour.PerformClick();
                break;
        }
    }

    private void CMSChunkColours_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            e.Cancel = true;
    }

    private void TSMISetBackColour_Click(object sender, EventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            return;

        var selectedItem = LVChunkColours.SelectedItems[0];
        if (selectedItem.Tag is not Type selectedType)
            return;

        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = selectedItem.BackColor,
            ShowAlphaChannel = true,
            Text = $"Edit {selectedType.Name} Back Colour",
        };
        
        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        var backColour = colorPicker.Color;
        selectedItem.SubItems[1].Text = backColour.IsEmpty ? "<Default>" : $"#{backColour.R:X2}{backColour.G:X2}{backColour.B:X2}";
        selectedItem.BackColor = backColour;
        Settings.SetChunkBackColour(selectedType, backColour);
    }

    private void TSMISetForeColour_Click(object sender, EventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            return;

        var selectedItem = LVChunkColours.SelectedItems[0];
        if (selectedItem.Tag is not Type selectedType)
            return;

        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = selectedItem.ForeColor,
            ShowAlphaChannel = true,
            Text = $"Edit {selectedType.Name} Fore Colour",
        };

        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        var foreColour = colorPicker.Color;
        selectedItem.SubItems[2].Text = foreColour.IsEmpty ? "<Default>" : $"#{foreColour.R:X2}{foreColour.G:X2}{foreColour.B:X2}";
        selectedItem.ForeColor = foreColour;
        Settings.SetChunkForeColour(selectedType, foreColour);
    }

    private void TSMIResetColours_Click(object sender, EventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            return;

        var selectedItem = LVChunkColours.SelectedItems[0];
        if (selectedItem.Tag is not Type selectedType)
            return;

        LVChunkColours.BeginUpdate();

        Settings.ResetChunkColour(selectedType);
        selectedItem.BackColor = Color.Empty;
        selectedItem.ForeColor = Color.Empty;
        selectedItem.SubItems[1].Text = "<Default>";
        selectedItem.SubItems[2].Text = "<Default>";

        LVChunkColours.EndUpdate();
    }
}
