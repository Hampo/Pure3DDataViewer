using SHARMemory.SHAR.Classes;
using System.Numerics;

namespace Pure3DDataViewerPluginAPI;
public static class MemoryTunnel
{
    public static SHARMemory.SHAR.Memory? GetSHARMemory()
    {
        var p = SHARMemory.SHAR.Memory.GetSHARProcess();
        return p == null ? null : new(p);
    }

    public static Vector3? GetPosition()
    {
        using var mem = GetSHARMemory();
        if (mem == null)
            return null;

        if (mem.Singletons.CharacterManager is not CharacterManager characterManager)
            return null;

        if (characterManager.Player is not Character player)
            return null;

        var pos = player.Car?.Position ?? player.Position;
        return new(pos.X, pos.Y, pos.Z);
    }
}
