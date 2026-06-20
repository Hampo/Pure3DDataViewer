using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Collections;

namespace MonstersIncScreamArenaChunks.Chunks;

[ChunkAttributes(0x1000C)]
public class WeightListOverrideChunk : Chunk
{
    public uint UnknownSize
    {
        get => (uint)(Unknown?.Count ?? 0);
        set
        {
            if (value == UnknownSize)
                return;

            if (value < UnknownSize)
            {
                Unknown.RemoveRange((int)value, (int)(UnknownSize - value));
            }
            else
            {
                int count = (int)(value - UnknownSize);
                var newVertices = new byte[count];

                for (var i = 0; i < count; i++)
                    newVertices[i] = default;

                Unknown.AddRange(newVertices);
            }
        }
    }
    public SizeAwareList<byte> Unknown { get; }

    public override byte[] DataBytes
    {
        get
        {
            List<byte> data = [];

            data.AddRange(Unknown);

            return [.. data];
        }
    }
    public override uint DataLength => UnknownSize;

    public WeightListOverrideChunk(EndianAwareBinaryReader br, uint headerSize) : this(br.ReadBytes((int)headerSize))
    {
    }

    public WeightListOverrideChunk(IList<byte> unknown) : base(0x1000C)
    {
        Unknown = CreateSizeAwareList(unknown, Unknown_CollectionChanged);
    }

    private void Unknown_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(Unknown));

    protected override void WriteData(EndianAwareBinaryWriter bw)
    {
        bw.Write([.. Unknown]);
    }

    protected override Chunk CloneSelf() => new WeightListOverrideChunk(Unknown);
}
