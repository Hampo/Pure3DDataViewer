using System.Numerics;

namespace Pure3DDataViewerPluginAPI.Models;
public class PositionAndRotation
{
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
    public Vector3 Position => new(X, Y, Z);
    public float Rotation { get; set; }

    public PositionAndRotation() { }

    public PositionAndRotation(Vector3 position, float rotation) : this(position.X, position.Y, position.Z, rotation) { }

    public PositionAndRotation(float x, float y, float z, float rotation)
    {
        X = x;
        Y = y;
        Z = z;
        Rotation = rotation;
    }

    public override string ToString() => $"{Position} | {Rotation}";
}
