using NetP3DLib.P3D;
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

    public static TabPage? GetLastTabPage(TabControl tc, Type type)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return null;

        var tpName = RegistryUtils.GetString(type.FullName, null, "LastEditor");
        return tc.TabPages.Cast<TabPage>().FirstOrDefault(tp => tp.Name == tpName);
    }

    public static void SetLastTabPage(Type type, TabPage tp)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return;

        RegistryUtils.SetString(type.FullName, tp.Name, "LastEditor");
    }

    public static string FindQuery
    {
        get => RegistryUtils.GetString("FindQuery", string.Empty)!;
        set => RegistryUtils.SetString("FindQuery", value);
    }

    public static (Color BackColour, Color ForeColour) GetErrorChunkColour()
    {
        var backColor = Color.FromArgb(RegistryUtils.GetInt32("Error_BackColour", Color.FromArgb(255, 230, 230).ToArgb(), "ChunkColours")!.Value);
        var foreColor = Color.FromArgb(RegistryUtils.GetInt32("Error_ForeColour", Color.DarkRed.ToArgb(), "ChunkColours")!.Value);

        return (backColor, foreColor);
    }

    public static void SetErrorChunkColour(Color backColour, Color foreColour)
    {
        SetErrorChunkBackColour(backColour);
        SetErrorChunkForeColour(foreColour);
    }

    public static void SetErrorChunkBackColour(Color backColour)
    {
        RegistryUtils.SetInt32("Error_BackColour", backColour.ToArgb(), "ChunkColours");
    }

    public static void SetErrorChunkForeColour(Color foreColour)
    {
        RegistryUtils.SetInt32("Error_ForeColour", foreColour.ToArgb(), "ChunkColours");
    }

    public static void ResetErrorChunkColour()
    {
        RegistryUtils.SetInt32("Error_BackColour", Color.FromArgb(255, 230, 230).ToArgb(), "ChunkColours");
        RegistryUtils.SetInt32("Error_ForeColour", Color.DarkRed.ToArgb(), "ChunkColours");
    }

    public static (Color BackColour, Color ForeColour) GetChunkColour(Type type)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return (Color.Empty, Color.Empty);

        var backColorInt = RegistryUtils.GetInt32($"{type.FullName}_BackColour", type == typeof(UnknownChunk) ? Color.LightGoldenrodYellow.ToArgb() : null, "ChunkColours");
        var foreColorInt = RegistryUtils.GetInt32($"{type.FullName}_ForeColour", null, "ChunkColours");

        Color backColor = backColorInt.HasValue ? Color.FromArgb(backColorInt.Value) : Color.Empty;
        Color foreColor = foreColorInt.HasValue ? Color.FromArgb(foreColorInt.Value) : Color.Empty;

        return (backColor, foreColor);
    }

    public static void SetChunkColour(Type type, Color backColour, Color foreColour)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return;

        SetChunkBackColour(type, backColour);
        SetChunkForeColour(type, foreColour);
    }

    public static void SetChunkBackColour(Type type, Color backColour)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return;

        RegistryUtils.SetInt32($"{type.FullName}_BackColour", backColour.ToArgb(), "ChunkColours");
    }

    public static void SetChunkForeColour(Type type, Color foreColour)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return;

        RegistryUtils.SetInt32($"{type.FullName}_ForeColour", foreColour.ToArgb(), "ChunkColours");
    }

    public static void ResetChunkColour(Type type)
    {
        if (string.IsNullOrEmpty(type.FullName))
            return;

        RegistryUtils.SetInt32($"{type.FullName}_BackColour", null, "ChunkColours");
        RegistryUtils.SetInt32($"{type.FullName}_ForeColour", null, "ChunkColours");
    }

    public static bool DarkMode
    {
        get => RegistryUtils.GetBoolean("DarkMode", false)!.Value;
        set => RegistryUtils.SetBoolean("DarkMode", value);
    }

    public static bool LargeFont
    {
        get => RegistryUtils.GetBoolean("LargeFont", false)!.Value;
        set => RegistryUtils.SetBoolean("LargeFont", value);
    }
}
