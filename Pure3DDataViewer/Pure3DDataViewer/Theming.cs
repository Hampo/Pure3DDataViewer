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

            case TextBoxBase textBoxBase:
                textBoxBase.BackColor = themeMode == ThemeMode.Dark ? Color.FromArgb(45, 45, 45) : backColor;
                textBoxBase.ForeColor = foreColor;
                break;

            default:
                control.BackColor = backColor;
                control.ForeColor = foreColor;
                break;
        }

        // Loop through children
        foreach (Control child in control.Controls)
        {
            ApplyTheme(child, themeMode, fontMode);
        }

        // For menu strip and status strip
        if (control is MenuStrip menu)
        {
            foreach (ToolStripMenuItem item in menu.Items)
            {
                ApplyToolStripTheme(item, themeMode, fontMode);
            }
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
}
