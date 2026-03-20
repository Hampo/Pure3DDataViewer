namespace ImportExportImages.Editors.Controls;

partial class ViewImageEditor
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
        TSMISizeMode = new ToolStripMenuItem();
        TSMISizeModeNormal = new ToolStripMenuItem();
        TSMISizeModeZoom = new ToolStripMenuItem();
        TSMISizeModeCenterImage = new ToolStripMenuItem();
        TSMISizeModeStretchImage = new ToolStripMenuItem();
        PnlPB = new Panel();
        ((System.ComponentModel.ISupportInitialize)PBImage).BeginInit();
        CMSPBImage.SuspendLayout();
        PnlPB.SuspendLayout();
        SuspendLayout();
        // 
        // PBImage
        // 
        PBImage.ContextMenuStrip = CMSPBImage;
        PBImage.Location = new Point(0, 0);
        PBImage.Name = "PBImage";
        PBImage.Size = new Size(150, 150);
        PBImage.SizeMode = PictureBoxSizeMode.AutoSize;
        PBImage.TabIndex = 0;
        PBImage.TabStop = false;
        PBImage.SizeModeChanged += PBImage_SizeModeChanged;
        PBImage.MouseDown += PBImage_MouseDown;
        PBImage.MouseMove += PBImage_MouseMove;
        PBImage.MouseUp += PBImage_MouseUp;
        // 
        // CMSPBImage
        // 
        CMSPBImage.Items.AddRange(new ToolStripItem[] { TSMISetBackgroundColour, TSMISizeMode });
        CMSPBImage.Name = "CMSPBImage";
        CMSPBImage.Size = new Size(197, 70);
        // 
        // TSMISetBackgroundColour
        // 
        TSMISetBackgroundColour.Image = Properties.Resources.BackgroundColor_16x;
        TSMISetBackgroundColour.Name = "TSMISetBackgroundColour";
        TSMISetBackgroundColour.Size = new Size(196, 22);
        TSMISetBackgroundColour.Text = "Set Background Colour";
        TSMISetBackgroundColour.Click += TSMISetBackgroundColour_Click;
        // 
        // TSMISizeMode
        // 
        TSMISizeMode.DropDownItems.AddRange(new ToolStripItem[] { TSMISizeModeNormal, TSMISizeModeZoom, TSMISizeModeCenterImage, TSMISizeModeStretchImage });
        TSMISizeMode.Image = Properties.Resources.ImageScale_16x;
        TSMISizeMode.Name = "TSMISizeMode";
        TSMISizeMode.Size = new Size(196, 22);
        TSMISizeMode.Text = "Size Mode";
        // 
        // TSMISizeModeNormal
        // 
        TSMISizeModeNormal.Checked = true;
        TSMISizeModeNormal.CheckOnClick = true;
        TSMISizeModeNormal.CheckState = CheckState.Checked;
        TSMISizeModeNormal.Name = "TSMISizeModeNormal";
        TSMISizeModeNormal.Size = new Size(180, 22);
        TSMISizeModeNormal.Text = "Normal";
        TSMISizeModeNormal.CheckedChanged += TSMISizeModeNormal_CheckedChanged;
        // 
        // TSMISizeModeZoom
        // 
        TSMISizeModeZoom.CheckOnClick = true;
        TSMISizeModeZoom.Name = "TSMISizeModeZoom";
        TSMISizeModeZoom.Size = new Size(180, 22);
        TSMISizeModeZoom.Text = "Zoom";
        TSMISizeModeZoom.CheckedChanged += TSMISizeModeZoom_CheckedChanged;
        // 
        // TSMISizeModeCenterImage
        // 
        TSMISizeModeCenterImage.CheckOnClick = true;
        TSMISizeModeCenterImage.Name = "TSMISizeModeCenterImage";
        TSMISizeModeCenterImage.Size = new Size(180, 22);
        TSMISizeModeCenterImage.Text = "Center Image";
        TSMISizeModeCenterImage.Visible = false;
        TSMISizeModeCenterImage.CheckedChanged += TSMISizeModeCenterImage_CheckedChanged;
        // 
        // TSMISizeModeStretchImage
        // 
        TSMISizeModeStretchImage.CheckOnClick = true;
        TSMISizeModeStretchImage.Name = "TSMISizeModeStretchImage";
        TSMISizeModeStretchImage.Size = new Size(180, 22);
        TSMISizeModeStretchImage.Text = "Stretch Image";
        TSMISizeModeStretchImage.CheckedChanged += TSMISizeModeStretchImage_CheckedChanged;
        // 
        // PnlPB
        // 
        PnlPB.AutoScroll = true;
        PnlPB.ContextMenuStrip = CMSPBImage;
        PnlPB.Controls.Add(PBImage);
        PnlPB.Dock = DockStyle.Fill;
        PnlPB.Location = new Point(0, 0);
        PnlPB.Name = "PnlPB";
        PnlPB.Size = new Size(150, 150);
        PnlPB.TabIndex = 1;
        PnlPB.Resize += PnlPB_Resize;
        // 
        // ViewImageEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackgroundImage = Properties.Resources.Transparent_16x;
        Controls.Add(PnlPB);
        Name = "ViewImageEditor";
        ((System.ComponentModel.ISupportInitialize)PBImage).EndInit();
        CMSPBImage.ResumeLayout(false);
        PnlPB.ResumeLayout(false);
        PnlPB.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private PictureBox PBImage;
    private ContextMenuStrip CMSPBImage;
    private ToolStripMenuItem TSMISetBackgroundColour;
    private ToolStripMenuItem TSMISizeMode;
    private ToolStripMenuItem TSMISizeModeNormal;
    private ToolStripMenuItem TSMISizeModeZoom;
    private ToolStripMenuItem TSMISizeModeCenterImage;
    private ToolStripMenuItem TSMISizeModeStretchImage;
    private Panel PnlPB;
}
