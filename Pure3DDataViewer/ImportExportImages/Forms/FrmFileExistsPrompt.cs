using ImportExportImages.Enums;

namespace ImportExportImages.Forms;
public partial class FrmFileExistsPrompt : Form
{
    public FileExistsResult Result { get; private set; }

    public bool ApplyToAll => CBApplyToAll.Checked;

    public FrmFileExistsPrompt(string fileName)
    {
        InitializeComponent();

        LblInfo.Text += fileName;
    }

    private void BtnKeepBoth_Click(object sender, EventArgs e) => Result = FileExistsResult.KeepBoth;

    private void BtnOverwrite_Click(object sender, EventArgs e) => Result = FileExistsResult.Overwrite;

    private void BtnKeepOriginal_Click(object sender, EventArgs e) => Result = FileExistsResult.KeepOriginal;
}
