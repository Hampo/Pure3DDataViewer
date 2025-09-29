namespace Pure3DDataViewer;

partial class FrmFind
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFind));
        BtnFindNext = new Button();
        BtnCancel = new Button();
        TxtFind = new TextBox();
        LblFindWhat = new Label();
        GBDirection = new GroupBox();
        RBDown = new RadioButton();
        RBUp = new RadioButton();
        CBMatchCase = new CheckBox();
        CBWrapAround = new CheckBox();
        CBIncludeProperties = new CheckBox();
        GBDirection.SuspendLayout();
        SuspendLayout();
        // 
        // BtnFindNext
        // 
        BtnFindNext.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnFindNext.Enabled = false;
        BtnFindNext.Location = new Point(297, 12);
        BtnFindNext.Name = "BtnFindNext";
        BtnFindNext.Size = new Size(75, 23);
        BtnFindNext.TabIndex = 1;
        BtnFindNext.Text = "Find Next";
        BtnFindNext.UseVisualStyleBackColor = true;
        BtnFindNext.Click += BtnFindNext_Click;
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        BtnCancel.Location = new Point(297, 41);
        BtnCancel.Name = "BtnCancel";
        BtnCancel.Size = new Size(75, 23);
        BtnCancel.TabIndex = 6;
        BtnCancel.Text = "Cancel";
        BtnCancel.UseVisualStyleBackColor = true;
        BtnCancel.Click += BtnCancel_Click;
        // 
        // TxtFind
        // 
        TxtFind.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        TxtFind.Location = new Point(82, 12);
        TxtFind.Name = "TxtFind";
        TxtFind.Size = new Size(209, 23);
        TxtFind.TabIndex = 0;
        TxtFind.TextChanged += TxtFind_TextChanged;
        TxtFind.KeyDown += TxtFind_KeyDown;
        // 
        // LblFindWhat
        // 
        LblFindWhat.AutoSize = true;
        LblFindWhat.Location = new Point(12, 15);
        LblFindWhat.Name = "LblFindWhat";
        LblFindWhat.Size = new Size(64, 15);
        LblFindWhat.TabIndex = 7;
        LblFindWhat.Text = "Find What:";
        // 
        // GBDirection
        // 
        GBDirection.Controls.Add(RBDown);
        GBDirection.Controls.Add(RBUp);
        GBDirection.Location = new Point(181, 41);
        GBDirection.Name = "GBDirection";
        GBDirection.Size = new Size(110, 46);
        GBDirection.TabIndex = 5;
        GBDirection.TabStop = false;
        GBDirection.Text = "Direction";
        // 
        // RBDown
        // 
        RBDown.AutoSize = true;
        RBDown.Checked = true;
        RBDown.Location = new Point(52, 22);
        RBDown.Name = "RBDown";
        RBDown.Size = new Size(56, 19);
        RBDown.TabIndex = 1;
        RBDown.TabStop = true;
        RBDown.Text = "Down";
        RBDown.UseVisualStyleBackColor = true;
        RBDown.CheckedChanged += RBDown_CheckedChanged;
        // 
        // RBUp
        // 
        RBUp.AutoSize = true;
        RBUp.Location = new Point(6, 22);
        RBUp.Name = "RBUp";
        RBUp.Size = new Size(40, 19);
        RBUp.TabIndex = 0;
        RBUp.Text = "Up";
        RBUp.UseVisualStyleBackColor = true;
        // 
        // CBMatchCase
        // 
        CBMatchCase.AutoSize = true;
        CBMatchCase.Location = new Point(12, 41);
        CBMatchCase.Name = "CBMatchCase";
        CBMatchCase.Size = new Size(88, 19);
        CBMatchCase.TabIndex = 2;
        CBMatchCase.Text = "Match Case";
        CBMatchCase.UseVisualStyleBackColor = true;
        CBMatchCase.CheckedChanged += CBMatchCase_CheckedChanged;
        // 
        // CBWrapAround
        // 
        CBWrapAround.AutoSize = true;
        CBWrapAround.Checked = true;
        CBWrapAround.CheckState = CheckState.Checked;
        CBWrapAround.Location = new Point(12, 59);
        CBWrapAround.Name = "CBWrapAround";
        CBWrapAround.Size = new Size(97, 19);
        CBWrapAround.TabIndex = 3;
        CBWrapAround.Text = "Wrap Around";
        CBWrapAround.UseVisualStyleBackColor = true;
        CBWrapAround.CheckedChanged += CBWrapAround_CheckedChanged;
        // 
        // CBIncludeProperties
        // 
        CBIncludeProperties.AutoSize = true;
        CBIncludeProperties.Location = new Point(12, 77);
        CBIncludeProperties.Name = "CBIncludeProperties";
        CBIncludeProperties.Size = new Size(121, 19);
        CBIncludeProperties.TabIndex = 4;
        CBIncludeProperties.Text = "Include Properties";
        CBIncludeProperties.UseVisualStyleBackColor = true;
        CBIncludeProperties.CheckedChanged += CBIncludeProperties_CheckedChanged;
        // 
        // FrmFind
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(384, 99);
        Controls.Add(CBIncludeProperties);
        Controls.Add(CBWrapAround);
        Controls.Add(CBMatchCase);
        Controls.Add(GBDirection);
        Controls.Add(LblFindWhat);
        Controls.Add(TxtFind);
        Controls.Add(BtnCancel);
        Controls.Add(BtnFindNext);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        Icon = (Icon)resources.GetObject("$this.Icon");
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmFind";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Find";
        Shown += FrmFind_Shown;
        LocationChanged += FrmFind_LocationChanged;
        GBDirection.ResumeLayout(false);
        GBDirection.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private Button BtnCancel;
    private TextBox TxtFind;
    private Label LblFindWhat;
    private GroupBox GBDirection;
    private RadioButton RBDown;
    private RadioButton RBUp;
    private CheckBox CBMatchCase;
    private CheckBox CBWrapAround;
    private CheckBox CBIncludeProperties;
    public Button BtnFindNext;
}