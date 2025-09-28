using Deduplicate.Handlers;
using NetP3DLib.P3D;

namespace Deduplicate.Forms;
public partial class FrmConfigureAllowedDuplicates : Form
{
    public HashSet<Type> AllowedDuplicates => [.. LBAllowedTypes.Items.Cast<Type>()];

    public FrmConfigureAllowedDuplicates()
    {
        InitializeComponent();
    }

    private void FrmConfigureAllowedDuplicates_Load(object sender, EventArgs e)
    {
        var allowedTypes = FindDuplicateNamedChunks.AllowedDuplicates;
        foreach (var type in ChunkLoader.ChunkTypes.Values.Select(x => x.Item1))
            if (allowedTypes.Contains(type))
                LBAllowedTypes.Items.Add(type);
            else
                LBDisallowedTypes.Items.Add(type);
    }

    private void LBTypes_Format(object sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is Type t)
            e.Value = t.Name;
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        if (LBDisallowedTypes.SelectedItems.Count == 0)
            return;

        LBAllowedTypes.SelectedIndex = -1;
        foreach (var type in LBDisallowedTypes.SelectedItems.Cast<Type>().ToArray())
        {
            LBDisallowedTypes.Items.Remove(type);
            LBAllowedTypes.SelectedIndex = LBAllowedTypes.Items.Add(type);
        }
    }

    private void BtnRemove_Click(object sender, EventArgs e)
    {
        if (LBAllowedTypes.SelectedItems.Count == 0)
            return;

        LBDisallowedTypes.SelectedIndex = -1;
        foreach (var type in LBAllowedTypes.SelectedItems.Cast<Type>().ToArray())
        {
            LBAllowedTypes.Items.Remove(type);
            LBDisallowedTypes.SelectedIndex = LBDisallowedTypes.Items.Add(type);
        }
    }
}
