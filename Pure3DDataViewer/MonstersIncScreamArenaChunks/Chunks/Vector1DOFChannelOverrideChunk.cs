using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Collections;

namespace MonstersIncScreamArenaChunks.Chunks;

[ChunkAttributes(0x121102)]
public class Vector1DOFChannelOverrideChunk : Chunk
{
    private uint _version;
    public uint Version
    {
        get => _version;
        set
        {
            if (_version == value)
                return;

            _version = value;
            OnPropertyChanged(nameof(Version));
        }
    }

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

            data.AddRange(BitConverter.GetBytes(Version));
            data.AddRange(Unknown);

            return [.. data];
        }
    }
    public override uint DataLength => sizeof(uint) + UnknownSize;

    public Vector1DOFChannelOverrideChunk(EndianAwareBinaryReader br, uint headerSize) : this(br.ReadUInt32(), br.ReadBytes((int)headerSize - sizeof(uint)))
    {
    }

    public Vector1DOFChannelOverrideChunk(uint version, IList<byte> unknown) : base(0x121102)
    {
        _version = version;
        Unknown = CreateSizeAwareList(unknown, Unknown_CollectionChanged);
    }

    private void Unknown_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) => OnPropertyChanged(nameof(Unknown));

    protected override void WriteData(EndianAwareBinaryWriter bw)
    {
        bw.Write(Version);
        bw.Write([.. Unknown]);
    }

    protected override Chunk CloneSelf() => new Vector1DOFChannelOverrideChunk(Version, Unknown);
}
