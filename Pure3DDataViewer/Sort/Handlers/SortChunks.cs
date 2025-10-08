using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace Sort.Handlers;
public class SortChunks : IFileHandler
{
    public string Name => "Sort Chunks";

    public Image? Image => SortPlugin.SortImage;

    public static bool IncludeSectionHeaders
    {
        get => RegistryUtils.GetBoolean("SortIncludeSectionHeaders", true)!.Value;
        set => RegistryUtils.SetBoolean("SortIncludeSectionHeaders", value);
    }

    public static bool AlphabeticalCaseSensitive
    {
        get => RegistryUtils.GetBoolean("SortAlphabeticalCaseSensitive", true)!.Value;
        set => RegistryUtils.SetBoolean("SortAlphabeticalCaseSensitive", value);
    }

    public static bool AlphabeticalCaseInsensitive
    {
        get => RegistryUtils.GetBoolean("SortAlphabeticalCaseInsensitive", false)!.Value;
        set => RegistryUtils.SetBoolean("SortAlphabeticalCaseInsensitive", value);
    }

    public static bool IncludeHistory
    {
        get => RegistryUtils.GetBoolean("SortIncludeHistory", true)!.Value;
        set => RegistryUtils.SetBoolean("SortIncludeHistory", value);
    }

    public IList<(string Name, bool Value)>? GetSettings() => [
        ( "Include Section Headers", IncludeSectionHeaders ),
        ( "Alphabetical (Case Sensitive)", AlphabeticalCaseSensitive ),
        ( "Alphabetical (Case Insensitive)", AlphabeticalCaseInsensitive ),
        ( "Include History Chunk", IncludeHistory ),
    ];

    public void SetSetting(string name, bool value)
    {
        switch (name)
        {
            case "Include Section Headers":
                IncludeSectionHeaders = value;
                break;
            case "Alphabetical (Case Sensitive)":
                AlphabeticalCaseSensitive = value;
                if (value)
                    AlphabeticalCaseInsensitive = false;
                break;
            case "Alphabetical (Case Insensitive)":
                AlphabeticalCaseInsensitive = value;
                if (value)
                    AlphabeticalCaseSensitive = false;
                break;
            case "Include History Chunk":
                IncludeHistory = value;
                break;
            default:
                MessageBox.Show($"Unsupported setting name: {name}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
        }
    }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        p3dFile.SortChunks(IncludeSectionHeaders, AlphabeticalCaseSensitive || AlphabeticalCaseInsensitive, IncludeHistory, AlphabeticalCaseInsensitive);

        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
