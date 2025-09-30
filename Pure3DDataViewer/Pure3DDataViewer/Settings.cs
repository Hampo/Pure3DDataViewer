using Pure3DDataViewerPluginAPI;

namespace Pure3DDataViewer;
public static class Settings
{
    public static IReadOnlyList<string> RecentFiles
    {
        get => RegistryUtils.GetStringArray("RecentFiles", [])!.AsReadOnly();
        set => RegistryUtils.SetStringArray("RecentFiles", [.. value]);
    }
    public static void AddRecentFile(string filePath)
    {
        var recentFiles = RecentFiles.ToList();
        int index = recentFiles.IndexOf(filePath);
        if (index != -1)
            recentFiles.RemoveAt(index);
        recentFiles.Insert(0, Path.GetFullPath(filePath));
        while (recentFiles.Count > 10)
            recentFiles.RemoveAt(RecentFiles.Count - 1);

        RecentFiles = recentFiles;
    }

    public static Point? FindWindowLocation
    {
        get
        {
            var findWindowX = RegistryUtils.GetInt32("FindWindowX");
            if (findWindowX == null)
                return null;

            var findWindowY = RegistryUtils.GetInt32("FindWindowY");
            if (findWindowY == null)
                return null;

            return new(findWindowX.Value, findWindowY.Value);
        }
        set
        {
            RegistryUtils.SetInt32("FindWindowX", value?.X);
            RegistryUtils.SetInt32("FindWindowY", value?.Y);
        }
    }

    public static bool FindMatchCase
    {
        get => RegistryUtils.GetBoolean("FindMatchCase", false)!.Value;
        set => RegistryUtils.SetBoolean("FindMatchCase", value);
    }

    public static bool FindWrapAround
    {
        get => RegistryUtils.GetBoolean("FindWrapAround", true)!.Value;
        set => RegistryUtils.SetBoolean("FindWrapAround", value);
    }

    public static bool FindIncludeProperties
    {
        get => RegistryUtils.GetBoolean("FindIncludeProperties", false)!.Value;
        set => RegistryUtils.SetBoolean("FindIncludeProperties", value);
    }

    public static bool FindDirection
    {
        get => RegistryUtils.GetBoolean("FindDirection", true)!.Value;
        set => RegistryUtils.SetBoolean("FindDirection", value);
    }

    public static Type? LastNewChunkType
    {
        get
        {
            var typeName = RegistryUtils.GetString("LastNewChunkType");
            if (string.IsNullOrWhiteSpace(typeName))
                return null;

            return Type.GetType(typeName, false);
        }
        set => RegistryUtils.SetString("LastNewChunkType", value != null ? $"{value.FullName}, {value.Assembly.GetName().Name}" : null);
    }
}
