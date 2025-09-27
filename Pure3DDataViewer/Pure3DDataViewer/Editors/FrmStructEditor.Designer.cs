namespace Pure3DDataViewer.Editors;

partial class FrmStructEditor
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStructEditor));
        BtnCancel = new Button();
        BtnOK = new Button();
        TLP1 = new TableLayoutPanel();
        SuspendLayout();
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(713, 415);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 5;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // BtnOK
        // 
        BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(632, 415);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 4;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        BtnOK.Click += BtnOK_Click;
        // 
        // TLP1
        // 
        TLP1.AutoSize = true;
        TLP1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        TLP1.ColumnCount = 2;
        TLP1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
        TLP1.ColumnStyles.Add(new ColumnStyle());
        TLP1.Location = new Point(3, 3);
        TLP1.Name = "TLP1";
        TLP1.RowCount = 1;
        TLP1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        TLP1.Size = new Size(20, 0);
        TLP1.TabIndex = 6;
        // 
        // FrmStructEditor
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(800, 450);
        Controls.Add(TLP1);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmStructEditor";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Values";
        Shown += FrmStructEditor_Shown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button BtnCancel;
    private Button BtnOK;
    private TableLayoutPanel TLP1;
}