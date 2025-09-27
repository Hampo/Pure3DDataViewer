using NetP3DLib.P3D;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;

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
        List<Chunk> newChunks = [.. p3dFile.Chunks];
        List<Chunk> seenChunks = [];

        long removedCount = 0;

        int i = 0;
        while (i < newChunks.Count)
        {
            var chunk = newChunks[i];
            if (!seenChunks.Contains(chunk))
            {
                seenChunks.Add(chunk);
                i++;
            }
            else
            {
                newChunks.RemoveAt(i);
                removedCount++;
            }
        }

        if (removedCount == 0)
        {
            MessageBox.Show("No duplicate chunks found.", "Remove Duplicate Chunks", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return FileCallbackResult.Unchanged;
        }

        if (MessageBox.Show($"Found {removedCount} duplicate chunk{(removedCount == 1 ? "" : "s")}.\nDo you want to remove {(removedCount == 1 ? "it" : "them")}?", "Remove Duplicate Chunks", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return FileCallbackResult.Unchanged;

        p3dFile.Chunks.Clear();
        p3dFile.Chunks.AddRange(newChunks);
        return FileCallbackResult.Modified;
    }
}
