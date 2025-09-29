namespace FrontendTextBibleEditor.Editors.Controls;

partial class FrontendTextBibleEditor
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        GBEntry = new GroupBox();
        CBEntry = new ComboBox();
        BtnUpdate = new Button();
        GBValues = new GroupBox();
        LVValues = new ListView();
        CHLanguage = new ColumnHeader();
        CHValue = new ColumnHeader();
        GBEntry.SuspendLayout();
        GBValues.SuspendLayout();
        SuspendLayout();
        // 
        // GBEntry
        // 
        GBEntry.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        GBEntry.Controls.Add(CBEntry);
        GBEntry.Location = new Point(3, 3);
        GBEntry.Name = "GBEntry";
        GBEntry.Size = new Size(357, 46);
        GBEntry.TabIndex = 1;
        GBEntry.TabStop = false;
        GBEntry.Text = "Entry";
        // 
        // CBEntry
        // 
        CBEntry.Dock = DockStyle.Top;
        CBEntry.DropDownStyle = ComboBoxStyle.DropDownList;
        CBEntry.FormattingEnabled = true;
        CBEntry.Location = new Point(3, 19);
        CBEntry.Name = "CBEntry";
        CBEntry.Size = new Size(351, 23);
        CBEntry.TabIndex = 0;
        CBEntry.SelectedIndexChanged += CBEntry_SelectedIndexChanged;
        // 
        // BtnUpdate
        // 
        BtnUpdate.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        BtnUpdate.Location = new Point(285, 330);
        BtnUpdate.Name = "BtnUpdate";
        BtnUpdate.Size = new Size(75, 23);
        BtnUpdate.TabIndex = 2;
        BtnUpdate.Text = "Update";
        BtnUpdate.UseVisualStyleBackColor = true;
        BtnUpdate.Click += BtnUpdate_Click;
        // 
        // GBValues
        // 
        GBValues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        GBValues.Controls.Add(LVValues);
        GBValues.Location = new Point(3, 55);
        GBValues.Name = "GBValues";
        GBValues.Size = new Size(357, 269);
        GBValues.TabIndex = 3;
        GBValues.TabStop = false;
        GBValues.Text = "Values";
        // 
        // LVValues
        // 
        LVValues.Activation = ItemActivation.OneClick;
        LVValues.Columns.AddRange(new ColumnHeader[] { CHLanguage, CHValue });
        LVValues.Dock = DockStyle.Fill;
        LVValues.FullRowSelect = true;
        LVValues.GridLines = true;
        LVValues.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        LVValues.Location = new Point(3, 19);
        LVValues.MultiSelect = false;
        LVValues.Name = "LVValues";
        LVValues.Size = new Size(351, 247);
        LVValues.TabIndex = 0;
        LVValues.UseCompatibleStateImageBehavior = false;
        LVValues.View = View.Details;
        LVValues.MouseDoubleClick += LVValues_MouseDoubleClick;
        // 
        // CHLanguage
        // 
        CHLanguage.Text = "Language";
        // 
        // CHValue
        // 
        CHValue.Text = "Value";
        // 
        // FrontendTextBibleEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(GBValues);
        Controls.Add(BtnUpdate);
        Controls.Add(GBEntry);
        Name = "FrontendTextBibleEditor";
        Size = new Size(363, 356);
        GBEntry.ResumeLayout(false);
        GBValues.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion
    private GroupBox GBEntry;
    private ComboBox CBEntry;
    private Button BtnUpdate;
    private GroupBox GBValues;
    private ListView LVValues;
    private ColumnHeader CHLanguage;
    private ColumnHeader CHValue;
}
