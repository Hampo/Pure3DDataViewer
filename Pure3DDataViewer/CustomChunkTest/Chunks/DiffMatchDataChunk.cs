using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;

namespace CustomChunkTest.Chunks;
[ChunkAttributes(0x69696969)]
public class DiffMatchDataChunk : Chunk
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

    public DiffMatchDataChunk(EndianAwareBinaryReader br) : base(0x69696969)
    {
        ChunkIndex = br.ReadUInt32();
    }

    public DiffMatchDataChunk(uint chunkIndex) : base(0x69696969)
    {
        ChunkIndex = chunkIndex;
    }

    protected override void WriteData(EndianAwareBinaryWriter bw)
    {
        bw.Write(ChunkIndex);
    }

    protected override Chunk CloneSelf() => new DiffMatchDataChunk(ChunkIndex);
}
