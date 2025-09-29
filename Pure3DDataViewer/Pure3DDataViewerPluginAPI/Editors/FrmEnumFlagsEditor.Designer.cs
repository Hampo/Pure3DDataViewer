namespace Pure3DDataViewerPluginAPI.Editors;

partial class FrmEnumFlagsEditor
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEnumFlagsEditor));
        LblPropertyName = new Label();
        BtnOK = new Button();
        BtnCancel = new Button();
        CLBValues = new CheckedListBox();
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
        BtnOK.Location = new Point(113, 167);
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
        BtnCancel.Location = new Point(194, 167);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 3;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // CLBValues
        // 
        CLBValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        CLBValues.FormattingEnabled = true;
        CLBValues.Location = new Point(12, 27);
        CLBValues.Name = "CLBValues";
        CLBValues.Size = new Size(255, 130);
        CLBValues.TabIndex = 4;
        // 
        // FrmEnumFlagsEditor
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(279, 200);
        Controls.Add(CLBValues);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(LblPropertyName);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(295, 131);
        Name = "FrmEnumFlagsEditor";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Value";
        Shown += FrmEnumFlagsEditor_Shown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblPropertyName;
    private Button BtnOK;
    private Button BtnCancel;
    private CheckedListBox CLBValues;
}