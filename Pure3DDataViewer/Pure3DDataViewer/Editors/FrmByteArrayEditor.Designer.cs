namespace Pure3DDataViewer.Editors;

partial class FrmByteArrayEditor
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmByteArrayEditor));
        LblPropertyName = new Label();
        BtnOK = new Button();
        BtnCancel = new Button();
        HBValue = new Be.Windows.Forms.HexBox();
        BtnImport = new Button();
        BtnExport = new Button();
        SuspendLayout();
        // 
        // LblPropertyName
        // 
        LblPropertyName.AutoSize = true;
        LblPropertyName.Location = new Point(12, 9);
        LblPropertyName.Name = "LblPropertyName";
        LblPropertyName.Size = new Size(87, 15);
        LblPropertyName.TabIndex = 0;
        LblPropertyName.Text = "Property Name";
        // 
        // BtnOK
        // 
        BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(213, 328);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 2;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(294, 328);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 3;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // HBValue
        // 
        HBValue.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        // 
        // 
        // 
        HBValue.BuiltInContextMenu.CopyMenuItemText = "Copy";
        HBValue.BuiltInContextMenu.CutMenuItemText = "Cut";
        HBValue.BuiltInContextMenu.PasteMenuItemText = "Paste";
        HBValue.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
        HBValue.ColumnInfoVisible = true;
        HBValue.Font = new Font("Segoe UI", 9F);
        HBValue.LineInfoVisible = true;
        HBValue.Location = new Point(12, 27);
        HBValue.Name = "HBValue";
        HBValue.ShadowSelectionColor = Color.FromArgb(100, 60, 188, 255);
        HBValue.Size = new Size(357, 295);
        HBValue.StringViewVisible = true;
        HBValue.TabIndex = 4;
        HBValue.VScrollBarVisible = true;
        // 
        // BtnImport
        // 
        BtnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnImport.Location = new Point(12, 328);
        BtnImport.Name = "BtnImport";
        BtnImport.Size = new Size(75, 23);
        BtnImport.TabIndex = 5;
        BtnImport.Text = "Import";
        BtnImport.UseVisualStyleBackColor = true;
        BtnImport.Click += BtnImport_Click;
        // 
        // BtnExport
        // 
        BtnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        BtnExport.Location = new Point(93, 328);
        BtnExport.Name = "BtnExport";
        BtnExport.Size = new Size(75, 23);
        BtnExport.TabIndex = 6;
        BtnExport.Text = "Export";
        BtnExport.UseVisualStyleBackColor = true;
        BtnExport.Click += BtnExport_Click;
        // 
        // FrmByteArrayEditor
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(379, 361);
        Controls.Add(BtnExport);
        Controls.Add(BtnImport);
        Controls.Add(HBValue);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(LblPropertyName);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(395, 175);
        Name = "FrmByteArrayEditor";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Value";
        Shown += FrmByteArrayEditor_Shown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblPropertyName;
    private Button BtnOK;
    private Button BtnCancel;
    private Be.Windows.Forms.HexBox HBValue;
    private Button BtnImport;
    private Button BtnExport;
}