using ConvertToLua.Helpers;
using NetP3DLib.P3D;
using System.Text;

namespace ConvertToLua.Forms;

public partial class FrmViewLua : Form
{
    private readonly P3DFile? _p3dFile = null;
    private readonly Chunk? _chunk = null;

    public FrmViewLua() => InitializeComponent();

    public FrmViewLua(P3DFile p3dFile) : this() => _p3dFile = p3dFile;

    public FrmViewLua(Chunk chunk) : this() => _chunk = chunk;

    private async void FrmViewLua_Shown(object sender, EventArgs e)
    {
        if (_p3dFile == null && _chunk == null)
        {
            PBProgress.Visible = false;
            TxtLua.Text = "Nothing to load";
            return;
        }

        var sb = new StringBuilder();
        PBProgress.Minimum = 0;
        PBProgress.Value = 0;
        PBProgress.Visible = true;
        var progress = new Progress<int>(value =>
        {
            PBProgress.Value += value;
        });

        if (_p3dFile != null)
        {
            sb.AppendLine("local P3DFile = P3D.P3DFile()");
            sb.AppendLine();

            PBProgress.Maximum = _p3dFile.AllChunks.Count;
            await ChunkMap.ProcessChunksAsync(progress, sb, "P3DFile", _p3dFile.Chunks);

            sb.AppendLine();
            sb.AppendLine("P3DFile:Output()");
        }
        else if (_chunk != null)
        {
            sb.AppendLine($"local Chunk = {ChunkMap.GetLuaConstructor(_chunk)}");
            sb.AppendLine();

            PBProgress.Maximum = _chunk.AllChildren.Count;
            await ChunkMap.ProcessChunksAsync(progress, sb, "Chunk", _chunk.Children);
        }

        PBProgress.Visible = false;
        TxtLua.SuspendLayout();
        TxtLua.Enabled = true;
        TxtLua.Text = sb.ToString();
        TxtLua.SelectionStart = 0;
        TxtLua.ResumeLayout();
        BtnCopy.Enabled = true;
    }

    private void BtnCopy_Click(object sender, EventArgs e) => Clipboard.SetText(TxtLua.Text);
}
