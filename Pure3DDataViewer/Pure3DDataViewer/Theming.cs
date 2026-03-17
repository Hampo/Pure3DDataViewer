using Pure3DDataViewerPluginAPI.Controls;
using System.Runtime.InteropServices;

namespace Pure3DDataViewer;
public static class Theming
{
    public enum ThemeMode
    {
        Light,
        Dark
    }

    public enum FontMode
    {
        Normal,
        Large
    }

    public static readonly Font NormalFont = new("Segoe UI", 9F);
    public static readonly Font LargeFont = new("Segoe UI", 13F);

    public static void ApplyTheme(Control control, ThemeMode themeMode, FontMode fontMode)
    {
        control.SuspendLayout();
        Color backColor = themeMode == ThemeMode.Dark ? Color.FromArgb(30, 30, 30) : Color.White;
        Color foreColor = themeMode == ThemeMode.Dark ? Color.LightGray : Color.Black;

        control.Font = fontMode == FontMode.Normal ? NormalFont : LargeFont;

        switch (control)
        {
            case Form form:
                form.BackColor = backColor;
                form.ForeColor = foreColor;
                UseImmersiveDarkMode(form.Handle, themeMode == ThemeMode.Dark);

                var oldBorderStyle = form.FormBorderStyle;
                form.FormBorderStyle = oldBorderStyle == FormBorderStyle.Fixed3D ? FormBorderStyle.FixedSingle : FormBorderStyle.Fixed3D;
                form.FormBorderStyle = oldBorderStyle;
                break;

            case ExplorerThemedTreeView treeView:
                treeView.DarkMode = themeMode == ThemeMode.Dark;
                break;

            case TextBoxBase textBoxBase:
                textBoxBase.BackColor = themeMode == ThemeMode.Dark ? Color.FromArgb(45, 45, 45) : backColor;
                textBoxBase.ForeColor = foreColor;
                break;

            case DataGridView dataGridView:
                ApplyDataGridViewTheme(dataGridView, themeMode, fontMode);
                break;

            default:
                control.BackColor = backColor;
                control.ForeColor = foreColor;
                break;
        }

        if (control is NumericTextBox numericTextBox)
        {
            numericTextBox.ValidColor = numericTextBox.BackColor;
            numericTextBox.InvalidColor = themeMode == ThemeMode.Dark ? Color.FromArgb(99, 34, 34) : Color.Pink;
        }

        foreach (Control child in control.Controls)
            ApplyTheme(child, themeMode, fontMode);

        if (control is MenuStrip menu)
        {
            menu.Renderer = themeMode == ThemeMode.Dark ? new DarkThemeRenderer() : new ToolStripProfessionalRenderer();
        }
        else if (control is StatusStrip statusStrip)
        {
            foreach (ToolStripItem item in statusStrip.Items)
            {
                item.BackColor = backColor;
                item.ForeColor = foreColor;
            }
        }
        control.ResumeLayout(true);
    }

    private static void ApplyToolStripTheme(ToolStripMenuItem item, ThemeMode themeMode, FontMode fontMode)
    {
        Color backColor = themeMode == ThemeMode.Dark ? Color.FromArgb(30, 30, 30) : Color.White;
        Color foreColor = themeMode == ThemeMode.Dark ? Color.LightGray : Color.Black;

        item.BackColor = backColor;
        item.ForeColor = foreColor;
        item.Font = fontMode == FontMode.Normal ? NormalFont : LargeFont;

        foreach (ToolStripItem subItem in item.DropDownItems)
        {
            if (subItem is ToolStripMenuItem subMenuItem)
            {
                ApplyToolStripTheme(subMenuItem, themeMode, fontMode);
            }
            else
            {
                subItem.BackColor = backColor;
                subItem.ForeColor = foreColor;
            }
        }
    }

