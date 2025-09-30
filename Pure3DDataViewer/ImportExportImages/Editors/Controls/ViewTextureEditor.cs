using Pure3DDataViewerPluginAPI.Extensions;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Controls;

namespace ImportExportImages.Editors.Controls;
public partial class ViewTextureEditor : EditorControl<TextureChunk>
{
    public static Color BackgroundColor
    {
        get => Color.FromArgb(RegistryUtils.GetInt32("ViewImageBackgroundColour", Color.FromKnownColor(KnownColor.Control).ToArgb())!.Value);
        set => RegistryUtils.SetInt32("ViewImageBackgroundColour", value.ToArgb());
    }

    public ViewTextureEditor()
    {
        InitializeComponent();
        PBImage.BackColor = BackgroundColor;
    }

    public override void LoadChunk(TextureChunk textureChunk) => PBImage.Image = textureChunk?.GetFirstChunkOfType<ImageChunk>()?.GetImage() ?? PBImage.ErrorImage;

    private void TSMISetBackgroundColour_Click(object sender, EventArgs e)
    {
        using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
        {
            Color = BackgroundColor,
            ShowAlphaChannel = true,
            Text = $"Set Background Colour",
        };
        if (colorPicker.ShowDialog() != DialogResult.OK)
            return;

        BackgroundColor = colorPicker.Color;
        PBImage.BackColor = colorPicker.Color;
    }
}
