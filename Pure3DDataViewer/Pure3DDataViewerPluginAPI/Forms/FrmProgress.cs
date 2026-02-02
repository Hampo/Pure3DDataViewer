namespace Pure3DDataViewerPluginAPI.Forms;

public partial class FrmProgress : Form
{
    public FrmProgress(string text, bool allowCancel = true)
    {
        InitializeComponent();
        Text = text;
        if (!allowCancel)
            ControlBox = false;
    }

    public void UpdateProgress(int value)
    {
        if (InvokeRequired)
        {
            Invoke(new Action<int>(UpdateProgress), value);
        }
        else
        {
            PBProgress.Value = value;
        }
    }
}
