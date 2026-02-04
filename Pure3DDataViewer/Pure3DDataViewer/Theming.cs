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

            default:
                control.BackColor = backColor;
                control.ForeColor = foreColor;
                break;
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
