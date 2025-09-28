using Pure3DDataViewerPluginAPI.Models;
using SHARMemory.SHAR.Classes;

namespace Pure3DDataViewerPluginAPIMemoryTunnel;
public static class MemoryTunnel
{
    private static SHARMemory.SHAR.Memory? GetSHARMemory()
    {
        var p = SHARMemory.SHAR.Memory.GetSHARProcess();
        return p == null ? null : new(p);
    }

    public static PositionAndRotation? GetPosition()
    {
        using var mem = GetSHARMemory();
        if (mem == null)
            return null;

        if (mem.Singletons.CharacterManager is not CharacterManager characterManager)
            return null;

        if (characterManager.Player is not Character player)
            return null;

        SHARMemory.SHAR.Structs.Vector3 pos;
        float rot;

        var car = player.Car;
        if (car == null)
        {
            pos = player.Position;
            rot = player.Rotation * (180f / MathF.PI);
        }
        else
        {
            pos = car.Position;
            var facing = car.VehicleFacing;
            rot = MathF.Atan2(facing.X, facing.Z) * (180f / MathF.PI);
            if (rot < 0)
                rot += 360f;
        }

        return new(pos.X, pos.Y, pos.Z, rot);
    }
}
