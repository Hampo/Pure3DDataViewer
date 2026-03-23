namespace CompositeDrawableEditor.Editors.Controls;

partial class CompositeDrawableEditor
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
        TLPMain = new TableLayoutPanel();
        GBEffectList = new GroupBox();
        DGVEffectList = new DataGridView();
        dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
        dataGridViewCheckBoxColumn3 = new DataGridViewCheckBoxColumn();
        dataGridViewCheckBoxColumn4 = new DataGridViewCheckBoxColumn();
        dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
        ColumnSortOrder3 = new DataGridViewTextBoxColumn();
        GBPropList = new GroupBox();
        DGVPropList = new DataGridView();
        dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
        dataGridViewCheckBoxColumn1 = new DataGridViewCheckBoxColumn();
        dataGridViewCheckBoxColumn2 = new DataGridViewCheckBoxColumn();
        ColumnSkeletonJointIndex = new DataGridViewTextBoxColumn();
        ColumnSortOrder2 = new DataGridViewTextBoxColumn();
        GBSkinList = new GroupBox();
        DGVSkinList = new DataGridView();
        ColumnName = new DataGridViewTextBoxColumn();
        ColumnIncluded = new DataGridViewCheckBoxColumn();
        ColumnTranslucent = new DataGridViewCheckBoxColumn();
        ColumnSortOrder = new DataGridViewTextBoxColumn();
        PnlSkeletonName = new Panel();
        CBSkeletonName = new ComboBox();
        LblSkeletonName = new Label();
        PnlName = new Panel();
        TxtName = new TextBox();
        LblName = new Label();
        TLPMain.SuspendLayout();
        GBEffectList.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DGVEffectList).BeginInit();
        GBPropList.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DGVPropList).BeginInit();
        GBSkinList.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)DGVSkinList).BeginInit();
        PnlSkeletonName.SuspendLayout();
        PnlName.SuspendLayout();
        SuspendLayout();
        // 
        // TLPMain
        // 
        TLPMain.ColumnCount = 1;
        TLPMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        TLPMain.Controls.Add(GBEffectList, 0, 4);
        TLPMain.Controls.Add(GBPropList, 0, 3);
        TLPMain.Controls.Add(GBSkinList, 0, 2);
        TLPMain.Controls.Add(PnlSkeletonName, 0, 1);
        TLPMain.Controls.Add(PnlName, 0, 0);
        TLPMain.Dock = DockStyle.Fill;
        TLPMain.Location = new Point(0, 0);
        TLPMain.Name = "TLPMain";
        TLPMain.RowCount = 5;
        TLPMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        TLPMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        TLPMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.335556F));
        TLPMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.332222F));
        TLPMain.RowStyles.Add(new RowStyle(SizeType.Percent, 33.332222F));
        TLPMain.Size = new Size(448, 420);
        TLPMain.TabIndex = 0;
        // 
        // GBEffectList
        // 
        GBEffectList.Controls.Add(DGVEffectList);
        GBEffectList.Dock = DockStyle.Fill;
        GBEffectList.Location = new Point(3, 302);
        GBEffectList.Name = "GBEffectList";
        GBEffectList.Size = new Size(442, 115);
        GBEffectList.TabIndex = 2;
        GBEffectList.TabStop = false;
        GBEffectList.Text = "Effect List";
        // 
        // DGVEffectList
        // 
        DGVEffectList.AllowUserToAddRows = false;
        DGVEffectList.AllowUserToDeleteRows = false;
        DGVEffectList.AllowUserToResizeRows = false;
        DGVEffectList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DGVEffectList.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn2, dataGridViewCheckBoxColumn3, dataGridViewCheckBoxColumn4, dataGridViewTextBoxColumn3, ColumnSortOrder3 });
        DGVEffectList.Dock = DockStyle.Fill;
        DGVEffectList.Location = new Point(3, 19);
        DGVEffectList.MultiSelect = false;
        DGVEffectList.Name = "DGVEffectList";
        DGVEffectList.RowHeadersVisible = false;
        DGVEffectList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGVEffectList.ShowCellToolTips = false;
        DGVEffectList.Size = new Size(436, 93);
        DGVEffectList.TabIndex = 2;
        DGVEffectList.CellValidating += DGV_CellValidating;
        DGVEffectList.CellValueChanged += DGVEffectList_CellValueChanged;
        DGVEffectList.CurrentCellDirtyStateChanged += DGV_CurrentCellDirtyStateChanged;
        DGVEffectList.EditingControlShowing += DGV_EditingControlShowing;
        // 
        // dataGridViewTextBoxColumn2
        // 
        dataGridViewTextBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewTextBoxColumn2.Frozen = true;
        dataGridViewTextBoxColumn2.HeaderText = "Name";
        dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
        dataGridViewTextBoxColumn2.ReadOnly = true;
        dataGridViewTextBoxColumn2.Width = 64;
        // 
        // dataGridViewCheckBoxColumn3
        // 
        dataGridViewCheckBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewCheckBoxColumn3.HeaderText = "Included";
        dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
        dataGridViewCheckBoxColumn3.Width = 59;
        // 
        // dataGridViewCheckBoxColumn4
        // 
        dataGridViewCheckBoxColumn4.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewCheckBoxColumn4.HeaderText = "Translucent";
        dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
        dataGridViewCheckBoxColumn4.Width = 73;
        // 
        // dataGridViewTextBoxColumn3
        // 
        dataGridViewTextBoxColumn3.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewTextBoxColumn3.HeaderText = "Skeleton Joint Index";
        dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
        dataGridViewTextBoxColumn3.Width = 99;
        // 
        // ColumnSortOrder3
        // 
        ColumnSortOrder3.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        ColumnSortOrder3.HeaderText = "Sort Order";
        ColumnSortOrder3.Name = "ColumnSortOrder3";
        // 
        // GBPropList
        // 
        GBPropList.Controls.Add(DGVPropList);
        GBPropList.Dock = DockStyle.Fill;
        GBPropList.Location = new Point(3, 183);
        GBPropList.Name = "GBPropList";
        GBPropList.Size = new Size(442, 113);
        GBPropList.TabIndex = 1;
        GBPropList.TabStop = false;
        GBPropList.Text = "Prop List";
        // 
        // DGVPropList
        // 
        DGVPropList.AllowUserToAddRows = false;
        DGVPropList.AllowUserToDeleteRows = false;
        DGVPropList.AllowUserToResizeRows = false;
        DGVPropList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DGVPropList.Columns.AddRange(new DataGridViewColumn[] { dataGridViewTextBoxColumn1, dataGridViewCheckBoxColumn1, dataGridViewCheckBoxColumn2, ColumnSkeletonJointIndex, ColumnSortOrder2 });
        DGVPropList.Dock = DockStyle.Fill;
        DGVPropList.Location = new Point(3, 19);
        DGVPropList.MultiSelect = false;
        DGVPropList.Name = "DGVPropList";
        DGVPropList.RowHeadersVisible = false;
        DGVPropList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGVPropList.ShowCellToolTips = false;
        DGVPropList.Size = new Size(436, 91);
        DGVPropList.TabIndex = 1;
        DGVPropList.CellValidating += DGV_CellValidating;
        DGVPropList.CellValueChanged += DGVPropList_CellValueChanged;
        DGVPropList.CurrentCellDirtyStateChanged += DGV_CurrentCellDirtyStateChanged;
        DGVPropList.EditingControlShowing += DGV_EditingControlShowing;
        // 
        // dataGridViewTextBoxColumn1
        // 
        dataGridViewTextBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewTextBoxColumn1.Frozen = true;
        dataGridViewTextBoxColumn1.HeaderText = "Name";
        dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
        dataGridViewTextBoxColumn1.ReadOnly = true;
        dataGridViewTextBoxColumn1.Width = 64;
        // 
        // dataGridViewCheckBoxColumn1
        // 
        dataGridViewCheckBoxColumn1.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewCheckBoxColumn1.HeaderText = "Included";
        dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
        dataGridViewCheckBoxColumn1.Width = 59;
        // 
        // dataGridViewCheckBoxColumn2
        // 
        dataGridViewCheckBoxColumn2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        dataGridViewCheckBoxColumn2.HeaderText = "Translucent";
        dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
        dataGridViewCheckBoxColumn2.Width = 73;
        // 
        // ColumnSkeletonJointIndex
        // 
        ColumnSkeletonJointIndex.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        ColumnSkeletonJointIndex.HeaderText = "Skeleton Joint Index";
        ColumnSkeletonJointIndex.Name = "ColumnSkeletonJointIndex";
        ColumnSkeletonJointIndex.Width = 99;
        // 
        // ColumnSortOrder2
        // 
        ColumnSortOrder2.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        ColumnSortOrder2.HeaderText = "Sort Order";
        ColumnSortOrder2.Name = "ColumnSortOrder2";
        // 
        // GBSkinList
        // 
        GBSkinList.Controls.Add(DGVSkinList);
        GBSkinList.Dock = DockStyle.Fill;
        GBSkinList.Location = new Point(3, 63);
        GBSkinList.Name = "GBSkinList";
        GBSkinList.Size = new Size(442, 114);
        GBSkinList.TabIndex = 0;
        GBSkinList.TabStop = false;
        GBSkinList.Text = "Skin List";
        // 
        // DGVSkinList
        // 
        DGVSkinList.AllowUserToAddRows = false;
        DGVSkinList.AllowUserToDeleteRows = false;
        DGVSkinList.AllowUserToResizeRows = false;
        DGVSkinList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        DGVSkinList.Columns.AddRange(new DataGridViewColumn[] { ColumnName, ColumnIncluded, ColumnTranslucent, ColumnSortOrder });
        DGVSkinList.Dock = DockStyle.Fill;
        DGVSkinList.Location = new Point(3, 19);
        DGVSkinList.MultiSelect = false;
        DGVSkinList.Name = "DGVSkinList";
        DGVSkinList.RowHeadersVisible = false;
        DGVSkinList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        DGVSkinList.ShowCellToolTips = false;
        DGVSkinList.Size = new Size(436, 92);
        DGVSkinList.TabIndex = 0;
        DGVSkinList.CellValidating += DGV_CellValidating;
        DGVSkinList.CellValueChanged += DGVSkinList_CellValueChanged;
        DGVSkinList.CurrentCellDirtyStateChanged += DGV_CurrentCellDirtyStateChanged;
        DGVSkinList.EditingControlShowing += DGV_EditingControlShowing;
        // 
        // ColumnName
        // 
        ColumnName.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        ColumnName.Frozen = true;
        ColumnName.HeaderText = "Name";
        ColumnName.Name = "ColumnName";
        ColumnName.ReadOnly = true;
        ColumnName.Width = 64;
        // 
        // ColumnIncluded
        // 
        ColumnIncluded.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        ColumnIncluded.HeaderText = "Included";
        ColumnIncluded.Name = "ColumnIncluded";
        ColumnIncluded.Width = 59;
        // 
        // ColumnTranslucent
        // 
        ColumnTranslucent.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        ColumnTranslucent.HeaderText = "Translucent";
        ColumnTranslucent.Name = "ColumnTranslucent";
        ColumnTranslucent.Width = 73;
        // 
        // ColumnSortOrder
        // 
        ColumnSortOrder.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        ColumnSortOrder.HeaderText = "Sort Order";
        ColumnSortOrder.Name = "ColumnSortOrder";
        // 
        // PnlSkeletonName
        // 
        PnlSkeletonName.Controls.Add(CBSkeletonName);
        PnlSkeletonName.Controls.Add(LblSkeletonName);
        PnlSkeletonName.Dock = DockStyle.Fill;
        PnlSkeletonName.Location = new Point(3, 33);
        PnlSkeletonName.Name = "PnlSkeletonName";
        PnlSkeletonName.Size = new Size(442, 24);
        PnlSkeletonName.TabIndex = 4;
        // 
        // CBSkeletonName
        // 
        CBSkeletonName.Dock = DockStyle.Fill;
        CBSkeletonName.FormattingEnabled = true;
        CBSkeletonName.Location = new Point(92, 0);
        CBSkeletonName.Name = "CBSkeletonName";
        CBSkeletonName.Size = new Size(350, 23);
        CBSkeletonName.TabIndex = 3;
        CBSkeletonName.Leave += CBSkeletonName_Leave;
        // 
        // LblSkeletonName
        // 
        LblSkeletonName.Dock = DockStyle.Left;
        LblSkeletonName.Location = new Point(0, 0);
        LblSkeletonName.Name = "LblSkeletonName";
        LblSkeletonName.Size = new Size(92, 24);
        LblSkeletonName.TabIndex = 2;
        LblSkeletonName.Text = "Skeleton Name:";
        LblSkeletonName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // PnlName
        // 
        PnlName.Controls.Add(TxtName);
        PnlName.Controls.Add(LblName);
        PnlName.Dock = DockStyle.Fill;
        PnlName.Location = new Point(3, 3);
        PnlName.Name = "PnlName";
        PnlName.Size = new Size(442, 24);
        PnlName.TabIndex = 3;
        // 
        // TxtName
        // 
        TxtName.Dock = DockStyle.Fill;
        TxtName.Location = new Point(92, 0);
        TxtName.MaxLength = 255;
        TxtName.Name = "TxtName";
        TxtName.Size = new Size(350, 23);
        TxtName.TabIndex = 1;
        TxtName.Leave += TxtName_Leave;
        // 
        // LblName
        // 
        LblName.Dock = DockStyle.Left;
        LblName.Location = new Point(0, 0);
        LblName.Name = "LblName";
        LblName.Size = new Size(92, 24);
        LblName.TabIndex = 0;
        LblName.Text = "Name:";
        LblName.TextAlign = ContentAlignment.MiddleRight;
        // 
        // CompositeDrawableEditor
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(TLPMain);
        Name = "CompositeDrawableEditor";
        Size = new Size(448, 420);
        TLPMain.ResumeLayout(false);
        GBEffectList.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DGVEffectList).EndInit();
        GBPropList.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DGVPropList).EndInit();
        GBSkinList.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)DGVSkinList).EndInit();
        PnlSkeletonName.ResumeLayout(false);
        PnlName.ResumeLayout(false);
        PnlName.PerformLayout();
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel TLPMain;
    private GroupBox GBEffectList;
    private GroupBox GBPropList;
    private GroupBox GBSkinList;
    private DataGridView DGVSkinList;
    private DataGridView DGVEffectList;
    private DataGridView DGVPropList;
    private Panel PnlName;
    private Panel PnlSkeletonName;
    private Label LblSkeletonName;
    private TextBox TxtName;
    private Label LblName;
    private DataGridViewTextBoxColumn ColumnName;
    private DataGridViewCheckBoxColumn ColumnIncluded;
    private DataGridViewCheckBoxColumn ColumnTranslucent;
    private DataGridViewTextBoxColumn ColumnSortOrder;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
    private DataGridViewTextBoxColumn ColumnSortOrder3;
    private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    private DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
    private DataGridViewTextBoxColumn ColumnSkeletonJointIndex;
    private DataGridViewTextBoxColumn ColumnSortOrder2;
    private ComboBox CBSkeletonName;
}
