namespace CarPhysicsObjectGenerator.Forms;

partial class FrmSelectCompositeDrawable
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
        LBCompositeDrawables = new ListBox();
        BtnCancel = new Button();
        BtnOK = new Button();
        SuspendLayout();
        // 
        // LblInfo
        // 
        LblInfo.AutoSize = true;
        LblInfo.Location = new Point(12, 9);
        LblInfo.Name = "LblInfo";
        LblInfo.Size = new Size(302, 15);
        LblInfo.TabIndex = 0;
        LblInfo.Text = "Multiple Composite Drawables found. Please select one:";
        // 
        // LBCompositeDrawables
        // 
        LBCompositeDrawables.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        LBCompositeDrawables.FormattingEnabled = true;
        LBCompositeDrawables.ItemHeight = 15;
        LBCompositeDrawables.Location = new Point(12, 27);
        LBCompositeDrawables.Name = "LBCompositeDrawables";
        LBCompositeDrawables.Size = new Size(302, 169);
        LBCompositeDrawables.TabIndex = 1;
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(239, 205);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 7;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // BtnOK
        // 
        BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(158, 205);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 6;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // FrmSelectCompositeDrawable
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(326, 240);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(LBCompositeDrawables);
        Controls.Add(LblInfo);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmSelectCompositeDrawable";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Select Composite Drawable";
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Label LblInfo;
    private ListBox LBCompositeDrawables;
    private Button BtnCancel;
    private Button BtnOK;
}