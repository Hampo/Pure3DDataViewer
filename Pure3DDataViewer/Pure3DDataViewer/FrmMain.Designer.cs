using Pure3DDataViewerPluginAPI.Controls;

namespace Pure3DDataViewer;

partial class FrmMain
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
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
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
        SC1 = new SplitContainer();
        TVChunks = new ExplorerThemedTreeView();
        CMSTVChunks = new ContextMenuStrip(components);
        TSMINewChunk2 = new ToolStripMenuItem();
        TSS5 = new ToolStripSeparator();
        TSMICut2 = new ToolStripMenuItem();
        TSMICopy2 = new ToolStripMenuItem();
        TSMICopyThis2 = new ToolStripMenuItem();
        TSMICopyChildren2 = new ToolStripMenuItem();
        TSMICopyType2 = new ToolStripMenuItem();
        TSMIPasteBefore2 = new ToolStripMenuItem();
        TSMIPasteAfter2 = new ToolStripMenuItem();
        TSMIPasteInside2 = new ToolStripMenuItem();
        TSMIDuplicate2 = new ToolStripMenuItem();
        TSS6 = new ToolStripSeparator();
        TSMIDelete2 = new ToolStripMenuItem();
        TSMIDeleteThis2 = new ToolStripMenuItem();
        TSMIDeleteType2 = new ToolStripMenuItem();
        TSMIDeleteChildren2 = new ToolStripMenuItem();
        TSMIRename2 = new ToolStripMenuItem();
        TSS8 = new ToolStripSeparator();
        TCEditors = new TabControl();
        TPValues = new TabPage();
        LVValues = new ListView();
        CHName = new ColumnHeader();
        CHValue = new ColumnHeader();
        TPHex = new TabPage();
        HBHex = new Be.Windows.Forms.HexBox();
        MS1 = new MenuStrip();
        TSMIFile = new ToolStripMenuItem();
        TSMINew = new ToolStripMenuItem();
        TSMIOpen = new ToolStripMenuItem();
        TSMISave = new ToolStripMenuItem();
        TSMISaveAs = new ToolStripMenuItem();
        TSS9 = new ToolStripSeparator();
        TSMIEndianness = new ToolStripMenuItem();
        TSMILittleEndian = new ToolStripMenuItem();
        TSMIBigEndian = new ToolStripMenuItem();
        TSMICompressed = new ToolStripMenuItem();
        TSS1 = new ToolStripSeparator();
        TSMIRecentFiles = new ToolStripMenuItem();
        dummyToolStripMenuItem = new ToolStripMenuItem();
        TSS2 = new ToolStripSeparator();
        TSMIExit = new ToolStripMenuItem();
        TSMIEdit = new ToolStripMenuItem();
        TSMIUndo = new ToolStripMenuItem();
        TSMIRedo = new ToolStripMenuItem();
        TSS10 = new ToolStripSeparator();
        TSMINewChunk1 = new ToolStripMenuItem();
        TSS4 = new ToolStripSeparator();
        TSMICut1 = new ToolStripMenuItem();
        TSMICopy1 = new ToolStripMenuItem();
        TSMICopyThis1 = new ToolStripMenuItem();
        TSMICopyChildren1 = new ToolStripMenuItem();
        TSMICopyType1 = new ToolStripMenuItem();
        TSMIPasteBefore1 = new ToolStripMenuItem();
        TSMIPasteAfter1 = new ToolStripMenuItem();
        TSMIPasteInside1 = new ToolStripMenuItem();
        TSMIDuplicate1 = new ToolStripMenuItem();
        TSS3 = new ToolStripSeparator();
        TSMIDelete1 = new ToolStripMenuItem();
        TSMIDeleteThisForced = new ToolStripMenuItem();
        TSMIDeleteThis1 = new ToolStripMenuItem();
        TSMIDeleteType1 = new ToolStripMenuItem();
        TSMIDeleteChildren1 = new ToolStripMenuItem();
        TSMIRename1 = new ToolStripMenuItem();
        TSS7 = new ToolStripSeparator();
        TSMIFind = new ToolStripMenuItem();
        TSMIFindNext = new ToolStripMenuItem();
        TSMITools = new ToolStripMenuItem();
        TSMIHelp = new ToolStripMenuItem();
        TSMIAbout = new ToolStripMenuItem();
        TSMIOptions = new ToolStripMenuItem();
        TSS11 = new ToolStripSeparator();
        ((System.ComponentModel.ISupportInitialize)SC1).BeginInit();
        SC1.Panel1.SuspendLayout();
        SC1.Panel2.SuspendLayout();
        SC1.SuspendLayout();
        CMSTVChunks.SuspendLayout();
        TCEditors.SuspendLayout();
        TPValues.SuspendLayout();
        TPHex.SuspendLayout();
        MS1.SuspendLayout();
        SuspendLayout();
        // 
        // SC1
        // 
        SC1.Dock = DockStyle.Fill;
        SC1.Location = new Point(0, 24);
        SC1.Margin = new Padding(4, 3, 4, 3);
        SC1.Name = "SC1";
        // 
        // SC1.Panel1
        // 
        SC1.Panel1.Controls.Add(TVChunks);
        // 
        // SC1.Panel2
        // 
        SC1.Panel2.Controls.Add(TCEditors);
        SC1.Size = new Size(933, 495);
        SC1.SplitterDistance = 464;
        SC1.SplitterWidth = 5;
        SC1.TabIndex = 0;
        SC1.SplitterMoving += SC1_SplitterMoving;
        SC1.Resize += SC1_Resize;
        // 
        // TVChunks
        // 
        TVChunks.AllowDrop = true;
        TVChunks.ContextMenuStrip = CMSTVChunks;
        TVChunks.Dock = DockStyle.Fill;
        TVChunks.FullRowSelect = true;
        TVChunks.Location = new Point(0, 0);
        TVChunks.Margin = new Padding(4, 3, 4, 3);
        TVChunks.Name = "TVChunks";
        TVChunks.ShowLines = false;
        TVChunks.Size = new Size(464, 495);
        TVChunks.TabIndex = 0;
        TVChunks.AfterSelect += TVChunks_AfterSelect;
        TVChunks.NodeMouseClick += TVChunks_NodeMouseClick;
        TVChunks.DragDrop += TVChunks_DragDrop;
        TVChunks.DragEnter += TVChunks_DragEnter;
        // 
        // CMSTVChunks
        // 
        CMSTVChunks.Items.AddRange(new ToolStripItem[] { TSMINewChunk2, TSS5, TSMICut2, TSMICopy2, TSMIPasteBefore2, TSMIPasteAfter2, TSMIPasteInside2, TSMIDuplicate2, TSS6, TSMIDelete2, TSMIRename2, TSS8 });
        CMSTVChunks.Name = "CMSTVChunks";
        CMSTVChunks.Size = new Size(140, 220);
        CMSTVChunks.Opening += CMSTVChunks_Opening;
        // 
        // TSMINewChunk2
        // 
        TSMINewChunk2.Image = Properties.Resources.NewItem_16x;
        TSMINewChunk2.Name = "TSMINewChunk2";
        TSMINewChunk2.Size = new Size(139, 22);
        TSMINewChunk2.Text = "New Chunk";
        TSMINewChunk2.Click += TSMINewChunk_Click;
        // 
        // TSS5
        // 
        TSS5.Name = "TSS5";
        TSS5.Size = new Size(136, 6);
        // 
        // TSMICut2
        // 
        TSMICut2.Image = Properties.Resources.Cut_16x;
        TSMICut2.Name = "TSMICut2";
        TSMICut2.Size = new Size(139, 22);
        TSMICut2.Text = "Cut";
        TSMICut2.Click += TSMICut_Click;
        // 
        // TSMICopy2
        // 
        TSMICopy2.DropDownItems.AddRange(new ToolStripItem[] { TSMICopyThis2, TSMICopyChildren2, TSMICopyType2 });
        TSMICopy2.Image = Properties.Resources.Copy_16x;
        TSMICopy2.Name = "TSMICopy2";
        TSMICopy2.Size = new Size(139, 22);
        TSMICopy2.Text = "Copy";
        TSMICopy2.Click += TSMICopyThis_Click;
        // 
        // TSMICopyThis2
        // 
        TSMICopyThis2.Name = "TSMICopyThis2";
        TSMICopyThis2.Size = new Size(122, 22);
        TSMICopyThis2.Text = "This";
        TSMICopyThis2.Click += TSMICopyThis_Click;
        // 
        // TSMICopyChildren2
        // 
        TSMICopyChildren2.Name = "TSMICopyChildren2";
        TSMICopyChildren2.Size = new Size(122, 22);
        TSMICopyChildren2.Text = "Children";
        TSMICopyChildren2.Click += TSMICopyChildren_Click;
        // 
        // TSMICopyType2
        // 
        TSMICopyType2.Name = "TSMICopyType2";
        TSMICopyType2.Size = new Size(122, 22);
        TSMICopyType2.Text = "This Type";
        TSMICopyType2.Click += TSMICopyType_Click;
        // 
        // TSMIPasteBefore2
        // 
        TSMIPasteBefore2.Image = Properties.Resources.Paste_16x;
        TSMIPasteBefore2.Name = "TSMIPasteBefore2";
        TSMIPasteBefore2.Size = new Size(139, 22);
        TSMIPasteBefore2.Text = "Paste Before";
        TSMIPasteBefore2.Click += TSMIPasteBefore_Click;
        // 
        // TSMIPasteAfter2
        // 
        TSMIPasteAfter2.Image = Properties.Resources.Paste_16x;
        TSMIPasteAfter2.Name = "TSMIPasteAfter2";
        TSMIPasteAfter2.Size = new Size(139, 22);
        TSMIPasteAfter2.Text = "Paste After";
        TSMIPasteAfter2.Click += TSMIPasteAfter_Click;
        // 
        // TSMIPasteInside2
        // 
        TSMIPasteInside2.Image = Properties.Resources.PasteAppend_16x;
        TSMIPasteInside2.Name = "TSMIPasteInside2";
        TSMIPasteInside2.Size = new Size(139, 22);
        TSMIPasteInside2.Text = "Paste Inside";
        TSMIPasteInside2.Click += TSMIPasteInside_Click;
        // 
        // TSMIDuplicate2
        // 
        TSMIDuplicate2.Image = Properties.Resources.Copy_16x;
        TSMIDuplicate2.Name = "TSMIDuplicate2";
        TSMIDuplicate2.Size = new Size(139, 22);
        TSMIDuplicate2.Text = "Duplicate";
        TSMIDuplicate2.Click += TSMIDuplicate_Click;
        // 
        // TSS6
        // 
        TSS6.Name = "TSS6";
        TSS6.Size = new Size(136, 6);
        // 
        // TSMIDelete2
        // 
        TSMIDelete2.DropDownItems.AddRange(new ToolStripItem[] { TSMIDeleteThis2, TSMIDeleteType2, TSMIDeleteChildren2 });
        TSMIDelete2.Image = Properties.Resources.Close_16x;
        TSMIDelete2.Name = "TSMIDelete2";
        TSMIDelete2.Size = new Size(139, 22);
        TSMIDelete2.Text = "Delete";
        // 
        // TSMIDeleteThis2
        // 
        TSMIDeleteThis2.Name = "TSMIDeleteThis2";
        TSMIDeleteThis2.Size = new Size(122, 22);
        TSMIDeleteThis2.Text = "This";
        TSMIDeleteThis2.Click += TSMIDeleteThis_Click;
        // 
        // TSMIDeleteType2
        // 
        TSMIDeleteType2.Name = "TSMIDeleteType2";
        TSMIDeleteType2.Size = new Size(122, 22);
        TSMIDeleteType2.Text = "This Type";
        TSMIDeleteType2.Click += TSMIDeleteType_Click;
        // 
        // TSMIDeleteChildren2
        // 
        TSMIDeleteChildren2.Name = "TSMIDeleteChildren2";
        TSMIDeleteChildren2.Size = new Size(122, 22);
        TSMIDeleteChildren2.Text = "Children";
        TSMIDeleteChildren2.Click += TSMIDeleteChildren_Click;
        // 
        // TSMIRename2
        // 
        TSMIRename2.Image = Properties.Resources.Rename_16x;
        TSMIRename2.Name = "TSMIRename2";
        TSMIRename2.Size = new Size(139, 22);
        TSMIRename2.Text = "Rename";
        TSMIRename2.Click += TSMIRename_Click;
        // 
        // TSS8
        // 
        TSS8.Name = "TSS8";
        TSS8.Size = new Size(136, 6);
        // 
        // TCEditors
        // 
        TCEditors.Controls.Add(TPValues);
        TCEditors.Controls.Add(TPHex);
        TCEditors.Dock = DockStyle.Fill;
        TCEditors.Location = new Point(0, 0);
        TCEditors.Name = "TCEditors";
        TCEditors.SelectedIndex = 0;
        TCEditors.Size = new Size(464, 495);
        TCEditors.TabIndex = 1;
        TCEditors.SelectedIndexChanged += TCEditors_SelectedIndexChanged;
        // 
        // TPValues
        // 
        TPValues.Controls.Add(LVValues);
        TPValues.Location = new Point(4, 24);
        TPValues.Name = "TPValues";
        TPValues.Padding = new Padding(3);
        TPValues.Size = new Size(456, 467);
        TPValues.TabIndex = 0;
        TPValues.Text = "Values";
        TPValues.UseVisualStyleBackColor = true;
        // 
        // LVValues
        // 
        LVValues.Activation = ItemActivation.OneClick;
        LVValues.Columns.AddRange(new ColumnHeader[] { CHName, CHValue });
        LVValues.Dock = DockStyle.Fill;
        LVValues.FullRowSelect = true;
        LVValues.GridLines = true;
        LVValues.HeaderStyle = ColumnHeaderStyle.Nonclickable;
        LVValues.Location = new Point(3, 3);
        LVValues.MultiSelect = false;
        LVValues.Name = "LVValues";
        LVValues.Size = new Size(450, 461);
        LVValues.TabIndex = 0;
        LVValues.UseCompatibleStateImageBehavior = false;
        LVValues.View = View.Details;
        LVValues.VirtualMode = true;
        LVValues.RetrieveVirtualItem += LVValues_RetrieveVirtualItem;
        LVValues.MouseDoubleClick += LVValues_MouseDoubleClick;
        LVValues.Resize += LVValues_Resize;
        // 
        // CHName
        // 
        CHName.Text = "Name";
        // 
        // CHValue
        // 
        CHValue.Text = "Value";
        // 
        // TPHex
        // 
        TPHex.Controls.Add(HBHex);
        TPHex.Location = new Point(4, 24);
        TPHex.Name = "TPHex";
        TPHex.Padding = new Padding(3);
        TPHex.Size = new Size(456, 467);
        TPHex.TabIndex = 1;
        TPHex.Text = "Hex";
        TPHex.UseVisualStyleBackColor = true;
        // 
        // HBHex
        // 
        // 
        // 
        // 
        HBHex.BuiltInContextMenu.CopyMenuItemText = "Copy";
        HBHex.BuiltInContextMenu.CutMenuItemText = "Cut";
        HBHex.BuiltInContextMenu.PasteMenuItemText = "Paste";
        HBHex.BuiltInContextMenu.SelectAllMenuItemText = "Select All";
        HBHex.ColumnInfoVisible = true;
        HBHex.Dock = DockStyle.Fill;
        HBHex.Font = new Font("Segoe UI", 9F);
        HBHex.LineInfoVisible = true;
        HBHex.Location = new Point(3, 3);
        HBHex.Name = "HBHex";
        HBHex.ReadOnly = true;
        HBHex.ShadowSelectionColor = Color.FromArgb(100, 60, 188, 255);
        HBHex.Size = new Size(450, 461);
        HBHex.StringViewVisible = true;
        HBHex.TabIndex = 0;
        HBHex.VScrollBarVisible = true;
        // 
        // MS1
        // 
        MS1.Items.AddRange(new ToolStripItem[] { TSMIFile, TSMIEdit, TSMITools, TSMIHelp });
        MS1.Location = new Point(0, 0);
        MS1.Name = "MS1";
        MS1.Padding = new Padding(7, 2, 0, 2);
        MS1.Size = new Size(933, 24);
        MS1.TabIndex = 1;
        MS1.Text = "menuStrip1";
        // 
        // TSMIFile
        // 
        TSMIFile.DropDownItems.AddRange(new ToolStripItem[] { TSMINew, TSMIOpen, TSMISave, TSMISaveAs, TSS9, TSMIEndianness, TSMICompressed, TSS1, TSMIRecentFiles, TSS2, TSMIExit });
        TSMIFile.Name = "TSMIFile";
        TSMIFile.Size = new Size(37, 20);
        TSMIFile.Text = "File";
        TSMIFile.DropDownOpening += TSMIFile_DropDownOpening;
        // 
        // TSMINew
        // 
        TSMINew.Image = Properties.Resources.NewFile_16x;
        TSMINew.Name = "TSMINew";
        TSMINew.ShortcutKeys = Keys.Control | Keys.N;
        TSMINew.Size = new Size(195, 22);
        TSMINew.Text = "New";
        TSMINew.Click += TSMINew_Click;
        // 
        // TSMIOpen
        // 
        TSMIOpen.Image = Properties.Resources.OpenfileDialog_16x;
        TSMIOpen.Name = "TSMIOpen";
        TSMIOpen.ShortcutKeys = Keys.Control | Keys.O;
        TSMIOpen.Size = new Size(195, 22);
        TSMIOpen.Text = "Open";
        TSMIOpen.Click += TSMIOpen_Click;
        // 
        // TSMISave
        // 
        TSMISave.Image = Properties.Resources.Save_16x;
        TSMISave.Name = "TSMISave";
        TSMISave.ShortcutKeys = Keys.Control | Keys.S;
        TSMISave.Size = new Size(195, 22);
        TSMISave.Text = "Save";
        TSMISave.Click += TSMISave_Click;
        // 
        // TSMISaveAs
        // 
        TSMISaveAs.Image = Properties.Resources.SaveAs_16x;
        TSMISaveAs.Name = "TSMISaveAs";
        TSMISaveAs.ShortcutKeys = Keys.Control | Keys.Shift | Keys.S;
        TSMISaveAs.Size = new Size(195, 22);
        TSMISaveAs.Text = "Save As...";
        TSMISaveAs.Click += TSMISaveAs_Click;
        // 
        // TSS9
        // 
        TSS9.Name = "TSS9";
        TSS9.Size = new Size(192, 6);
        // 
        // TSMIEndianness
        // 
        TSMIEndianness.DropDownItems.AddRange(new ToolStripItem[] { TSMILittleEndian, TSMIBigEndian });
        TSMIEndianness.Image = Properties.Resources.Sort_16x;
        TSMIEndianness.Name = "TSMIEndianness";
        TSMIEndianness.Size = new Size(195, 22);
        TSMIEndianness.Text = "Endianness";
        // 
        // TSMILittleEndian
        // 
        TSMILittleEndian.CheckOnClick = true;
        TSMILittleEndian.Name = "TSMILittleEndian";
        TSMILittleEndian.Size = new Size(139, 22);
        TSMILittleEndian.Text = "Little Endian";
        TSMILittleEndian.CheckedChanged += TSMILittleEndian_CheckedChanged;
        // 
        // TSMIBigEndian
        // 
        TSMIBigEndian.CheckOnClick = true;
        TSMIBigEndian.Name = "TSMIBigEndian";
        TSMIBigEndian.Size = new Size(139, 22);
        TSMIBigEndian.Text = "Big Endian";
        TSMIBigEndian.CheckedChanged += TSMIBigEndian_CheckedChanged;
        // 
        // TSMICompressed
        // 
        TSMICompressed.CheckOnClick = true;
        TSMICompressed.Image = Properties.Resources.ZipFile_16x;
        TSMICompressed.Name = "TSMICompressed";
        TSMICompressed.Size = new Size(195, 22);
        TSMICompressed.Text = "Compressed";
        TSMICompressed.CheckedChanged += TSMICompressed_CheckedChanged;
        // 
        // TSS1
        // 
        TSS1.Name = "TSS1";
        TSS1.Size = new Size(192, 6);
        // 
        // TSMIRecentFiles
        // 
        TSMIRecentFiles.DropDownItems.AddRange(new ToolStripItem[] { dummyToolStripMenuItem });
        TSMIRecentFiles.Image = Properties.Resources.History_16x;
        TSMIRecentFiles.Name = "TSMIRecentFiles";
        TSMIRecentFiles.Size = new Size(195, 22);
        TSMIRecentFiles.Text = "Recent Files";
        // 
        // dummyToolStripMenuItem
        // 
        dummyToolStripMenuItem.Name = "dummyToolStripMenuItem";
        dummyToolStripMenuItem.Size = new Size(117, 22);
        dummyToolStripMenuItem.Text = "Dummy";
        // 
        // TSS2
        // 
        TSS2.Name = "TSS2";
        TSS2.Size = new Size(192, 6);
        // 
        // TSMIExit
        // 
        TSMIExit.Image = Properties.Resources.Exit_16x;
        TSMIExit.Name = "TSMIExit";
        TSMIExit.ShortcutKeys = Keys.Alt | Keys.F4;
        TSMIExit.Size = new Size(195, 22);
        TSMIExit.Text = "Exit";
        TSMIExit.Click += TSMIExit_Click;
        // 
        // TSMIEdit
        // 
        TSMIEdit.DropDownItems.AddRange(new ToolStripItem[] { TSMIUndo, TSMIRedo, TSS10, TSMINewChunk1, TSS4, TSMICut1, TSMICopy1, TSMIPasteBefore1, TSMIPasteAfter1, TSMIPasteInside1, TSMIDuplicate1, TSS3, TSMIDelete1, TSMIRename1, TSS7, TSMIFind, TSMIFindNext });
        TSMIEdit.Name = "TSMIEdit";
        TSMIEdit.Size = new Size(39, 20);
        TSMIEdit.Text = "Edit";
        TSMIEdit.DropDownOpening += TSMIEdit_DropDownOpening;
        // 
        // TSMIUndo
        // 
        TSMIUndo.Image = Properties.Resources.Undo_16x;
        TSMIUndo.Name = "TSMIUndo";
        TSMIUndo.ShortcutKeys = Keys.Control | Keys.Z;
        TSMIUndo.Size = new Size(211, 22);
        TSMIUndo.Text = "Undo";
        TSMIUndo.Click += TSMIUndo_Click;
        // 
        // TSMIRedo
        // 
        TSMIRedo.Image = Properties.Resources.Redo_16x;
        TSMIRedo.Name = "TSMIRedo";
        TSMIRedo.ShortcutKeys = Keys.Control | Keys.Y;
        TSMIRedo.Size = new Size(211, 22);
        TSMIRedo.Text = "Redo";
        TSMIRedo.Click += TSMIRedo_Click;
        // 
        // TSS10
        // 
        TSS10.Name = "TSS10";
        TSS10.Size = new Size(208, 6);
        // 
        // TSMINewChunk1
        // 
        TSMINewChunk1.Image = Properties.Resources.NewItem_16x;
        TSMINewChunk1.Name = "TSMINewChunk1";
        TSMINewChunk1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.N;
        TSMINewChunk1.Size = new Size(211, 22);
        TSMINewChunk1.Text = "New Chunk";
        TSMINewChunk1.Click += TSMINewChunk_Click;
        // 
        // TSS4
        // 
        TSS4.Name = "TSS4";
        TSS4.Size = new Size(208, 6);
        // 
        // TSMICut1
        // 
        TSMICut1.Image = Properties.Resources.Cut_16x;
        TSMICut1.Name = "TSMICut1";
        TSMICut1.ShortcutKeys = Keys.Control | Keys.X;
        TSMICut1.Size = new Size(211, 22);
        TSMICut1.Text = "Cut";
        TSMICut1.Click += TSMICut_Click;
        // 
        // TSMICopy1
        // 
        TSMICopy1.DropDownItems.AddRange(new ToolStripItem[] { TSMICopyThis1, TSMICopyChildren1, TSMICopyType1 });
        TSMICopy1.Image = Properties.Resources.Copy_16x;
        TSMICopy1.Name = "TSMICopy1";
        TSMICopy1.Size = new Size(211, 22);
        TSMICopy1.Text = "Copy";
        TSMICopy1.Click += TSMICopyThis_Click;
        // 
        // TSMICopyThis1
        // 
        TSMICopyThis1.Name = "TSMICopyThis1";
        TSMICopyThis1.ShortcutKeys = Keys.Control | Keys.C;
        TSMICopyThis1.Size = new Size(193, 22);
        TSMICopyThis1.Text = "This";
        TSMICopyThis1.Click += TSMICopyThis_Click;
        // 
        // TSMICopyChildren1
        // 
        TSMICopyChildren1.Name = "TSMICopyChildren1";
        TSMICopyChildren1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.C;
        TSMICopyChildren1.Size = new Size(193, 22);
        TSMICopyChildren1.Text = "Children";
        TSMICopyChildren1.Click += TSMICopyChildren_Click;
        // 
        // TSMICopyType1
        // 
        TSMICopyType1.Name = "TSMICopyType1";
        TSMICopyType1.ShortcutKeys = Keys.Control | Keys.Alt | Keys.C;
        TSMICopyType1.Size = new Size(193, 22);
        TSMICopyType1.Text = "This Type";
        TSMICopyType1.Click += TSMICopyType_Click;
        // 
        // TSMIPasteBefore1
        // 
        TSMIPasteBefore1.Image = Properties.Resources.Paste_16x;
        TSMIPasteBefore1.Name = "TSMIPasteBefore1";
        TSMIPasteBefore1.ShortcutKeys = Keys.Control | Keys.V;
        TSMIPasteBefore1.Size = new Size(211, 22);
        TSMIPasteBefore1.Text = "Paste Before";
        TSMIPasteBefore1.Click += TSMIPasteBefore_Click;
        // 
        // TSMIPasteAfter1
        // 
        TSMIPasteAfter1.Image = Properties.Resources.Paste_16x;
        TSMIPasteAfter1.Name = "TSMIPasteAfter1";
        TSMIPasteAfter1.ShortcutKeys = Keys.Control | Keys.Shift | Keys.V;
        TSMIPasteAfter1.Size = new Size(211, 22);
        TSMIPasteAfter1.Text = "Paste After";
        TSMIPasteAfter1.Click += TSMIPasteAfter_Click;
        // 
        // TSMIPasteInside1
        // 
        TSMIPasteInside1.Image = Properties.Resources.PasteAppend_16x;
        TSMIPasteInside1.Name = "TSMIPasteInside1";
        TSMIPasteInside1.ShortcutKeys = Keys.Control | Keys.Alt | Keys.V;
        TSMIPasteInside1.Size = new Size(211, 22);
        TSMIPasteInside1.Text = "Paste Inside";
        TSMIPasteInside1.Click += TSMIPasteInside_Click;
        // 
        // TSMIDuplicate1
        // 
        TSMIDuplicate1.Image = Properties.Resources.Copy_16x;
        TSMIDuplicate1.Name = "TSMIDuplicate1";
        TSMIDuplicate1.ShortcutKeys = Keys.Control | Keys.D;
        TSMIDuplicate1.Size = new Size(211, 22);
        TSMIDuplicate1.Text = "Duplicate";
        TSMIDuplicate1.Click += TSMIDuplicate_Click;
        // 
        // TSS3
        // 
        TSS3.Name = "TSS3";
        TSS3.Size = new Size(208, 6);
        // 
        // TSMIDelete1
        // 
        TSMIDelete1.DropDownItems.AddRange(new ToolStripItem[] { TSMIDeleteThisForced, TSMIDeleteThis1, TSMIDeleteType1, TSMIDeleteChildren1 });
        TSMIDelete1.Image = Properties.Resources.Close_16x;
        TSMIDelete1.Name = "TSMIDelete1";
        TSMIDelete1.Size = new Size(211, 22);
        TSMIDelete1.Text = "Delete";
        TSMIDelete1.Click += TSMIDeleteThis_Click;
        // 
        // TSMIDeleteThisForced
        // 
        TSMIDeleteThisForced.Name = "TSMIDeleteThisForced";
        TSMIDeleteThisForced.ShortcutKeys = Keys.Shift | Keys.Delete;
        TSMIDeleteThisForced.Size = new Size(198, 22);
        TSMIDeleteThisForced.Text = "This (Forced)";
        TSMIDeleteThisForced.Visible = false;
        TSMIDeleteThisForced.Click += TSMIDeleteThis_Click;
        // 
        // TSMIDeleteThis1
        // 
        TSMIDeleteThis1.Name = "TSMIDeleteThis1";
        TSMIDeleteThis1.ShortcutKeys = Keys.Delete;
        TSMIDeleteThis1.Size = new Size(198, 22);
        TSMIDeleteThis1.Text = "This";
        TSMIDeleteThis1.Click += TSMIDeleteThis_Click;
        // 
        // TSMIDeleteType1
        // 
        TSMIDeleteType1.Name = "TSMIDeleteType1";
        TSMIDeleteType1.Size = new Size(198, 22);
        TSMIDeleteType1.Text = "This Type";
        TSMIDeleteType1.Click += TSMIDeleteType_Click;
        // 
        // TSMIDeleteChildren1
        // 
        TSMIDeleteChildren1.Name = "TSMIDeleteChildren1";
        TSMIDeleteChildren1.Size = new Size(198, 22);
        TSMIDeleteChildren1.Text = "Children";
        TSMIDeleteChildren1.Click += TSMIDeleteChildren_Click;
        // 
        // TSMIRename1
        // 
        TSMIRename1.Image = Properties.Resources.Rename_16x;
        TSMIRename1.Name = "TSMIRename1";
        TSMIRename1.ShortcutKeys = Keys.F2;
        TSMIRename1.Size = new Size(211, 22);
        TSMIRename1.Text = "Rename";
        TSMIRename1.Click += TSMIRename_Click;
        // 
        // TSS7
        // 
        TSS7.Name = "TSS7";
        TSS7.Size = new Size(208, 6);
        // 
        // TSMIFind
        // 
        TSMIFind.Image = Properties.Resources.FindInFile_16x;
        TSMIFind.Name = "TSMIFind";
        TSMIFind.ShortcutKeys = Keys.Control | Keys.F;
        TSMIFind.Size = new Size(211, 22);
        TSMIFind.Text = "Find...";
        TSMIFind.Click += TSMIFind_Click;
        // 
        // TSMIFindNext
        // 
        TSMIFindNext.Image = Properties.Resources.FindNext_16x;
        TSMIFindNext.Name = "TSMIFindNext";
        TSMIFindNext.ShortcutKeys = Keys.F3;
        TSMIFindNext.Size = new Size(211, 22);
        TSMIFindNext.Text = "Find Next";
        TSMIFindNext.Click += TSMIFindNext_Click;
        // 
        // TSMITools
        // 
        TSMITools.DropDownItems.AddRange(new ToolStripItem[] { TSMIOptions, TSS11 });
        TSMITools.Name = "TSMITools";
        TSMITools.Size = new Size(46, 20);
        TSMITools.Text = "Tools";
        TSMITools.DropDownOpening += TSMITools_DropDownOpening;
        // 
        // TSMIHelp
        // 
        TSMIHelp.DropDownItems.AddRange(new ToolStripItem[] { TSMIAbout });
        TSMIHelp.Name = "TSMIHelp";
        TSMIHelp.Size = new Size(44, 20);
        TSMIHelp.Text = "Help";
        // 
        // TSMIAbout
        // 
        TSMIAbout.Image = Properties.Resources.InformationSymbol_16x;
        TSMIAbout.Name = "TSMIAbout";
        TSMIAbout.Size = new Size(107, 22);
        TSMIAbout.Text = "About";
        TSMIAbout.Click += TSMIAbout_Click;
        // 
        // TSMIOptions
        // 
        TSMIOptions.Image = Properties.Resources.Settings_16x;
        TSMIOptions.Name = "TSMIOptions";
        TSMIOptions.Size = new Size(180, 22);
        TSMIOptions.Text = "Options";
        TSMIOptions.Click += TSMIOptions_Click;
        // 
        // TSS11
        // 
        TSS11.Name = "TSS11";
        TSS11.Size = new Size(177, 6);
        // 
        // FrmMain
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(933, 519);
        Controls.Add(SC1);
        Controls.Add(MS1);
        Icon = (Icon)resources.GetObject("$this.Icon");
        KeyPreview = true;
        MainMenuStrip = MS1;
        Margin = new Padding(4, 3, 4, 3);
        Name = "FrmMain";
        Text = "Pure3D Data Viewer";
        FormClosing += FrmMain_FormClosing;
        Load += FrmMain_Load;
        SC1.Panel1.ResumeLayout(false);
        SC1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)SC1).EndInit();
        SC1.ResumeLayout(false);
        CMSTVChunks.ResumeLayout(false);
        TCEditors.ResumeLayout(false);
        TPValues.ResumeLayout(false);
        TPHex.ResumeLayout(false);
        MS1.ResumeLayout(false);
        MS1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private MenuStrip MS1;
    private SplitContainer SC1;
    private ExplorerThemedTreeView TVChunks;
    private System.Windows.Forms.ToolStripMenuItem TSMIFile;
    private System.Windows.Forms.ToolStripMenuItem TSMINew;
    private System.Windows.Forms.ToolStripMenuItem TSMIOpen;
    private System.Windows.Forms.ToolStripMenuItem TSMISave;
    private System.Windows.Forms.ToolStripMenuItem TSMISaveAs;
    private System.Windows.Forms.ToolStripSeparator TSS1;
    private System.Windows.Forms.ToolStripMenuItem TSMIRecentFiles;
    private System.Windows.Forms.ToolStripSeparator TSS2;
    private System.Windows.Forms.ToolStripMenuItem TSMIExit;
    private ListView LVValues;
    private ColumnHeader CHName;
    private ColumnHeader CHValue;
    private TabControl TCEditors;
    private TabPage TPValues;
    private TabPage TPHex;
    private Be.Windows.Forms.HexBox HBHex;
    private ToolStripMenuItem TSMIEdit;
    private ToolStripMenuItem TSMIFind;
    private ToolStripMenuItem TSMIFindNext;
    private ToolStripMenuItem TSMINewChunk1;
    private ToolStripSeparator TSS4;
    private ToolStripMenuItem TSMICut1;
    private ToolStripMenuItem TSMICopy1;
    private ToolStripMenuItem TSMIPasteBefore1;
    private ToolStripMenuItem TSMIPasteAfter1;
    private ToolStripMenuItem TSMIPasteInside1;
    private ToolStripSeparator TSS3;
    private ToolStripMenuItem TSMICopyThis1;
    private ToolStripMenuItem TSMICopyChildren1;
    private ToolStripMenuItem TSMICopyType1;
    private ToolStripMenuItem TSMITools;
    private ContextMenuStrip CMSTVChunks;
    private ToolStripMenuItem TSMINewChunk2;
    private ToolStripSeparator TSS5;
    private ToolStripMenuItem TSMICut2;
    private ToolStripMenuItem TSMICopy2;
    private ToolStripMenuItem TSMICopyThis2;
    private ToolStripMenuItem TSMICopyChildren2;
    private ToolStripMenuItem TSMICopyType2;
    private ToolStripMenuItem TSMIPasteBefore2;
    private ToolStripMenuItem TSMIPasteAfter2;
    private ToolStripMenuItem TSMIPasteInside2;
    private ToolStripSeparator TSS6;
    private ToolStripMenuItem dummyToolStripMenuItem;
    private ToolStripMenuItem TSMIHelp;
    private ToolStripMenuItem TSMIAbout;
    private ToolStripMenuItem TSMIDelete1;
    private ToolStripMenuItem TSMIDeleteThis1;
    private ToolStripMenuItem TSMIDeleteType1;
    private ToolStripMenuItem TSMIDeleteChildren1;
    private ToolStripSeparator TSS7;
    private ToolStripMenuItem TSMIDeleteThisForced;
    private ToolStripMenuItem TSMIDelete2;
    private ToolStripSeparator TSS8;
    private ToolStripMenuItem TSMIDeleteThis2;
    private ToolStripMenuItem TSMIDeleteType2;
    private ToolStripMenuItem TSMIDeleteChildren2;
    private ToolStripMenuItem TSMIDuplicate2;
    private ToolStripMenuItem TSMIDuplicate1;
    private ToolStripMenuItem TSMIRename2;
    private ToolStripMenuItem TSMIRename1;
    private ToolStripSeparator TSS9;
    private ToolStripMenuItem TSMIEndianness;
    private ToolStripMenuItem TSMILittleEndian;
    private ToolStripMenuItem TSMIBigEndian;
    private ToolStripMenuItem TSMICompressed;
    private ToolStripMenuItem TSMIUndo;
    private ToolStripMenuItem TSMIRedo;
    private ToolStripSeparator TSS10;
    private ToolStripMenuItem TSMIOptions;
    private ToolStripSeparator TSS11;
}
