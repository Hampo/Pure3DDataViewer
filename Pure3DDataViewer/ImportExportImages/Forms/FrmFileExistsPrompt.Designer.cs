namespace ImportExportImages.Forms;

partial class FrmFileExistsPrompt
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
        LblInfo = new Label();
        BtnOverwrite = new Button();
        BtnKeepBoth = new Button();
        BtnKeepOriginal = new Button();
        CBApplyToAll = new CheckBox();
        SuspendLayout();
        // 
        // LblInfo
        // 
        LblInfo.Location = new Point(12, 9);
        LblInfo.Name = "LblInfo";
        LblInfo.Size = new Size(333, 73);
        LblInfo.TabIndex = 0;
        LblInfo.Text = "The following file already exists:\r\n";
        // 
        // BtnOverwrite
        // 
        BtnOverwrite.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOverwrite.DialogResult = DialogResult.OK;
        BtnOverwrite.Location = new Point(104, 85);
        BtnOverwrite.Name = "BtnOverwrite";
        BtnOverwrite.Size = new Size(90, 23);
        BtnOverwrite.TabIndex = 1;
        BtnOverwrite.Text = "Overwrite";
        BtnOverwrite.UseVisualStyleBackColor = true;
        BtnOverwrite.Click += BtnOverwrite_Click;
        // 
        // BtnKeepBoth
        // 
        BtnKeepBoth.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnKeepBoth.DialogResult = DialogResult.OK;
        BtnKeepBoth.Location = new Point(200, 85);
        BtnKeepBoth.Name = "BtnKeepBoth";
        BtnKeepBoth.Size = new Size(90, 23);
        BtnKeepBoth.TabIndex = 2;
        BtnKeepBoth.Text = "Keep Both";
        BtnKeepBoth.UseVisualStyleBackColor = true;
        BtnKeepBoth.Click += BtnKeepBoth_Click;
        // 
        // BtnKeepOriginal
        // 
        BtnKeepOriginal.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnKeepOriginal.DialogResult = DialogResult.Cancel;
        BtnKeepOriginal.Location = new Point(296, 85);
        BtnKeepOriginal.Name = "BtnKeepOriginal";
        BtnKeepOriginal.Size = new Size(90, 23);
        BtnKeepOriginal.TabIndex = 3;
        BtnKeepOriginal.Text = "Keep Original";
        BtnKeepOriginal.UseVisualStyleBackColor = true;
        BtnKeepOriginal.Click += BtnKeepOriginal_Click;
        // 
        // CBApplyToAll
        // 
        CBApplyToAll.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        CBApplyToAll.AutoSize = true;
        CBApplyToAll.Location = new Point(12, 88);
        CBApplyToAll.Name = "CBApplyToAll";
        CBApplyToAll.Size = new Size(88, 19);
        CBApplyToAll.TabIndex = 4;
        CBApplyToAll.Text = "Apply to All";
        CBApplyToAll.UseVisualStyleBackColor = true;
        // 
        // FrmFileExistsPrompt
        // 
        AcceptButton = BtnOverwrite;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnKeepOriginal;
        ClientSize = new Size(398, 120);
        Controls.Add(CBApplyToAll);
        Controls.Add(BtnKeepOriginal);
        Controls.Add(BtnKeepBoth);
        Controls.Add(BtnOverwrite);
        Controls.Add(LblInfo);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmFileExistsPrompt";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "File Exists";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblInfo;
    private Button BtnOverwrite;
    private Button BtnKeepBoth;
    private Button BtnKeepOriginal;
    private CheckBox CBApplyToAll;
}