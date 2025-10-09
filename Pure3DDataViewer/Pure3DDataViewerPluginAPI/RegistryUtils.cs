namespace Pure3DDataViewerPluginAPI;
public static class RegistryUtils
{
    private const string RegistrySettings = @"Software\Pure3DDataViewer";
    public static readonly Microsoft.Win32.RegistryKey RegistryKey = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RegistrySettings, Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree);

    public static string[]? GetStringArray(string name, string[]? defaultValue = null, string? subKey = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                return defaultValue;

            registryKey = RegistryKey.OpenSubKey(subKey, false);
            if (registryKey == null)
                return defaultValue;
        }

        string[] names = registryKey.GetValueNames();

        if (Array.IndexOf(names, name) < 0)
            return defaultValue;

        if (registryKey.GetValueKind(name) != Microsoft.Win32.RegistryValueKind.MultiString)
            return defaultValue;
        
        return (string[])registryKey.GetValue(name)!;
    }

    public static void SetStringArray(string name, string[]? value, string? subKey = null)
    {
        if (RegistryKey == null)
            return;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                registryKey = RegistryKey.CreateSubKey(subKey, true);
            else
                registryKey = RegistryKey.OpenSubKey(subKey, true);

            if (registryKey == null)
                return;
        }

        if (value == null)
            registryKey.DeleteValue(name);
        else
            registryKey.SetValue(name, value, Microsoft.Win32.RegistryValueKind.MultiString);
    }

    public static string? GetString(string name, string? defaultValue = null, string? subKey = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                return defaultValue;

            registryKey = RegistryKey.OpenSubKey(subKey, false);
            if (registryKey == null)
                return defaultValue;
        }

        string[] names = registryKey.GetValueNames();

        if (Array.IndexOf(names, name) < 0)
            return defaultValue;

        if (registryKey.GetValueKind(name) != Microsoft.Win32.RegistryValueKind.String)
            return defaultValue;

        return (string)registryKey.GetValue(name)!;
    }

    public static void SetString(string name, string? value, string? subKey = null)
    {
        if (RegistryKey == null)
            return;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                registryKey = RegistryKey.CreateSubKey(subKey, true);
            else
                registryKey = RegistryKey.OpenSubKey(subKey, true);

            if (registryKey == null)
                return;
        }

        if (value == null)
            registryKey.DeleteValue(name);
        else
            registryKey.SetValue(name, value, Microsoft.Win32.RegistryValueKind.String);
    }

    public static int? GetInt32(string name, int? defaultValue = null, string? subKey = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                return defaultValue;

            registryKey = RegistryKey.OpenSubKey(subKey, false);
            if (registryKey == null)
                return defaultValue;
        }

        string[] names = registryKey.GetValueNames();

        if (Array.IndexOf(names, name) < 0)
            return defaultValue;

        if (registryKey.GetValueKind(name) != Microsoft.Win32.RegistryValueKind.DWord)
            return defaultValue;

        return (int)registryKey.GetValue(name)!;
    }

    public static void SetInt32(string name, int? value, string? subKey = null)
    {
        if (RegistryKey == null)
            return;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                registryKey = RegistryKey.CreateSubKey(subKey, true);
            else
                registryKey = RegistryKey.OpenSubKey(subKey, true);

            if (registryKey == null)
                return;
        }

        if (value == null)
            registryKey.DeleteValue(name);
        else
            registryKey.SetValue(name, value, Microsoft.Win32.RegistryValueKind.DWord);
    }

    public static bool? GetBoolean(string name, bool? defaultValue = null, string? subKey = null)
    {
        if (RegistryKey == null)
            return defaultValue;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                return defaultValue;

            registryKey = RegistryKey.OpenSubKey(subKey, false);
            if (registryKey == null)
                return defaultValue;
        }

        string[] names = registryKey.GetValueNames();

        if (Array.IndexOf(names, name) < 0)
            return defaultValue;

        if (registryKey.GetValueKind(name) != Microsoft.Win32.RegistryValueKind.DWord)
            return defaultValue;

        return (int)registryKey.GetValue(name)! != 0;
    }

    public static void SetBoolean(string name, bool? value, string? subKey = null)
    {
        if (RegistryKey == null)
            return;

        var registryKey = RegistryKey;
        if (!string.IsNullOrEmpty(subKey))
        {
            var subKeys = RegistryKey.GetSubKeyNames();
            if (Array.IndexOf(subKeys, subKey) < 0)
                registryKey = RegistryKey.CreateSubKey(subKey, true);
            else
                registryKey = RegistryKey.OpenSubKey(subKey, true);

            if (registryKey == null)
                return;
        }

        if (value == null)
            registryKey.DeleteValue(name);
        else
            registryKey.SetValue(name, value.Value ? 1 : 0, Microsoft.Win32.RegistryValueKind.DWord);
    }
}
