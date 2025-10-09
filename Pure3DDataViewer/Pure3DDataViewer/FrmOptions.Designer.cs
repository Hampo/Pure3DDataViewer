namespace Pure3DDataViewer;

partial class FrmOptions
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        TCOptions = new TabControl();
        TPChunkColours = new TabPage();
        LVChunkColours = new ListView();
        CHType = new ColumnHeader();
        CHBackColour = new ColumnHeader();
        CHForeColour = new ColumnHeader();
        CMSChunkColours = new ContextMenuStrip(components);
        TSMISetBackColour = new ToolStripMenuItem();
        TSMISetForeColour = new ToolStripMenuItem();
        TSS1 = new ToolStripSeparator();
        TSMIResetColours = new ToolStripMenuItem();
        PnlButtons = new Panel();
        BtnOK = new Button();
        TCOptions.SuspendLayout();
        TPChunkColours.SuspendLayout();
        CMSChunkColours.SuspendLayout();
        PnlButtons.SuspendLayout();
        SuspendLayout();
        // 
        // TCOptions
        // 
        TCOptions.Controls.Add(TPChunkColours);
        TCOptions.Dock = DockStyle.Fill;
        TCOptions.Location = new Point(0, 0);
        TCOptions.Name = "TCOptions";
        TCOptions.SelectedIndex = 0;
        TCOptions.Size = new Size(800, 409);
        TCOptions.TabIndex = 0;
        // 
        // TPChunkColours
        // 
        TPChunkColours.Controls.Add(LVChunkColours);
        TPChunkColours.Location = new Point(4, 24);
        TPChunkColours.Name = "TPChunkColours";
        TPChunkColours.Size = new Size(792, 381);
        TPChunkColours.TabIndex = 0;
        TPChunkColours.Text = "Chunk Colours";
        TPChunkColours.UseVisualStyleBackColor = true;
        // 
        // LVChunkColours
        // 
        LVChunkColours.Activation = ItemActivation.OneClick;
        LVChunkColours.Columns.AddRange(new ColumnHeader[] { CHType, CHBackColour, CHForeColour });
        LVChunkColours.ContextMenuStrip = CMSChunkColours;
        LVChunkColours.Dock = DockStyle.Fill;
        LVChunkColours.FullRowSelect = true;
        LVChunkColours.GridLines = true;
        LVChunkColours.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        LVChunkColours.Location = new Point(0, 0);
        LVChunkColours.MultiSelect = false;
        LVChunkColours.Name = "LVChunkColours";
        LVChunkColours.Size = new Size(792, 381);
        LVChunkColours.TabIndex = 0;
        LVChunkColours.UseCompatibleStateImageBehavior = false;
        LVChunkColours.View = View.Details;
        LVChunkColours.MouseDoubleClick += LVChunkColours_MouseDoubleClick;
        // 
        // CHType
        // 
        CHType.Text = "Chunk Type";
        // 
        // CHBackColour
        // 
        CHBackColour.Text = "Back Colour";
        // 
        // CHForeColour
        // 
        CHForeColour.Text = "Fore Colour";
        // 
        // CMSChunkColours
        // 
        CMSChunkColours.Items.AddRange(new ToolStripItem[] { TSMISetBackColour, TSMISetForeColour, TSS1, TSMIResetColours });
        CMSChunkColours.Name = "CMSChunkColours";
        CMSChunkColours.Size = new Size(158, 76);
        CMSChunkColours.Opening += CMSChunkColours_Opening;
        // 
        // TSMISetBackColour
        // 
        TSMISetBackColour.Image = Properties.Resources.ColorDialog_16x;
        TSMISetBackColour.Name = "TSMISetBackColour";
        TSMISetBackColour.Size = new Size(157, 22);
        TSMISetBackColour.Text = "Set Back Colour";
        TSMISetBackColour.Click += TSMISetBackColour_Click;
        // 
        // TSMISetForeColour
        // 
        TSMISetForeColour.Image = Properties.Resources.ColorDialog_16x;
        TSMISetForeColour.Name = "TSMISetForeColour";
        TSMISetForeColour.Size = new Size(157, 22);
        TSMISetForeColour.Text = "Set Fore Colour";
        TSMISetForeColour.Click += TSMISetForeColour_Click;
        // 
        // TSS1
        // 
        TSS1.Name = "TSS1";
        TSS1.Size = new Size(154, 6);
        // 
        // TSMIResetColours
        // 
        TSMIResetColours.Image = Properties.Resources.Undo_16x;
        TSMIResetColours.Name = "TSMIResetColours";
        TSMIResetColours.Size = new Size(157, 22);
        TSMIResetColours.Text = "Reset Colours";
        TSMIResetColours.Click += TSMIResetColours_Click;
        // 
        // PnlButtons
        // 
        PnlButtons.Controls.Add(BtnOK);
        PnlButtons.Dock = DockStyle.Bottom;
        PnlButtons.Location = new Point(0, 409);
        PnlButtons.Name = "PnlButtons";
        PnlButtons.Size = new Size(800, 41);
        PnlButtons.TabIndex = 1;
        // 
        // BtnOK
        // 
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(713, 6);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 0;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // FrmOptions
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(TCOptions);
        Controls.Add(PnlButtons);
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmOptions";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Options";
        Load += FrmOptions_Load;
        TCOptions.ResumeLayout(false);
        TPChunkColours.ResumeLayout(false);
        CMSChunkColours.ResumeLayout(false);
        PnlButtons.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TabControl TCOptions;
    private TabPage TPChunkColours;
    private ListView LVChunkColours;
    private ColumnHeader CHType;
    private ColumnHeader CHBackColour;
    private ColumnHeader CHForeColour;
    private Panel PnlButtons;
    private Button BtnOK;
    private ContextMenuStrip CMSChunkColours;
    private ToolStripMenuItem TSMIResetColours;
    private ToolStripMenuItem TSMISetBackColour;
    private ToolStripMenuItem TSMISetForeColour;
    private ToolStripSeparator TSS1;
}