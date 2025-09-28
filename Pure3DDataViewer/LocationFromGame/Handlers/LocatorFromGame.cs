using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Enums;
using Pure3DDataViewerPluginAPI.Interfaces;

namespace LocationFromGame.Handlers;
internal class LocatorFromGame : IChunkHandler<LocatorChunk>
{
    public string Name => "Set Location From Game";

    public Image? Image => LocationFromGamePlugin.FromGameImage;

    public IList<(string Name, bool Value)>? GetSettings() => null;

    public void SetSetting(string name, bool value) { }

    public ChunkCallbackResult Handle(LocatorChunk chunk)
    {
        if (!MemoryTunnel.TunnelExeExists)
        {
            MessageBox.Show($"Unable to find Memory Tunnel at \"{MemoryTunnel.TunnelExePath}.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ChunkCallbackResult.Unchanged;
        }

        try
        {
            var pos = MemoryTunnel.GetPosition();

            if (pos == null)
            {
                MessageBox.Show("Failed to retrieve position from game.\nEnsure the game is open and you're in gameplay.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ChunkCallbackResult.Unchanged;
            }

            chunk.Position = pos.Position;
            return ChunkCallbackResult.Modified;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"There was an error retrieving position from game: {ex}", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        return ChunkCallbackResult.Unchanged;
    }

}
