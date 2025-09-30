using Pure3DDataViewerPluginAPI.Extensions;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Controls;
using NetP3DLib.P3D;

namespace ImportExportImages.Editors.Controls;
public partial class ViewImageEditor : EditorControl
{
    public static Color BackgroundColor
    {
        get => Color.FromArgb(RegistryUtils.GetInt32("ViewImageBackgroundColour", Color.FromKnownColor(KnownColor.Control).ToArgb())!.Value);
        set => RegistryUtils.SetInt32("ViewImageBackgroundColour", value.ToArgb());
    }

    public ViewImageEditor()
    {
        InitializeComponent();
        PBImage.BackColor = BackgroundColor;
    }

    public override void LoadChunk(Chunk chunk)
    {
        var image = chunk switch
        {
            ImageChunk imageChunk => imageChunk.GetImage(),
            TextureChunk textureChunk => textureChunk.GetFirstChunkOfType<ImageChunk>()?.GetImage(),
            _ => throw new NotSupportedException($"{nameof(ViewImageEditor)} does not support chunks of type {chunk.GetType()}")
        };
        PBImage.Image = image ?? PBImage.ErrorImage;
    }

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
