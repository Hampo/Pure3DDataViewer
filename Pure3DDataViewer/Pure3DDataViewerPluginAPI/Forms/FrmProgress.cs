namespace Pure3DDataViewerPluginAPI.Forms;

public partial class FrmProgress : Form
{
    public FrmProgress(string text)
    {
        InitializeComponent();
        Text = text;
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
