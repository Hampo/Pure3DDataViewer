using Be.Windows.Forms;
using NetP3DLib.P3D;

namespace Pure3DDataViewer;

internal class ChunkByteProvider : IByteProvider, IDisposable
{
    private readonly Chunk Chunk;

    private long _lastLength;
    public long Length => Chunk.DataLength;

    public event EventHandler? LengthChanged;
    public event EventHandler? Changed;

    public ChunkByteProvider(Chunk chunk)
    {
        Chunk = chunk;
        _lastLength = Chunk.DataLength;
        Chunk.PropertyChanged += Chunk_PropertyChanged;
    }

    private void Chunk_PropertyChanged(string obj)
    {
        Changed?.Invoke(this, EventArgs.Empty);
        if (_lastLength != Chunk.DataLength)
        {
            _lastLength = Chunk.DataLength;
            LengthChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ApplyChanges()
    {
    }

    public void DeleteBytes(long index, long length) => throw new NotSupportedException();

    public bool HasChanges() => false;

    public void InsertBytes(long index, byte[] bs) => throw new NotSupportedException();

    public byte ReadByte(long index)
    {
        if (index < 0 || index >= Length)
            throw new ArgumentOutOfRangeException(nameof(index));

        return Chunk.DataBytes[index];
    }

    public bool SupportsDeleteBytes() => false;

    public bool SupportsInsertBytes() => false;

    public bool SupportsWriteByte() => false;

    public void WriteByte(long index, byte value) => throw new NotSupportedException();

    public void Dispose() => Chunk.PropertyChanged -= Chunk_PropertyChanged;
}
