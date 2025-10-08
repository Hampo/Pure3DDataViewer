using NetP3DLib.P3D.Chunks;

namespace CarPhysicsObjectGenerator.Forms;
public partial class FrmSelectCompositeDrawable : Form
{
    public CompositeDrawableChunk Value => (CompositeDrawableChunk)LBCompositeDrawables.SelectedItem!;

    public FrmSelectCompositeDrawable(CompositeDrawableChunk[] compositeDrawables)
    {
        InitializeComponent();

        LBCompositeDrawables.Items.AddRange(compositeDrawables);
        LBCompositeDrawables.SelectedIndex = 0;
    }
}
