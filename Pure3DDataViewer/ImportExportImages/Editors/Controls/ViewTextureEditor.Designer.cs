namespace ImportExportImages.Editors.Controls;

partial class ViewTextureEditor
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        PBImage = new PictureBox();
        CMSPBImage = new ContextMenuStrip(components);
        TSMISetBackgroundColour = new ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)PBImage).BeginInit();
        CMSPBImage.SuspendLayout();
        SuspendLayout();
        // 
        // PBImage
        // 
        PBImage.ContextMenuStrip = CMSPBImage;
        PBImage.Dock = DockStyle.Fill;
        PBImage.Location = new Point(0, 0);
        PBImage.Name = "PBImage";
        PBImage.Size = new Size(150, 150);
        PBImage.TabIndex = 0;
        PBImage.TabStop = false;
        // 
        // CMSPBImage
        // 
        CMSPBImage.Items.AddRange(new ToolStripItem[] { TSMISetBackgroundColour });
        CMSPBImage.Name = "CMSPBImage";
        CMSPBImage.Size = new Size(197, 48);
        // 
        // TSMISetBackgroundColour
        // 
        TSMISetBackgroundColour.Image = Properties.Resources.BackgroundColor_16x;
        TSMISetBackgroundColour.Name = "TSMISetBackgroundColour";
        TSMISetBackgroundColour.Size = new Size(196, 22);
        TSMISetBackgroundColour.Text = "Set Background Colour";
        TSMISetBackgroundColour.Click += TSMISetBackgroundColour_Click;
        // 
        // ViewImageEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(PBImage);
        Name = "ViewImageEditor";
        ((System.ComponentModel.ISupportInitialize)PBImage).EndInit();
        CMSPBImage.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private PictureBox PBImage;
    private ContextMenuStrip CMSPBImage;
    private ToolStripMenuItem TSMISetBackgroundColour;
}
