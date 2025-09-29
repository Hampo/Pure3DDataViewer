using Pure3DDataViewerPluginAPI.Controls;

namespace Pure3DDataViewerPluginAPI.Editors;

partial class FrmBooleanEditor
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBooleanEditor));
        BtnOK = new Button();
        BtnCancel = new Button();
        CBValue = new CheckBox();
        SuspendLayout();
        // 
        // BtnOK
        // 
        BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(113, 33);
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
        BtnCancel.Location = new Point(194, 33);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 3;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        // 
        // CBValue
        // 
        CBValue.AutoSize = true;
        CBValue.Location = new Point(12, 12);
        CBValue.Name = "CBValue";
        CBValue.Size = new Size(106, 19);
        CBValue.TabIndex = 4;
        CBValue.Text = "Property Name";
        CBValue.UseVisualStyleBackColor = true;
        // 
        // FrmBooleanEditor
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(279, 65);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(CBValue);
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(295, 104);
        Name = "FrmBooleanEditor";
        ShowIcon = false;
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Edit Value";
        Shown += FrmBooleanEditor_Shown;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button BtnOK;
    private Button BtnCancel;
    private CheckBox CBValue;
}