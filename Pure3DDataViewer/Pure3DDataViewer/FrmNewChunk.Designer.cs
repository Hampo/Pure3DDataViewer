namespace Pure3DDataViewer;

partial class FrmNewChunk
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmNewChunk));
        BtnCancel = new Button();
        BtnOK = new Button();
        GBChunkType = new GroupBox();
        CBChunkType = new ComboBox();
        GBValues = new GroupBox();
        LVValues = new ListView();
        CHName = new ColumnHeader();
        CHValue = new ColumnHeader();
        GBLocatorType = new GroupBox();
        CBLocatorType = new ComboBox();
        LblCreateX = new Label();
        NUDCreateX = new NumericUpDown();
        GBChunkType.SuspendLayout();
        GBValues.SuspendLayout();
        GBLocatorType.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)NUDCreateX).BeginInit();
        SuspendLayout();
        // 
        // BtnCancel
        // 
        BtnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnCancel.DialogResult = DialogResult.Cancel;
        BtnCancel.Location = new Point(521, 299);
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
        BtnOK.Location = new Point(440, 299);
        BtnOK.Name = "BtnOK";
        BtnOK.Size = new Size(75, 23);
        BtnOK.TabIndex = 4;
        BtnOK.Text = "OK";
        BtnOK.UseVisualStyleBackColor = true;
        BtnOK.Click += BtnOK_Click;
        // 
        // GBChunkType
        // 
        GBChunkType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GBChunkType.Controls.Add(CBChunkType);
        GBChunkType.Location = new Point(12, 12);
        GBChunkType.Name = "GBChunkType";
        GBChunkType.Size = new Size(584, 46);
        GBChunkType.TabIndex = 6;
        GBChunkType.TabStop = false;
        GBChunkType.Text = "Chunk Type";
        // 
        // CBChunkType
        // 
        CBChunkType.Dock = DockStyle.Fill;
        CBChunkType.DropDownStyle = ComboBoxStyle.DropDownList;
        CBChunkType.FormattingEnabled = true;
        CBChunkType.Location = new Point(3, 19);
        CBChunkType.Name = "CBChunkType";
        CBChunkType.Size = new Size(578, 23);
        CBChunkType.TabIndex = 0;
        CBChunkType.SelectedValueChanged += CBChunkType_SelectedValueChanged;
        // 
        // GBValues
        // 
        GBValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        GBValues.Controls.Add(LVValues);
        GBValues.Location = new Point(12, 116);
        GBValues.Name = "GBValues";
        GBValues.Size = new Size(584, 177);
        GBValues.TabIndex = 7;
        GBValues.TabStop = false;
        GBValues.Text = "Values";
        // 
        // LVValues
        // 
        LVValues.Activation = ItemActivation.OneClick;
        LVValues.Columns.AddRange(new ColumnHeader[] { CHName, CHValue });
        LVValues.Dock = DockStyle.Fill;
        LVValues.FullRowSelect = true;
        LVValues.GridLines = true;
        LVValues.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        LVValues.Location = new Point(3, 19);
        LVValues.MultiSelect = false;
        LVValues.Name = "LVValues";
        LVValues.Size = new Size(578, 155);
        LVValues.TabIndex = 0;
        LVValues.UseCompatibleStateImageBehavior = false;
        LVValues.View = View.Details;
        LVValues.MouseDoubleClick += LVValues_MouseDoubleClick;
        // 
        // CHName
        // 
        CHName.Text = "Name";
        // 
        // CHValue
        // 
        CHValue.Text = "Value";
        // 
        // GBLocatorType
        // 
        GBLocatorType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GBLocatorType.Controls.Add(CBLocatorType);
        GBLocatorType.Location = new Point(12, 64);
        GBLocatorType.Name = "GBLocatorType";
        GBLocatorType.Size = new Size(584, 46);
        GBLocatorType.TabIndex = 7;
        GBLocatorType.TabStop = false;
        GBLocatorType.Text = "Locator Type";
        // 
        // CBLocatorType
        // 
        CBLocatorType.Dock = DockStyle.Fill;
        CBLocatorType.DropDownStyle = ComboBoxStyle.DropDownList;
        CBLocatorType.FormattingEnabled = true;
        CBLocatorType.Location = new Point(3, 19);
        CBLocatorType.Name = "CBLocatorType";
        CBLocatorType.Size = new Size(578, 23);
        CBLocatorType.TabIndex = 0;
        // 
        // LblCreateX
        // 
        LblCreateX.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        LblCreateX.AutoSize = true;
        LblCreateX.Location = new Point(328, 303);
        LblCreateX.Name = "LblCreateX";
        LblCreateX.Size = new Size(54, 15);
        LblCreateX.TabIndex = 8;
        LblCreateX.Text = "Create X:";
        // 
        // NUDCreateX
        // 
        NUDCreateX.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        NUDCreateX.Location = new Point(388, 299);
        NUDCreateX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        NUDCreateX.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        NUDCreateX.Name = "NUDCreateX";
        NUDCreateX.Size = new Size(46, 23);
        NUDCreateX.TabIndex = 9;
        NUDCreateX.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // FrmNewChunk
        // 
        AcceptButton = BtnOK;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = BtnCancel;
        ClientSize = new Size(608, 334);
        Controls.Add(NUDCreateX);
        Controls.Add(LblCreateX);
        Controls.Add(GBValues);
        Controls.Add(GBChunkType);
        Controls.Add(BtnCancel);
        Controls.Add(BtnOK);
        Controls.Add(GBLocatorType);
        Icon = (Icon)resources.GetObject("$this.Icon");
        Name = "FrmNewChunk";
        StartPosition = FormStartPosition.CenterParent;
        Text = "New Chunk";
        Load += FrmNewChunk_Load;
        GBChunkType.ResumeLayout(false);
        GBValues.ResumeLayout(false);
        GBLocatorType.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)NUDCreateX).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private Button BtnCancel;
    private Button BtnOK;
    private GroupBox GBChunkType;
    private ComboBox CBChunkType;
    private GroupBox GBValues;
    private ListView LVValues;
    private ColumnHeader CHName;
    private ColumnHeader CHValue;
    private GroupBox GBLocatorType;
    private ComboBox CBLocatorType;
    private Label LblCreateX;
    private NumericUpDown NUDCreateX;
}