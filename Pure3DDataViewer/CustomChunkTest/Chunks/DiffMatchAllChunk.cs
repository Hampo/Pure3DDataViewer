using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;

namespace CustomChunkTest.Chunks;
[ChunkAttributes(0x73737373)]
public class DiffMatchAllChunk : Chunk
{
    public uint ChunkIndex { get; set; }

    public override byte[] DataBytes
    {
        get
        {
            List<byte> data = [];

            data.AddRange(BitConverter.GetBytes(ChunkIndex));

            return [.. data];
        }
    }
    public override uint DataLength => sizeof(uint);

    public DiffMatchAllChunk(EndianAwareBinaryReader br) : base(0x73737373)
    {
        ChunkIndex = br.ReadUInt32();
    }

    public DiffMatchAllChunk(uint chunkIndex) : base(0x73737373)
    {
        ChunkIndex = chunkIndex;
    }

    protected override void WriteData(EndianAwareBinaryWriter bw)
    {
        bw.Write(ChunkIndex);
    }

    protected override Chunk CloneSelf() => new DiffMatchAllChunk(ChunkIndex);
}
