namespace Deduplicate.Forms;

partial class FrmConfigureAllowedDuplicates
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
        BtnCancel = new Button();
        BtnOK = new Button();
        LBDisallowedTypes = new ListBox();
        LBAllowedTypes = new ListBox();
        BtnAdd = new Button();
        BtnRemove = new Button();
        LblDisallowed = new Label();
        LblAllowed = new Label();
        SuspendLayout();
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(530, 292);
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
        BtnOK.Location = new Point(449, 292);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 4;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // LBDisallowedTypes
        // 
        LBDisallowedTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        LBDisallowedTypes.FormattingEnabled = true;
        LBDisallowedTypes.ItemHeight = 15;
        LBDisallowedTypes.Location = new Point(12, 27);
        LBDisallowedTypes.Name = "LBDisallowedTypes";
        LBDisallowedTypes.SelectionMode = SelectionMode.MultiExtended;
        LBDisallowedTypes.Size = new Size(278, 259);
        LBDisallowedTypes.TabIndex = 6;
        LBDisallowedTypes.Format += LBTypes_Format;
        // 
        // LBAllowedTypes
        // 
        LBAllowedTypes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        LBAllowedTypes.FormattingEnabled = true;
        LBAllowedTypes.ItemHeight = 15;
        LBAllowedTypes.Location = new Point(327, 27);
        LBAllowedTypes.Name = "LBAllowedTypes";
        LBAllowedTypes.SelectionMode = SelectionMode.MultiExtended;
        LBAllowedTypes.Size = new Size(278, 259);
        LBAllowedTypes.TabIndex = 7;
        LBAllowedTypes.Format += LBTypes_Format;
        // 
        // BtnAdd
        // 
        BtnAdd.Anchor = AnchorStyles.None;
        BtnAdd.Location = new Point(296, 128);
        BtnAdd.Name = "BtnAdd";
        BtnAdd.Size = new Size(25, 23);
        BtnAdd.TabIndex = 8;
        BtnAdd.Text = ">";
        BtnAdd.UseVisualStyleBackColor = true;
        BtnAdd.Click += BtnAdd_Click;
        // 
        // BtnRemove
        // 
        BtnRemove.Anchor = AnchorStyles.None;
        BtnRemove.Location = new Point(296, 158);
        BtnRemove.Name = "BtnRemove";
        BtnRemove.Size = new Size(25, 23);
        BtnRemove.TabIndex = 9;
        BtnRemove.Text = "<";
        BtnRemove.UseVisualStyleBackColor = true;
        BtnRemove.Click += BtnRemove_Click;
        // 
        // LblDisallowed
        // 
        LblDisallowed.AutoSize = true;
        LblDisallowed.Location = new Point(12, 9);
        LblDisallowed.Name = "LblDisallowed";
        LblDisallowed.Size = new Size(67, 15);
        LblDisallowed.TabIndex = 10;
        LblDisallowed.Text = "Disallowed:";
        // 
        // LblAllowed
        // 
        LblAllowed.AutoSize = true;
        LblAllowed.Location = new Point(327, 9);
        LblAllowed.Name = "LblAllowed";
        LblAllowed.Size = new Size(53, 15);
        LblAllowed.TabIndex = 11;
        LblAllowed.Text = "Allowed:";
        // 
        // FrmConfigureAllowedDuplicates
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(617, 327);
        Controls.Add(LblAllowed);
        Controls.Add(LblDisallowed);
        Controls.Add(BtnRemove);
        Controls.Add(BtnAdd);
        Controls.Add(LBAllowedTypes);
        Controls.Add(LBDisallowedTypes);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        MaximizeBox = false;
        MaximumSize = new Size(633, 9999);
        MinimizeBox = false;
        MinimumSize = new Size(633, 190);
        Name = "FrmConfigureAllowedDuplicates";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Configure Allowed Duplicates";
        Load += FrmConfigureAllowedDuplicates_Load;
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button BtnCancel;
    private Button BtnOK;
    private ListBox LBDisallowedTypes;
    private ListBox LBAllowedTypes;
    private Button BtnAdd;
    private Button BtnRemove;
    private Label LblDisallowed;
    private Label LblAllowed;
}