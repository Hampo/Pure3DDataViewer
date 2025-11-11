namespace TimeOfDayTint.Forms;

partial class FrmTintOptions
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
        GBCurrentTint = new GroupBox();
        PnlCurrent = new Panel();
        CBCurrentUseCustom = new CheckBox();
        CBCurrentTimeOfDay = new ComboBox();
        LblCurrentTimeOfDay = new Label();
        GBNewTint = new GroupBox();
        PnlNew = new Panel();
        CBNewUseCustom = new CheckBox();
        CBNewTimeOfDay = new ComboBox();
        LblNewTimeOfDay = new Label();
        GBModifiers = new GroupBox();
        LblBrightness = new Label();
        LblBlend = new Label();
        NUDBrightness = new NumericUpDown();
        NUDBlend = new NumericUpDown();
        GBCurrentTint.SuspendLayout();
        GBNewTint.SuspendLayout();
        GBModifiers.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NUDBrightness).BeginInit();
        ((System.ComponentModel.ISupportInitialize)NUDBlend).BeginInit();
        SuspendLayout();
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(401, 343);
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
        BtnOK.Location = new Point(320, 343);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 6;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        // 
        // GBCurrentTint
        // 
        GBCurrentTint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GBCurrentTint.Controls.Add(PnlCurrent);
        GBCurrentTint.Controls.Add(CBCurrentUseCustom);
        GBCurrentTint.Controls.Add(CBCurrentTimeOfDay);
        GBCurrentTint.Controls.Add(LblCurrentTimeOfDay);
        GBCurrentTint.Location = new Point(12, 12);
        GBCurrentTint.Name = "GBCurrentTint";
        GBCurrentTint.Size = new Size(464, 116);
        GBCurrentTint.TabIndex = 8;
        GBCurrentTint.TabStop = false;
        GBCurrentTint.Text = "Current Tint";
        // 
        // PnlCurrent
        // 
        PnlCurrent.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        PnlCurrent.Cursor = Cursors.Hand;
        PnlCurrent.Enabled = false;
        PnlCurrent.Location = new Point(128, 51);
        PnlCurrent.Name = "PnlCurrent";
        PnlCurrent.Size = new Size(330, 59);
        PnlCurrent.TabIndex = 3;
        PnlCurrent.Click += PnlCurrent_Click;
        // 
        // CBCurrentUseCustom
        // 
        CBCurrentUseCustom.AutoSize = true;
        CBCurrentUseCustom.Location = new Point(6, 51);
        CBCurrentUseCustom.Name = "CBCurrentUseCustom";
        CBCurrentUseCustom.Size = new Size(90, 19);
        CBCurrentUseCustom.TabIndex = 2;
        CBCurrentUseCustom.Text = "Use Custom";
        CBCurrentUseCustom.UseVisualStyleBackColor = true;
        CBCurrentUseCustom.CheckedChanged += CBCurrentUseCustom_CheckedChanged;
        // 
        // CBCurrentTimeOfDay
        // 
        CBCurrentTimeOfDay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CBCurrentTimeOfDay.DropDownStyle = ComboBoxStyle.DropDownList;
        CBCurrentTimeOfDay.FormattingEnabled = true;
        CBCurrentTimeOfDay.Location = new Point(128, 22);
        CBCurrentTimeOfDay.Name = "CBCurrentTimeOfDay";
        CBCurrentTimeOfDay.Size = new Size(330, 23);
        CBCurrentTimeOfDay.TabIndex = 1;
        CBCurrentTimeOfDay.SelectedValueChanged += CBCurrentTimeOfDay_SelectedValueChanged;
        // 
        // LblCurrentTimeOfDay
        // 
        LblCurrentTimeOfDay.AutoSize = true;
        LblCurrentTimeOfDay.Location = new Point(6, 25);
        LblCurrentTimeOfDay.Name = "LblCurrentTimeOfDay";
        LblCurrentTimeOfDay.Size = new Size(116, 15);
        LblCurrentTimeOfDay.TabIndex = 0;
        LblCurrentTimeOfDay.Text = "Current Time of Day:";
        // 
        // GBNewTint
        // 
        GBNewTint.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GBNewTint.Controls.Add(PnlNew);
        GBNewTint.Controls.Add(CBNewUseCustom);
        GBNewTint.Controls.Add(CBNewTimeOfDay);
        GBNewTint.Controls.Add(LblNewTimeOfDay);
        GBNewTint.Location = new Point(12, 134);
        GBNewTint.Name = "GBNewTint";
        GBNewTint.Size = new Size(464, 116);
        GBNewTint.TabIndex = 9;
        GBNewTint.TabStop = false;
        GBNewTint.Text = "New Tint";
        // 
        // PnlNew
        // 
        PnlNew.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        PnlNew.Cursor = Cursors.Hand;
        PnlNew.Enabled = false;
        PnlNew.Location = new Point(128, 51);
        PnlNew.Name = "PnlNew";
        PnlNew.Size = new Size(330, 59);
        PnlNew.TabIndex = 3;
        PnlNew.Click += PnlNew_Click;
        // 
        // CBNewUseCustom
        // 
        CBNewUseCustom.AutoSize = true;
        CBNewUseCustom.Location = new Point(6, 51);
        CBNewUseCustom.Name = "CBNewUseCustom";
        CBNewUseCustom.Size = new Size(90, 19);
        CBNewUseCustom.TabIndex = 2;
        CBNewUseCustom.Text = "Use Custom";
        CBNewUseCustom.UseVisualStyleBackColor = true;
        CBNewUseCustom.CheckedChanged += CBNewUseCustom_CheckedChanged;
        // 
        // CBNewTimeOfDay
        // 
        CBNewTimeOfDay.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        CBNewTimeOfDay.DropDownStyle = ComboBoxStyle.DropDownList;
        CBNewTimeOfDay.FormattingEnabled = true;
        CBNewTimeOfDay.Location = new Point(128, 22);
        CBNewTimeOfDay.Name = "CBNewTimeOfDay";
        CBNewTimeOfDay.Size = new Size(330, 23);
        CBNewTimeOfDay.TabIndex = 1;
        CBNewTimeOfDay.SelectedValueChanged += CBNewTimeOfDay_SelectedValueChanged;
        // 
        // LblNewTimeOfDay
        // 
        LblNewTimeOfDay.AutoSize = true;
        LblNewTimeOfDay.Location = new Point(22, 25);
        LblNewTimeOfDay.Name = "LblNewTimeOfDay";
        LblNewTimeOfDay.Size = new Size(100, 15);
        LblNewTimeOfDay.TabIndex = 0;
        LblNewTimeOfDay.Text = "New Time of Day:";
        // 
        // GBModifiers
        // 
        GBModifiers.Controls.Add(LblBrightness);
        GBModifiers.Controls.Add(LblBlend);
        GBModifiers.Controls.Add(NUDBrightness);
        GBModifiers.Controls.Add(NUDBlend);
        GBModifiers.Location = new Point(12, 256);
        GBModifiers.Name = "GBModifiers";
        GBModifiers.Size = new Size(464, 81);
        GBModifiers.TabIndex = 10;
        GBModifiers.TabStop = false;
        GBModifiers.Text = "Modifiers";
        // 
        // LblBrightness
        // 
        LblBrightness.AutoSize = true;
        LblBrightness.Location = new Point(57, 53);
        LblBrightness.Name = "LblBrightness";
        LblBrightness.Size = new Size(65, 15);
        LblBrightness.TabIndex = 3;
        LblBrightness.Text = "Brightness:";
        // 
        // LblBlend
        // 
        LblBlend.AutoSize = true;
        LblBlend.Location = new Point(82, 24);
        LblBlend.Name = "LblBlend";
        LblBlend.Size = new Size(40, 15);
        LblBlend.TabIndex = 2;
        LblBlend.Text = "Blend:";
        // 
        // NUDBrightness
        // 
        NUDBrightness.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NUDBrightness.DecimalPlaces = 2;
        NUDBrightness.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
        NUDBrightness.Location = new Point(128, 51);
        NUDBrightness.Maximum = new decimal(new int[] { 255, 0, 0, 0 });
        NUDBrightness.Name = "NUDBrightness";
        NUDBrightness.Size = new Size(330, 23);
        NUDBrightness.TabIndex = 1;
        NUDBrightness.Value = new decimal(new int[] { 8, 0, 0, 65536 });
        // 
        // NUDBlend
        // 
        NUDBlend.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        NUDBlend.DecimalPlaces = 2;
        NUDBlend.Increment = new decimal(new int[] { 5, 0, 0, 131072 });
        NUDBlend.Location = new Point(128, 22);
        NUDBlend.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
        NUDBlend.Name = "NUDBlend";
        NUDBlend.Size = new Size(330, 23);
        NUDBlend.TabIndex = 0;
        NUDBlend.Value = new decimal(new int[] { 75, 0, 0, 131072 });
        // 
        // FrmTintOptions
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(488, 378);
        Controls.Add(GBModifiers);
        Controls.Add(GBNewTint);
        Controls.Add(GBCurrentTint);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "FrmTintOptions";
        ShowIcon = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tint Options";
        Load += FrmTintOptions_Load;
        GBCurrentTint.ResumeLayout(false);
        GBCurrentTint.PerformLayout();
        GBNewTint.ResumeLayout(false);
        GBNewTint.PerformLayout();
        GBModifiers.ResumeLayout(false);
        GBModifiers.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)NUDBrightness).EndInit();
        ((System.ComponentModel.ISupportInitialize)NUDBlend).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private Button BtnCancel;
    private Button BtnOK;
    private GroupBox GBCurrentTint;
    private ComboBox CBCurrentTimeOfDay;
    private Label LblCurrentTimeOfDay;
    private CheckBox CBCurrentUseCustom;
    private Panel PnlCurrent;
    private GroupBox GBNewTint;
    private Panel PnlNew;
    private CheckBox CBNewUseCustom;
    private ComboBox CBNewTimeOfDay;
    private Label LblNewTimeOfDay;
    private GroupBox GBModifiers;
    private NumericUpDown NUDBrightness;
    private NumericUpDown NUDBlend;
    private Label LblBrightness;
    private Label LblBlend;
}