namespace Pure3DDataViewer.Editors;

partial class FrmKnownStringEditor
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKnownStringEditor));
        LblPropertyName = new Label();
        BtnOK = new Button();
        BtnCancel = new Button();
        CBValue = new ComboBox();
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
        BtnOK.Location = new Point(113, 55);
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
        BtnCancel.Location = new Point(194, 55);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 3;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // CBValue
        // 
        CBValue.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        CBValue.FormattingEnabled = true;
        CBValue.Location = new Point(12, 27);
        CBValue.Name = "CBValue";
        CBValue.Size = new Size(255, 23);
        CBValue.TabIndex = 4;
        // 
        // FrmKnownStringEditor
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(279, 88);
        Controls.Add(CBValue);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(LblPropertyName);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(295, 127);
        Name = "FrmKnownStringEditor";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Value";
        Shown += FrmStringEditor_Shown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblPropertyName;
    private Button BtnOK;
    private Button BtnCancel;
    private ComboBox CBValue;
}