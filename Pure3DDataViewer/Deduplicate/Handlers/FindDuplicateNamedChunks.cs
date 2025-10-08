using Deduplicate.Forms;
using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace Deduplicate.Handlers;
public class FindDuplicateNamedChunks : IFileHandler
{
    public string Name => "Find Duplicate Named Chunks";

    public Image? Image => DeduplicatePlugin.FindDuplicateNamedImage;

    public static bool IgnoreAllowedDuplicates
    {
        get => RegistryUtils.GetBoolean("DeduplicateIgnoreAllowedDuplicates", true)!.Value;
        set => RegistryUtils.SetBoolean("DeduplicateIgnoreAllowedDuplicates", value);
    }

    internal static HashSet<Type> AllowedDuplicates
    {
        get
        {
            var typeNames = RegistryUtils.GetStringArray("FindDuplicateNamedChunksAllowedDuplicates", [
                "NetP3DLib.P3D.Chunks.StaticEntityChunk, NetP3DLib",
            ]);
            if (typeNames == null)
                return [];

            var allowedDuplicates = new HashSet<Type>(typeNames.Length);
            foreach (var typeName in typeNames)
            {
                var type = Type.GetType(typeName, false);
                if (type == null)
                    continue;

                allowedDuplicates.Add(type);
            }

            return allowedDuplicates;
        }
        set => RegistryUtils.SetStringArray("FindDuplicateNamedChunksAllowedDuplicates", [.. value.Where(x => x != null).Select(x => $"{x.FullName}, {x.Assembly.GetName().Name}")]);
    }

    public IList<(string Name, bool Value)>? GetSettings() => [
        ( "Ignore Allowed Duplicates", IgnoreAllowedDuplicates ),
        ( "Configure Allowed Duplicates", false),
    ];

    public void SetSetting(string name, bool value)
    {
        switch (name)
        {
            case "Ignore Allowed Duplicates":
                IgnoreAllowedDuplicates = value;
                break;
            case "Configure Allowed Duplicates":
                using (var frmConfigureAllowedDuplicates = new FrmConfigureAllowedDuplicates())
                {
                    if (frmConfigureAllowedDuplicates.ShowDialog() != DialogResult.OK)
                        return;

                    AllowedDuplicates = frmConfigureAllowedDuplicates.AllowedDuplicates;
                }
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
        List<int> indexes = [];

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
            {
                foundChunks.Add($"{i}. {chunk}");
                indexes.Add(i);
            }

            seenNames.Add(chunk.Name);
        }

        if (foundChunks.Count == 0)
        {
            MessageBox.Show("No duplicate named chunks found.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        if (MessageBox.Show($"Found {foundChunks.Count} duplicate named chunks.\nWould you like to remove them?\n\n- {string.Join("\n- ", foundChunks)}", Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.No)
            return FileCallbackResult.Unchanged;

        foreach (var index in indexes.OrderByDescending(x => x))
            p3dFile.Chunks.RemoveAt(index);

        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
