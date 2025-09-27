using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;

namespace Deduplicate.Handlers;
public class FindDuplicateNamedChunks : IFileHandler
{
    private static HashSet<Type> AllowedDuplicates = [
        typeof(StaticEntityChunk)
    ];

    public string Name => "Find Duplicate Named Chunks";

    public Image? Image => DeduplicatePlugin.FindDuplicateNamedImage;

    public static bool IgnoreAllowedDuplicates
    {
        get => RegistryUtils.GetBoolean("DeduplicateIgnoreAllowedDuplicates", true)!.Value;
        set => RegistryUtils.SetBoolean("DeduplicateIgnoreAllowedDuplicates", value);
    }

    public IList<(string Name, bool Value)>? GetSettings() => [
        ( "Ignore Allowed Duplicates", IgnoreAllowedDuplicates ),
    ];

    public void SetSetting(string name, bool value)
    {
        switch (name)
        {
            case "Ignore Allowed Duplicates":
                IgnoreAllowedDuplicates = value;
                break;
            default:
                MessageBox.Show($"Unsupported setting name: {name}", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
        }
    }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        Dictionary<Type, HashSet<string>> seenNameMap = [];

        List<string> foundChunks = [];

        var namedChunks = p3dFile.GetChunksOfType<NamedChunk>();
        for (int i = 0; i < namedChunks.Count; i++)
        {
            var chunk = namedChunks[i];
            var chunkType = chunk.GetType();

            if (IgnoreAllowedDuplicates && AllowedDuplicates.Contains(chunkType))
                continue;

            if (!seenNameMap.TryGetValue(chunkType, out var seenNames))
            {
                seenNameMap[chunkType] = [
                    chunk.Name,
                ];
                continue;
            }

            if (seenNames.Contains(chunk.Name))
                foundChunks.Add($"{i}. {chunk}");

            seenNames.Add(chunk.Name);
        }

        var message = foundChunks.Count == 0 ? "No duplicate named chunks found." : $"Found duplicate named chunks:\n\n{string.Join("\n", foundChunks)}";

        MessageBox.Show(message, Name, MessageBoxButtons.OK, MessageBoxIcon.Information);

        return FileCallbackResult.Unchanged;
    }
}
