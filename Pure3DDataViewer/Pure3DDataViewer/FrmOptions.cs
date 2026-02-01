using NetP3DLib.P3D;

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

        var (errorBackColour, errorForeColour) = Settings.GetErrorChunkColour();
        var errorLVI = AddChunkColour("Errored Chunk", errorBackColour, errorForeColour);

        AddChunkColourType(typeof(UnknownChunk));
        foreach (var type in ChunkLoader.ChunkTypes.Values.Select(x => x.Item1))
            AddChunkColourType(type);

        foreach (ColumnHeader column in LVChunkColours.Columns)
            column.Width = -2;
        LVChunkColours.EndUpdate();

        CBDarkMode.Checked = Settings.DarkMode;
        CBLargeFont.Checked = Settings.LargeFont;
    }

    private void AddChunkColourType(Type type)
    {
        var (backColour, foreColour) = Settings.GetChunkColour(type);
        var lvi = AddChunkColour($"{type}", backColour, foreColour);
        lvi.Tag = type;
    }

    private ListViewItem AddChunkColour(string name, Color backColour, Color foreColour)
    {
        var lvi = LVChunkColours.Items.Add(name);
        lvi.SubItems.Add(backColour.IsEmpty ? "<Default>" : $"#{backColour.R:X2}{backColour.G:X2}{backColour.B:X2}");
        lvi.SubItems.Add(foreColour.IsEmpty ? "<Default>" : $"#{foreColour.R:X2}{foreColour.G:X2}{foreColour.B:X2}");
        lvi.BackColor = backColour;
        lvi.ForeColor = foreColour;
        return lvi;
    }

    private void LVChunkColours_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        var info = LVChunkColours.HitTest(e.Location);
        var item = info.Item;
        var subItem = info.SubItem;

        if (item == null || subItem == null)
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

        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = selectedItem.BackColor,
            ShowAlphaChannel = true,
            Text = $"Edit {selectedItem.Text} Back Colour",
        };

        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        var backColour = colorPicker.Color;
        selectedItem.SubItems[1].Text = backColour.IsEmpty ? "<Default>" : $"#{backColour.R:X2}{backColour.G:X2}{backColour.B:X2}";
        selectedItem.BackColor = backColour;

        if (selectedItem.Tag is not Type selectedType)
            Settings.SetErrorChunkBackColour(backColour);
        else
            Settings.SetChunkBackColour(selectedType, backColour);
    }

    private void TSMISetForeColour_Click(object sender, EventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            return;

        var selectedItem = LVChunkColours.SelectedItems[0];

        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = selectedItem.ForeColor,
            ShowAlphaChannel = true,
            Text = $"Edit {selectedItem.Text} Fore Colour",
        };

        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        var foreColour = colorPicker.Color;
        selectedItem.SubItems[2].Text = foreColour.IsEmpty ? "<Default>" : $"#{foreColour.R:X2}{foreColour.G:X2}{foreColour.B:X2}";
        selectedItem.ForeColor = foreColour;

        if (selectedItem.Tag is not Type selectedType)
            Settings.SetErrorChunkForeColour(foreColour);
        else
            Settings.SetChunkForeColour(selectedType, foreColour);
    }

    private void TSMIResetColours_Click(object sender, EventArgs e)
    {
        if (LVChunkColours.SelectedItems.Count == 0)
            return;

        var selectedItem = LVChunkColours.SelectedItems[0];

        LVChunkColours.BeginUpdate();

        (Color BackColour, Color ForeColour) colours;
        if (selectedItem.Tag is not Type selectedType)
        {
            Settings.ResetErrorChunkColour();
            colours = Settings.GetErrorChunkColour();
        }
        else
        {
            Settings.ResetChunkColour(selectedType);
            colours = Settings.GetChunkColour(selectedType);
        }

        selectedItem.BackColor = colours.BackColour;
        selectedItem.ForeColor = colours.ForeColour;
        selectedItem.SubItems[1].Text = colours.BackColour.IsEmpty ? "<Default>" : $"#{colours.BackColour.R:X2}{colours.BackColour.G:X2}{colours.BackColour.B:X2}";
        selectedItem.SubItems[2].Text = colours.ForeColour.IsEmpty ? "<Default>" : $"#{colours.ForeColour.R:X2}{colours.ForeColour.G:X2}{colours.ForeColour.B:X2}";

        LVChunkColours.EndUpdate();
    }

    private void CBTheming_CheckedChanged(object sender, EventArgs e)
    {
        Settings.DarkMode = CBDarkMode.Checked;
        Settings.LargeFont = CBLargeFont.Checked;

        foreach (Form form in Application.OpenForms)
            Theming.ApplyTheme(form, CBDarkMode.Checked ? Theming.ThemeMode.Dark : Theming.ThemeMode.Light, CBLargeFont.Checked ? Theming.FontMode.Large : Theming.FontMode.Normal);
    }
}
