namespace Pure3DDataViewer;
public partial class FrmFind : Form
{
    private readonly FrmMain _frmMain;
    private bool _loaded = false;

    public FrmFind(FrmMain frmMain)
    {
        InitializeComponent();
        _frmMain = frmMain;
        CBMatchCase.Checked = Settings.FindMatchCase;
        CBWrapAround.Checked = Settings.FindWrapAround;
        CBIncludeProperties.Checked = Settings.FindIncludeProperties;
        RBUp.Checked = !Settings.FindDirection;
        RBDown.Checked = Settings.FindDirection;
    }

    private void TxtFind_TextChanged(object sender, EventArgs e) => BtnFindNext.Enabled = !string.IsNullOrEmpty(TxtFind.Text);

    private void BtnFindNext_Click(object sender, EventArgs e) => _frmMain.Find(TxtFind.Text);

    private void CBMatchCase_CheckedChanged(object sender, EventArgs e) => Settings.FindMatchCase = CBMatchCase.Checked;

    private void CBWrapAround_CheckedChanged(object sender, EventArgs e) => Settings.FindWrapAround = CBWrapAround.Checked;

    private void CBIncludeProperties_CheckedChanged(object sender, EventArgs e) => Settings.FindIncludeProperties = CBIncludeProperties.Checked;

    private void RBDown_CheckedChanged(object sender, EventArgs e) => Settings.FindDirection = RBDown.Checked;

    private void TxtFind_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Enter)
        {
            e.Handled = true;
            e.SuppressKeyPress = true;
            BtnFindNext.PerformClick();
        }
    }

    private void FrmFind_Shown(object sender, EventArgs e)
    {
        var location = Settings.FindWindowLocation;
        if (location.HasValue)
        {
            Location = location.Value;
            return;
        }

        var frmMainLocation = _frmMain.Location;
        Location = new(frmMainLocation.X + (_frmMain.Width - Width) / 2, frmMainLocation.Y + (_frmMain.Height - Height) / 2);
        _loaded = true;
    }

    private void FrmFind_LocationChanged(object sender, EventArgs e)
    {
        if (!_loaded)
            return;

        Settings.FindWindowLocation = Location;
    }

    private void BtnCancel_Click(object sender, EventArgs e) => Close();
}
