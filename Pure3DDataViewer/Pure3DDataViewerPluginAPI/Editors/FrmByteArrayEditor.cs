using Be.Windows.Forms;

namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmByteArrayEditor : Form
{
    private byte[] _data;
    public byte[] Value
    {
        get => _data;
        set
        {
            if (_data.SequenceEqual(value))
                return;

            _data = value ?? [];

            if (_provider != null)
            {
                _provider.Changed -= Provider_Changed;
                _provider = null;
            }

            _provider = new DynamicByteProvider(_data);
            _provider.Changed += Provider_Changed;
            HBValue.ByteProvider = _provider;

            HBValue.ScrollByteIntoView(0);
            HBValue.Refresh();
        }
    }
    private DynamicByteProvider? _provider;

    public FrmByteArrayEditor(string propertyName, byte[] currentValue)
    {
        InitializeComponent();
        LblPropertyName.Text = $"{propertyName}:";
        _data = currentValue;
        _provider = new DynamicByteProvider(_data);
        _provider.Changed += Provider_Changed;
        HBValue.ByteProvider = _provider;
    }

    private void Provider_Changed(object? sender, EventArgs e)
    {
        if (_provider == null)
            return;

        _data = [.. _provider.Bytes];
    }

    private void FrmByteArrayEditor_Shown(object sender, EventArgs e)
    {
        HBValue.Focus();
    }

    private void BtnImport_Click(object sender, EventArgs e)
    {
        using var ofd = new OpenFileDialog()
        {
            CheckFileExists = true,
            CheckPathExists = true,
            ClientGuid = new Guid("61ee66c6-6d3b-49ab-8037-2e6478d0a0a2"),
            Filter = "All Files|*",
            Multiselect = false,
            Title = "Choose file to import bytes",
        };
        if (ofd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            Value = File.ReadAllBytes(ofd.FileName);
            MessageBox.Show($"Imported bytes from: {ofd.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to read file bytes: {ex}", "Error reading file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnExport_Click(object sender, EventArgs e)
    {
        using var sfd = new SaveFileDialog()
        {
            CheckWriteAccess = true,
            CheckPathExists = true,
            ClientGuid = new Guid("e37a1f55-d31e-4aba-801d-db366df217b7"),
            Filter = "All Files|*",
            OverwritePrompt = true,
            Title = "Choose file to export bytes",
        };
        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            File.WriteAllBytes(sfd.FileName, Value);
            MessageBox.Show($"Exported bytes to: {sfd.FileName}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to save file bytes: {ex}", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
