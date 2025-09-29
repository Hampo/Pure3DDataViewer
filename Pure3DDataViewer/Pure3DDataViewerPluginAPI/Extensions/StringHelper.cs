namespace Pure3DDataViewerPluginAPI.Extensions;
public static class StringHelper
{
    private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();
    public static string SanitizeFileName(this string name, char replaceCharacter = '_')
    {
        if (string.IsNullOrEmpty(name))
            return replaceCharacter.ToString();

        foreach (var c in InvalidFileNameChars)
            name = name.Replace(c, replaceCharacter);

        return name;
    }

    private static readonly char[] InvalidDirectoryNameChars = [.. InvalidFileNameChars.Concat([':', '*', '?', '"', '<', '>', '|']).Distinct()];
    public static string SanitizeDirectoryName(this string name, char replaceCharacter = '_')
    {
        if (string.IsNullOrEmpty(name))
            return replaceCharacter.ToString();

        foreach (var c in InvalidDirectoryNameChars)
            name = name.Replace(c, replaceCharacter);

        return name;
    }
}
