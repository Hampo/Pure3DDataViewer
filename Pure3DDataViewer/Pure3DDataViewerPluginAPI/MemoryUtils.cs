using SHARMemory.SHAR.Classes;
using System.Numerics;

namespace Pure3DDataViewerPluginAPI;
public static class MemoryUtils
{
    public static SHARMemory.SHAR.Memory? GetSHARMemory()
    {
        var p = SHARMemory.SHAR.Memory.GetSHARProcess();
        return p == null ? null : new(p);
    }

    public static (Vector3, float)? GetPosition()
    {
        using var mem = GetSHARMemory();
        if (mem == null)
            return null;

        if (mem.Singletons.CharacterManager is not CharacterManager characterManager)
            return null;

        if (characterManager.Player is not Character player)
            return null;

        if (player.Car is Vehicle car)
        {
            var mat = car.Transform;

            var wheelRadius = car.Wheels[0].Radius;
            var suspensionRestPoint = car.SuspensionRestPoints[0];

            var deltaX = mat.M11;
            var deltaY = mat.M13;

            var rot = -Math.Atan2(deltaY, deltaX);
            while (rot < 0)
                rot += Math.PI * 2;

            return (new(mat.M41, mat.M42 - wheelRadius - suspensionRestPoint.Y, mat.M43), (float)rot);
        }
        else
        {
            var pos = player.Position;
            var rot = player.Rotation;

            return (new(pos.X, pos.Y, pos.Z), rot);
        }
    }
}
