using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using System.Globalization;
using System.Numerics;
using System.Reflection;

namespace ConvertToLua.Helpers;

internal static class ChunkMap
{
    private static readonly Dictionary<Type, LuaChunkMapping> _mappings = [];

    static ChunkMap()
    {
        Register<WallChunk>(new()
        {
            LuaClassName = "Fence2",
            PropertyOrder = new()
            {
                { "Start", 1 },
                { "End", 2 },
                { "Normal", 3 },
            }
        });

        Register<TextureChunk>(new()
        {
            LuaClassName = "Texture",
            PropertyOrder =
            {
                { "Name", 1 },
                { "Version", 2 },
                { "Width", 3 },
                { "Height", 4 },
                { "Bpp", 5 },
                { "AlphaDepth", 6 },
                { "NumMipMaps", 7 },
                { "TextureType", 8 },
                { "UsageHint", 9 },
                { "Priority", 10 },
            }
        });
    }

    public static void Register<TChunk>(LuaChunkMapping mapping) where TChunk : Chunk => _mappings[typeof(TChunk)] = mapping;

    public static string GetLuaConstructor(Chunk chunk)
    {
        var type = chunk.GetType();

        if (!_mappings.TryGetValue(type, out var mapping))
            throw new NotSupportedException($"Chunk {chunk} is not supported.");

        var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);

        object?[] args = new object[mapping.PropertyOrder.Count];

        foreach (var prop in properties)
        {
            if (!mapping.PropertyOrder.TryGetValue(prop.Name, out int index))
                continue;

            var value = prop.GetValue(chunk);
            args[index - 1] = value;
        }

        var luaArgs = string.Join(", ", args.Select(FormatLuaValue));

        return $"P3D.{mapping.LuaClassName}P3DChunk({luaArgs})";
    }

    private static string FormatLuaValue(object? value)
    {
        if (value is null)
            return "nil";

        if (value is Enum e)
            return Convert.ToUInt64(e).ToString(CultureInfo.InvariantCulture);

        return value switch
        {
            string s => $"\"{s}\"",
            byte b => b.ToString(CultureInfo.InvariantCulture),
            sbyte sb => sb.ToString(CultureInfo.InvariantCulture),
            short s => s.ToString(CultureInfo.InvariantCulture),
            ushort us => us.ToString(CultureInfo.InvariantCulture),
            int i => i.ToString(CultureInfo.InvariantCulture),
            uint ui => ui.ToString(CultureInfo.InvariantCulture),
            long l => l.ToString(CultureInfo.InvariantCulture),
            ulong ul => ul.ToString(CultureInfo.InvariantCulture),
            float f => f.ToString(CultureInfo.InvariantCulture),
            double d => d.ToString(CultureInfo.InvariantCulture),
            Vector3 v => $"P3D.Vector3({v.X}, {v.Y}, {v.Z})",
            bool b => b ? "true" : "false",
            _ => throw new NotSupportedException($"Lua serialization not supported for {value.GetType().Name}")
        };
    }
}

internal sealed class LuaChunkMapping
{
    public string LuaClassName { get; init; } = null!;
    public Dictionary<string, int> PropertyOrder { get; init; } = [];
}
