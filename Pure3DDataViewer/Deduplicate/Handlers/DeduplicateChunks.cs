using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace Deduplicate.Handlers;
public class DeduplicateChunks : IFileHandler
{
    public string Name => "Remove Duplicate Chunks";

    public Image? Image => DeduplicatePlugin.DeduplicateImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value)
    { }

    public FileCallbackResult Handle(P3DFile p3dFile)
    {
        var seenChunks = new HashSet<Chunk>(p3dFile.Chunks.Count);
        var dupeIndices = new List<int>(p3dFile.Chunks.Count);

        for (int i = 0; i < p3dFile.Chunks.Count; i++)
        {
            var chunk = p3dFile.Chunks[i];
            if (seenChunks.Contains(chunk))
                dupeIndices.Add(i);
            seenChunks.Add(chunk);
        }

        var removedCount = dupeIndices.Count;
        if (removedCount == 0)
        {
            MessageBox.Show("No duplicate chunks found.", Name, MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        if (MessageBox.Show($"Found {removedCount} duplicate chunk{(removedCount == 1 ? "" : "s")}.\nDo you want to remove {(removedCount == 1 ? "it" : "them")}?", Name, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return FileCallbackResult.Unchanged;

        p3dFile.Chunks.RemoveAtIndices(dupeIndices);
        return FileCallbackResult.Modified;
    }

    public bool IsFileSupported(P3DFile p3dFile) => true;
}
