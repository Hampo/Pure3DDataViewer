using Pure3DDataViewerPluginAPI.Extensions;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI;
using Pure3DDataViewerPluginAPI.Controls;
using NetP3DLib.P3D;

namespace ImportExportImages.Editors.Controls;
public partial class ViewImageEditor : EditorControl
{
    private static Color BackgroundColor
    {
        get => Color.FromArgb(RegistryUtils.GetInt32("ViewImageBackgroundColour", Color.FromKnownColor(KnownColor.Control).ToArgb())!.Value);
        set => RegistryUtils.SetInt32("ViewImageBackgroundColour", value.ToArgb());
    }

    private static PictureBoxSizeMode SizeMode
    {
        get => (PictureBoxSizeMode)RegistryUtils.GetInt32("ViewImageSizeMode", (int)PictureBoxSizeMode.Normal)!;
        set => RegistryUtils.SetInt32("ViewImageSizeMode", (int)value);
    }

    public ViewImageEditor()
    {
        InitializeComponent();
        PBImage.BackColor = BackgroundColor;
        PBImage.SizeMode = SizeMode;
        switch (PBImage.SizeMode)
        {
            case PictureBoxSizeMode.Normal:
                TSMISizeModeNormal.Checked = true;
                break;
            case PictureBoxSizeMode.Zoom:
                TSMISizeModeZoom.Checked = true;
                break;
            case PictureBoxSizeMode.CenterImage:
                TSMISizeModeCenterImage.Checked = true;
                break;
            case PictureBoxSizeMode.StretchImage:
                TSMISizeModeStretchImage.Checked = true;
                break;
            default:
                TSMISizeModeNormal.Checked = true;
                break;
        }
    }

    public override void LoadChunk(Chunk chunk)
    {
        var image = chunk switch
        {
            ImageDataChunk imageDataChunk => (imageDataChunk.ParentChunk as ImageChunk)?.GetImage(),
            ImageChunk imageChunk => imageChunk.GetImage(),
            TextureChunk textureChunk => textureChunk.GetFirstChunkOfType<ImageChunk>()?.GetImage(),
            SpriteChunk spriteChunk => spriteChunk.GetImage(),
            _ => throw new NotSupportedException($"{nameof(ViewImageEditor)} does not support chunks of type {chunk.GetType()}")
        };
        PBImage.Image = image ?? PBImage.ErrorImage;
        PBImage.BackColor = BackgroundColor;
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

    private void TSMISizeModeNormal_CheckedChanged(object sender, EventArgs e)
    {
        if (!TSMISizeModeNormal.Checked)
            return;

        TSMISizeModeZoom.Checked = false;
        TSMISizeModeCenterImage.Checked = false;
        TSMISizeModeStretchImage.Checked = false;

        PBImage.SizeMode = PictureBoxSizeMode.Normal;
        SizeMode = PBImage.SizeMode;
    }

    private void TSMISizeModeZoom_CheckedChanged(object sender, EventArgs e)
    {
        if (!TSMISizeModeZoom.Checked)
            return;

        TSMISizeModeNormal.Checked = false;
        TSMISizeModeCenterImage.Checked = false;
        TSMISizeModeStretchImage.Checked = false;

        PBImage.SizeMode = PictureBoxSizeMode.Zoom;
        SizeMode = PBImage.SizeMode;
    }

    private void TSMISizeModeCenterImage_CheckedChanged(object sender, EventArgs e)
    {
        if (!TSMISizeModeCenterImage.Checked)
            return;

        TSMISizeModeZoom.Checked = false;
        TSMISizeModeNormal.Checked = false;
        TSMISizeModeStretchImage.Checked = false;

        PBImage.SizeMode = PictureBoxSizeMode.CenterImage;
        SizeMode = PBImage.SizeMode;
    }

    private void TSMISizeModeStretchImage_CheckedChanged(object sender, EventArgs e)
    {
        if (!TSMISizeModeStretchImage.Checked)
            return;

        TSMISizeModeZoom.Checked = false;
        TSMISizeModeCenterImage.Checked = false;
        TSMISizeModeNormal.Checked = false;

        PBImage.SizeMode = PictureBoxSizeMode.StretchImage;
        SizeMode = PBImage.SizeMode;
    }
}
