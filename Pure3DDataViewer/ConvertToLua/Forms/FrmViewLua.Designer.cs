namespace ConvertToLua.Forms;

partial class FrmViewLua
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
        BtnOK = new Button();
        BtnCopy = new Button();
        PBProgress = new ProgressBar();
        TxtLua = new RichTextBox();
        SuspendLayout();
        // 
        // BtnOK
        // 
        BtnOK.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnOK.DialogResult = DialogResult.OK;
        BtnOK.Location = new Point(713, 415);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 0;
        BtnOK.Text = "Ok";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // BtnCopy
        // 
        BtnCopy.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCopy.Enabled = false;
        BtnCopy.Location = new Point(632, 415);
        BtnCopy.Name = "BtnCopy";
        BtnCopy.Size = new Size(75, 23);
        BtnCopy.TabIndex = 1;
        BtnCopy.Text = "Copy";
        BtnCopy.UseVisualStyleBackColor = true;
        BtnCopy.Click += BtnCopy_Click;
        // 
        // PBProgress
        // 
        PBProgress.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        PBProgress.Location = new Point(12, 415);
        PBProgress.Name = "PBProgress";
        PBProgress.Size = new Size(614, 23);
        PBProgress.TabIndex = 3;
        // 
        // TxtLua
        // 
        TxtLua.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        TxtLua.BackColor = SystemColors.Window;
        TxtLua.Enabled = false;
        TxtLua.Location = new Point(0, 0);
        TxtLua.Name = "TxtLua";
        TxtLua.ReadOnly = true;
        TxtLua.Size = new Size(800, 409);
        TxtLua.TabIndex = 4;
        TxtLua.Text = "";
        // 
        // FrmViewLua
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(PBProgress);
        Controls.Add(TxtLua);
        Controls.Add(BtnCopy);
        Controls.Add(BtnOK);
        MinimizeBox = false;
        Name = "FrmViewLua";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Converted Lua";
        Shown += FrmViewLua_Shown;
        ResumeLayout(false);
    }

    #endregion

    private Button BtnOK;
    private Button BtnCopy;
    private ProgressBar PBProgress;
    private RichTextBox TxtLua;
}