    private static void ApplyDataGridViewTheme(DataGridView grid, ThemeMode themeMode, FontMode fontMode)
    {
        bool dark = themeMode == ThemeMode.Dark;

        Color baseBack = dark ? Color.FromArgb(30, 30, 30) : Color.White;
        Color altBack = dark ? Color.FromArgb(35, 35, 35) : Color.FromArgb(245, 245, 245);
        Color fore = dark ? Color.LightGray : Color.Black;

        Color readonlyBack = dark ? Color.FromArgb(55, 55, 55) : Color.Silver;
        Color readonlyFore = dark ? Color.FromArgb(160, 160, 160) : Color.Black;

        grid.EnableHeadersVisualStyles = false;
        grid.BackgroundColor = baseBack;
        grid.GridColor = dark ? Color.FromArgb(60, 60, 60) : SystemColors.ControlDark;

        grid.DefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = baseBack,
            ForeColor = fore,
            SelectionBackColor = dark ? Color.FromArgb(55, 90, 140) : SystemColors.Highlight,
            SelectionForeColor = Color.White,
            Font = fontMode == FontMode.Normal ? NormalFont : LargeFont
        };

        grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = altBack,
            ForeColor = fore,
            SelectionBackColor = dark ? Color.FromArgb(65, 100, 155) : SystemColors.Highlight,
            SelectionForeColor = Color.White
        };

        grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
        {
            BackColor = dark ? Color.FromArgb(45, 45, 45) : SystemColors.Control,
            ForeColor = fore,
            Font = fontMode == FontMode.Normal ? NormalFont : LargeFont
        };

        grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = grid.ColumnHeadersDefaultCellStyle.BackColor;
        grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = grid.ColumnHeadersDefaultCellStyle.ForeColor;

        grid.RowPrePaint -= DataGridView_RowPrePaint;
        grid.RowPrePaint += DataGridView_RowPrePaint;

        grid.Tag = new DataGridViewThemeState
        {
            ReadonlyBackColor = readonlyBack,
            ReadonlyForeColor = readonlyFore
        };
    }

    private static void DataGridView_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        var grid = (DataGridView)sender!;
        var row = grid.Rows[e.RowIndex];
        var theme = (DataGridViewThemeState)grid.Tag!;

        bool isReadonly = row.ReadOnly;

        if (!isReadonly)
            return;

        row.DefaultCellStyle.BackColor = theme.ReadonlyBackColor;
        row.DefaultCellStyle.ForeColor = theme.ReadonlyForeColor;

        bool selected = row.Selected || grid.CurrentCell?.RowIndex == e.RowIndex;
        row.DefaultCellStyle.SelectionBackColor = selected ? ControlPaint.Light(theme.ReadonlyBackColor, 0.7f) : theme.ReadonlyBackColor;
        row.DefaultCellStyle.SelectionForeColor = theme.ReadonlyForeColor;
    }

    private class DataGridViewThemeState
    {
        public Color ReadonlyBackColor { get; set; }
        public Color ReadonlyForeColor { get; set; }
    }

    [DllImport("dwmapi.dll")]
    private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

    private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
    private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

    private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
    {
        if (IsWindows10OrGreater(17763))
        {
            var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
            if (IsWindows10OrGreater(18985))
            {
                attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
            }

            int useImmersiveDarkMode = enabled ? 1 : 0;
            return DwmSetWindowAttribute(handle, (int)attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
        }

        return false;
    }

    private static bool IsWindows10OrGreater(int build = -1) => Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;

    private class DarkColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => Color.FromArgb(50, 50, 50);
        public override Color MenuItemSelectedGradientBegin => Color.FromArgb(50, 50, 50);
        public override Color MenuItemSelectedGradientEnd => Color.FromArgb(50, 50, 50);
        public override Color MenuItemBorder => Color.FromArgb(100, 100, 100);

        // This fixes your light separator
        public override Color SeparatorDark => Color.FromArgb(80, 80, 80);
        public override Color SeparatorLight => Color.Transparent; // Hide the 3D 'shadow'

        public override Color ToolStripDropDownBackground => Color.FromArgb(30, 30, 30);
        public override Color ImageMarginGradientBegin => Color.FromArgb(30, 30, 30);
        public override Color ImageMarginGradientMiddle => Color.FromArgb(30, 30, 30);
        public override Color ImageMarginGradientEnd => Color.FromArgb(30, 30, 30);
    }

    private class DarkThemeRenderer : ToolStripProfessionalRenderer
    {
        public DarkThemeRenderer() : base(new DarkColorTable()) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            // Force the text color to LightGray for all items
            e.TextColor = Color.LightGray;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            // Force sub-menu arrows to be LightGray instead of black
            e.ArrowColor = Color.LightGray;
            base.OnRenderArrow(e);
        }
    }
}
