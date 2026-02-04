namespace Pure3DDataViewer;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();

        Application.Idle += Application_Idle;

        Application.Run(new FrmMain());
    }

    private static void Application_Idle(object? sender, EventArgs e)
    {
        var dark = Settings.DarkMode;
        var large = Settings.LargeFont;
        foreach (Form form in Application.OpenForms)
        {
            if (form.Tag as string == "Themed")
                continue;

            Theming.ApplyTheme(form, dark ? Theming.ThemeMode.Dark : Theming.ThemeMode.Light, large ? Theming.FontMode.Large : Theming.FontMode.Normal);

            form.Tag = "Themed";
        }
    }
}