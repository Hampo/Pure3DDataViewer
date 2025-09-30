namespace Pure3DDataViewerPluginAPI;
public static class RegistryUtils
{
    private const string RegistrySettings = @"Software\Pure3DDataViewer";
    public static readonly Microsoft.Win32.RegistryKey RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistrySettings, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

    public static string[]? GetStringArray(string key, string[]? defaultValue = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        string[] names = RegistryKey.GetValueNames();

        if (Array.IndexOf(names, key) < 0)
            return defaultValue;

        if (RegistryKey.GetValueKind(key) != Microsoft.Win32.RegistryValueKind.MultiString)
            return defaultValue;

        return (string[])RegistryKey.GetValue(key)!;
    }

    public static void SetStringArray(string key, string[]? value)
    {
        if (RegistryKey == null)
            return;

        if (value == null)
            RegistryKey.DeleteValue(key);
        else
            RegistryKey.SetValue(key, value, Microsoft.Win32.RegistryValueKind.MultiString);
    }

    public static string? GetString(string key, string? defaultValue = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        string[] names = RegistryKey.GetValueNames();

        if (Array.IndexOf(names, key) < 0)
            return defaultValue;

        if (RegistryKey.GetValueKind(key) != Microsoft.Win32.RegistryValueKind.String)
            return defaultValue;

        return (string)RegistryKey.GetValue(key)!;
    }

    public static void SetString(string key, string? value)
    {
        if (RegistryKey == null)
            return;

        if (value == null)
            RegistryKey.DeleteValue(key);
        else
            RegistryKey.SetValue(key, value, Microsoft.Win32.RegistryValueKind.String);
    }

    public static int? GetInt32(string key, int? defaultValue = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        string[] names = RegistryKey.GetValueNames();

        if (Array.IndexOf(names, key) < 0)
            return defaultValue;

        if (RegistryKey.GetValueKind(key) != Microsoft.Win32.RegistryValueKind.DWord)
            return defaultValue;

        return (int)RegistryKey.GetValue(key)!;
    }

    public static void SetInt32(string key, int? value)
    {
        if (RegistryKey == null)
            return;

        if (value == null)
            RegistryKey.DeleteValue(key);
        else
            RegistryKey.SetValue(key, value, Microsoft.Win32.RegistryValueKind.DWord);
    }

    public static bool? GetBoolean(string key, bool? defaultValue = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        string[] names = RegistryKey.GetValueNames();

        if (Array.IndexOf(names, key) < 0)
            return defaultValue;

        if (RegistryKey.GetValueKind(key) != Microsoft.Win32.RegistryValueKind.DWord)
            return defaultValue;

        return (int)RegistryKey.GetValue(key)! != 0;
    }

    public static void SetBoolean(string key, bool? value)
    {
        if (RegistryKey == null)
            return;

        if (value == null)
            RegistryKey.DeleteValue(key);
        else
            RegistryKey.SetValue(key, value.Value ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
    }
}
