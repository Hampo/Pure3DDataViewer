using Be.Windows.Forms;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Enums;
using NetP3DLib.P3D.Exceptions;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Editors;
using Pure3DDataViewerPluginAPI.Events;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Helpers;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Pure3DDataViewer;

public partial class FrmMain : Form
{
    private P3DFile P3DFile = new();

    private FileSystemWatcher? _watcher = null;

    private record UndoEntry(string Change, P3DFile OldFile);
    private readonly Stack<UndoEntry> UndoStack = [];
    private readonly Stack<UndoEntry> RedoStack = [];

    private string _Text = string.Empty;
    private string _lastPath = string.Empty;
    private string LastPath
    {
        get => _lastPath;
        set
        {
            if (_lastPath == value)
                return;

            _lastPath = value;
            UpdateText();
        }
    }
    private bool _unsavedChanges = false;
    private bool UnsavedChanges
    {
        get
        {
            return _unsavedChanges;
        }
        set
        {
            if (_unsavedChanges == value)
                return;

            _unsavedChanges = value;
            UpdateText();
        }
    }

    private readonly List<(IFileHandler FileHandler, ToolStripMenuItem ToolMenu, ToolStripMenuItem ContextMenu)> _pluginFileHandlers = [];
    private readonly List<(Type ChunkType, ToolStripMenuItem ToolMenu, ToolStripMenuItem ContextMenu)> _pluginChunkHandlers = [];
    private readonly Dictionary<Type, List<TabPage>> _pluginChunkEditors = [];
    private readonly ToolStripSeparator _toolsFileChunkSeparator = new();

    private void UpdateText()
    {
        StringBuilder text = new();
        if (_unsavedChanges)
            text.Append('*');
        if (!string.IsNullOrEmpty(_lastPath))
            text.Append($"{Path.GetFileName(_lastPath)} - ");
        text.Append(_Text);

        Text = text.ToString();
    }

    private void TSMIRecentFile_Click(object? sender, EventArgs e)
    {
        if (sender is not ToolStripItem menuItem)
            return;

        if (UnsavedChanges)
        {
            var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (result)
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    TSMISave.PerformClick();
                    break;
            }
        }

        LoadP3DFile(menuItem.Text!);
    }

    public FrmMain()
    {
        InitializeComponent();
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        string version = Application.ProductVersion.Split('+')[0];
        while (version.EndsWith(".0"))
            version = version[..^2];
        _Text = $"{Text} v{version}";
        UpdateText();

        PluginLoader.LoadPlugins(Path.Combine(AppContext.BaseDirectory, "Plugins"));
        if (PluginLoader.Plugins.Count == 0)
        {
            TSMITools.Visible = false;
        }
        else
        {
            foreach (var plugin in PluginLoader.Plugins)
            {
                var fileHandlers = plugin.GetFileHandlers();
                if (fileHandlers == null)
                    continue;

                foreach (var fileHandler in fileHandlers)
                {
                    if (fileHandler == null)
                        continue;

                    var tsmiToolsMenu = new ToolStripMenuItem(fileHandler.Name)
                    {
                        Image = fileHandler.Image,
                        Tag = fileHandler
                    };
                    tsmiToolsMenu.Click += TSMIPlugin_Click;

                    var tsmiContextMenu = new ToolStripMenuItem(fileHandler.Name)
                    {
                        Image = fileHandler.Image,
                        Tag = fileHandler
                    };
                    tsmiContextMenu.Click += TSMIPlugin_Click;

                    _pluginFileHandlers.Add((fileHandler, tsmiToolsMenu, tsmiContextMenu));
                }
            }
            TSMITools.DropDownItems.Add(_toolsFileChunkSeparator);

            foreach (var plugin in PluginLoader.Plugins)
            {
                var chunkHandlers = plugin.GetChunkHandlers();
                if (chunkHandlers == null)
                    continue;

                foreach (var chunkHandler in chunkHandlers)
                {
                    if (chunkHandler == null)
                        continue;

                    var tsmiToolsMenu = new ToolStripMenuItem(chunkHandler.Name)
                    {
                        Image = chunkHandler.Image,
                        Tag = chunkHandler
                    };
                    tsmiToolsMenu.Click += TSMIPlugin_Click;

                    var tsmiContextMenu = new ToolStripMenuItem(chunkHandler.Name)
                    {
                        Image = chunkHandler.Image,
                        Tag = chunkHandler
                    };
                    tsmiContextMenu.Click += TSMIPlugin_Click;

                    var type = chunkHandler.ChunkType ?? typeof(Chunk);
                    _pluginChunkHandlers.Add((type, tsmiToolsMenu, tsmiContextMenu));
                }
            }

            foreach (var plugin in PluginLoader.Plugins)
            {
                var chunkEditors = plugin.GetChunkEditors();
                if (chunkEditors == null)
                    continue;

                foreach (var chunkEditor in chunkEditors)
                {
                    var types = chunkEditor.ChunkTypes;

                    var editor = chunkEditor.Editor;
                    editor.UpdatedChunk += IChunkEditor_UpdatedChunk;
                    editor.Dock = DockStyle.Fill;

                    var tp = new TabPage(chunkEditor.Name)
                    {
                        Name = chunkEditor.GetType().FullName,
                        Tag = types,
                    };
                    tp.Controls.Add(editor);

                    foreach (var type in types)
                    {
                        if (!_pluginChunkEditors.TryGetValue(type, out var editors))
                        {
                            _pluginChunkEditors[type] = [tp];
                            continue;
                        }
                        editors.Add(tp);
                    }
                }
            }
        }

        switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
        {
            case NetP3DLib.IO.Endianness.Little:
                TSMILittleEndian.Checked = true;
                break;
            case NetP3DLib.IO.Endianness.Big:
                TSMIBigEndian.Checked = true;
                break;
        }

        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            string file = args[1];
            if (File.Exists(file))
            {
                LoadP3DFile(file);
            }
        }

        PopulateData();
    }

    private void IChunkEditor_UpdatedChunk(object? sender, UpdatedChunkEventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        UpdateChunk(node, e.Chunk);
    }

    private void TSMIPlugin_Click(object? sender, EventArgs e)
    {
        if (sender is not ToolStripMenuItem tsmi)
            return;
        if (tsmi.GetCurrentParent() is ToolStripDropDownMenu toolStripDropDownMenu)
            toolStripDropDownMenu.Close();
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;
        var tag = node.Tag;
        if (tag == null)
            return;

        var handler = tsmi.Tag;
        var clone = P3DFile.Clone();
        switch (handler)
        {
            case IFileHandler fileHandler:
                try
                {
                    switch (fileHandler.Handle(P3DFile))
                    {
                        case Pure3DDataViewerPluginAPI.Enums.FileCallbackResult.Modified:
                            PreChange($"{fileHandler.Name}", clone);
                            UnsavedChanges = true;
                            PopulateData();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing \"{fileHandler.Name}\": {ex}", "Error executing plugin callback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
            case IChunkHandler chunkHandler:
                if (tag is not Chunk chunk)
                    return;

                if (chunkHandler.ChunkType != null && chunkHandler.ChunkType != chunk.GetType())
                    return;
                try
                {
                    switch (chunkHandler.Handle(chunk))
                    {
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.ModifiedData:
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.ModifiedChildren:
                            PreChange($"{chunkHandler.Name}", clone);
                            UpdateChunk(node, chunk);
                            break;
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.Deleted:
                            var parentNode = node.Parent;

                            if (parentNode.Tag is Chunk parentChunk)
                                parentChunk.Children.RemoveAt(node.Index);
                            else if (parentNode.Tag is P3DFile parentFile)
                                parentFile.Chunks.RemoveAt(node.Index);
                            PreChange($"{chunkHandler.Name}", clone);
                            UnsavedChanges = true;

                            TVChunks.BeginUpdate();
                            if (node.NextNode != null)
                                TVChunks.SelectedNode = node.NextNode;
                            else if (node.PrevNode != null)
                                TVChunks.SelectedNode = node.PrevNode;
                            else
                                TVChunks.SelectedNode = parentNode;
                            parentNode.Nodes.Remove(node);
                            for (int i = 0; i < parentNode.Nodes.Count; i++)
                            {
                                var childNode = parentNode.Nodes[i];
                                if (childNode.Tag is Chunk nodeChunk)
                                    childNode.Text = $"{childNode.Index}. {nodeChunk}";
                            }
                            UpdateErrors();
                            TVChunks.EndUpdate();
                            break;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error executing \"{chunkHandler.Name}\" from \"{chunkHandler.Name}\": {ex}", "Error executing plugin callback", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return;
        }
    }

    private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (UnsavedChanges)
        {
            var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (result)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    return;
                case DialogResult.Yes:
                    TSMISave.PerformClick();
                    break;
            }
        }
    }

    private void TSMINew_Click(object sender, EventArgs e)
    {
        if (UnsavedChanges)
        {
            var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (result)
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    TSMISave.PerformClick();
                    break;
            }
        }

        UnsavedChanges = false;
        UndoStack.Clear();
        RedoStack.Clear();
        P3DFile = new P3DFile();
        LastPath = string.Empty;
        PopulateData();

        _watcher?.Dispose();

        TSMICompressed.Checked = false;
        switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
        {
            case NetP3DLib.IO.Endianness.Little:
                TSMILittleEndian.Checked = true;
                break;
            case NetP3DLib.IO.Endianness.Big:
                TSMIBigEndian.Checked = true;
                break;
        }
    }

    private void TSMIOpen_Click(object sender, EventArgs e)
    {
        if (UnsavedChanges)
        {
            var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (result)
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    TSMISave.PerformClick();
                    break;
            }
        }

        using var ofd = new OpenFileDialog() { Title = "Open P3D File", Filter = "P3D files (*.p3d)|*.p3d|All files (*.*)|*.*" };
        if (!string.IsNullOrEmpty(LastPath))
            ofd.InitialDirectory = Path.GetDirectoryName(LastPath);
        if (ofd.ShowDialog() != DialogResult.OK)
            return;

        LoadP3DFile(ofd.FileName);
    }

    private void Save(string? filePath)
    {
        var validationErrors = new List<InvalidP3DException>();

        foreach (var chunk in P3DFile.Chunks)
            validationErrors.AddRange(chunk.ValidateChunks());

        if (validationErrors.Count > 0)
        {
            var firstError = validationErrors[0];
            if (MessageBox.Show($"There were {validationErrors.Count} validation errors in the P3D file:\n\n{firstError.Chunk} - {firstError.Message}\n\nDo you want to ignore these errors and save anyway?", "Validation errors in file", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.No)
                return;
        }

        try
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                var fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists && fileInfo.IsReadOnly)
                {
                    MessageBox.Show($"\"{filePath}\" is read-only.\nPlease choose a different location.", "Read-only", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    filePath = null;
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error checking \"{filePath}\": {ex.Message}.\nPlease choose a different location.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            filePath = null;
        }

        if (string.IsNullOrEmpty(filePath))
        {
            using var sfd = new SaveFileDialog()
            {
                Title = "Save File",
                Filter = "P3D files|*.p3d|All files|*.*",
                OverwritePrompt = true,
                CheckWriteAccess = true,
            };
            if (!string.IsNullOrEmpty(LastPath))
            {
                sfd.InitialDirectory = Path.GetDirectoryName(LastPath);
                sfd.FileName = Path.GetFileName(LastPath);
                if (!Path.GetExtension(LastPath).Equals(".p3d", StringComparison.OrdinalIgnoreCase))
                    sfd.FilterIndex = 2;
            }
            if (sfd.ShowDialog() != DialogResult.OK)
                return;
            filePath = sfd.FileName;
        }

        try
        {
            _watcher?.Dispose();
            if (!TSMICompressed.Checked)
                P3DFile.Write(filePath, TSMILittleEndian.Checked ? NetP3DLib.IO.Endianness.Little : NetP3DLib.IO.Endianness.Big, false);
            else
                P3DFile.Compress(filePath, false, false, false);
            UnsavedChanges = false;
            LastPath = filePath;
            TVChunks.Nodes[0].Text = LastPath;
            Settings.AddRecentFile(filePath);

            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath)!)
            {
                Filter = Path.GetFileName(filePath)!,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true,
            };
            _watcher.Changed += FileChanged;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error writing P3D file: {ex.Message}", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TSMISave_Click(object sender, EventArgs e)
    {
        Save(LastPath);
    }

    private void TSMISaveAs_Click(object sender, EventArgs e)
    {
        Save(null);
    }

    private void TSMIExit_Click(object sender, EventArgs e) => Application.Exit();

    private void LoadP3DFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            MessageBox.Show($"Could not find P3D file: {filePath}", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        try
        {
            var p3dFile = new P3DFile(filePath);
            UndoStack.Clear();
            RedoStack.Clear();
            P3DFile = p3dFile;
            LastPath = filePath;
            PopulateData();
            UnsavedChanges = false;
            Settings.AddRecentFile(LastPath);

            _watcher?.Dispose();
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath)!)
            {
                Filter = Path.GetFileName(filePath)!,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true,
            };
            _watcher.Changed += FileChanged;

            if (p3dFile.Size > int.MaxValue)
                return; // TODO: Support validating larger files

            var originalBytes = File.ReadAllBytes(filePath);
            var originalSignature = BitConverter.ToUInt32(originalBytes);
            switch (originalSignature)
            {
                case P3DFile.COMPRESSED_SIGNATURE:
                    TSMICompressed.Checked = true;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case NetP3DLib.IO.Endianness.Little:
                            TSMILittleEndian.Checked = true;
                            break;
                        case NetP3DLib.IO.Endianness.Big:
                            TSMIBigEndian.Checked = true;
                            break;
                    }
                    break;
                case P3DFile.COMPRESSED_SIGNATURE_SWAP:
                    TSMICompressed.Checked = false; // TODO: When compressed endian supported, change this
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case NetP3DLib.IO.Endianness.Little:
                            TSMIBigEndian.Checked = true;
                            break;
                        case NetP3DLib.IO.Endianness.Big:
                            TSMILittleEndian.Checked = true;
                            break;
                    }
                    UnsavedChanges = true;
                    MessageBox.Show($"Detected that the opened file is both compressed and has an endian that doesn't match the system's.\nIt is currently not possible to compress a file in a swapped endian.\nSaving will either remove compression or flip endian.", "Compression and endian mismatch detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                case P3DFile.SIGNATURE_SWAP:
                    TSMICompressed.Checked = false;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case NetP3DLib.IO.Endianness.Little:
                            TSMIBigEndian.Checked = true;
                            break;
                        case NetP3DLib.IO.Endianness.Big:
                            TSMILittleEndian.Checked = true;
                            break;
                    }
                    break;
                case P3DFile.SIGNATURE:
                    TSMICompressed.Checked = false;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case NetP3DLib.IO.Endianness.Little:
                            TSMILittleEndian.Checked = true;
                            break;
                        case NetP3DLib.IO.Endianness.Big:
                            TSMIBigEndian.Checked = true;
                            break;
                    }
                    break;
            }

            var newBytes = new byte[p3dFile.Size];
            using var ms = new MemoryStream(newBytes);
            if (!TSMICompressed.Checked)
                p3dFile.Write(ms, TSMILittleEndian.Checked ? NetP3DLib.IO.Endianness.Little : NetP3DLib.IO.Endianness.Big, false);
            else
                newBytes = LZR_Compression.CompressFile(p3dFile, false, false);

            if (!originalBytes.SequenceEqual(newBytes))
            {
                UnsavedChanges = true;
                MessageBox.Show($"Detected that the opened file has changed values.\n\nThis is likely caused by one of the following:\n- The file contains chunks with incorrect property values that were auto corrected.\n- Some Radical files released with SHAR contain incorrect chunk headers.\n- The file contains different string padding than expected.\n\nSaving is recommended, but will result in a modified file.", "Changes detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading P3D file: {ex}", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void FileChanged(object sender, FileSystemEventArgs e)
    {
        Invoke(() =>
        {
            if (MessageBox.Show(this, "Changes were detected in the original file. Do you want to reload?", "Changes detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                UnsavedChanges = true;
                return;
            }

            if (UnsavedChanges && MessageBox.Show(this, "There are unsaved changes in the current file. Do you want to save them?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                Save(null);

            LoadP3DFile(LastPath);
        });
    }

    private void PopulateData()
    {
        TVChunks.BeginUpdate();
        TVChunks.Nodes.Clear();

        var rootNode = new TreeNode(string.IsNullOrWhiteSpace(LastPath) ? "Untitled" : LastPath)
        {
            Tag = P3DFile
        };

        var (cancelled, childNodes) = ProgressHelper.Run("Loading chunk nodes", (reportProgress, isCancellationRequested) =>
        {
            var childNodes = new TreeNode[P3DFile.Chunks.Count];

            double index = 0;
            for (var i = 0; i < P3DFile.Chunks.Count; i++)
            {
                childNodes[i] = CreateChunkNode(i, P3DFile.Chunks[i]);
                reportProgress((int)(index++ / P3DFile.Chunks.Count * 100));
            }
            rootNode.Nodes.AddRange(childNodes);

            return childNodes;
        }, false);

        if (P3DFile.Chunks.Any(x => x.ValidateChunks().Any()))
        {
            var (errorBackColour, errorForeColour) = Settings.GetErrorChunkColour();
            rootNode.BackColor = errorBackColour;
            rootNode.ForeColor = errorForeColour;
        }

        TVChunks.Nodes.Add(rootNode);
        rootNode.Expand();
        TVChunks.SelectedNode = rootNode;

        TVChunks.EndUpdate();
    }

    private static TreeNode CreateChunkNode(int index, Chunk chunk)
    {
        var node = new TreeNode($"{index}. {chunk}")
        {
            Tag = chunk
        };

        if (chunk.Children != null && chunk.Children.Count > 0)
        {
            var children = new TreeNode[chunk.Children.Count];
            for (int i = 0; i < chunk.Children.Count; i++)
            {
                children[i] = CreateChunkNode(i, chunk.Children[i]);
            }
            node.Nodes.AddRange(children);
        }

        ApplyNodeStyling(node, chunk);

        return node;
    }

    private static void ApplyNodeStyling(TreeNode node, Chunk chunk)
    {
        if (!chunk.ValidateChunks().Any())
        {
            var (chunkBackColour, chunkForeColour) = Settings.GetChunkColour(chunk.GetType());
            node.BackColor = chunkBackColour;
            node.ForeColor = chunkForeColour;
        }
        else
        {
            var (errorBackColour, errorForeColour) = Settings.GetErrorChunkColour();
            node.BackColor = errorBackColour;
            node.ForeColor = errorForeColour;
        }

#if DEBUG
        if (chunk is UnknownChunk)
        {
            // Marking it for expansion later is faster than calling .Expand() now
        }
#endif
    }

    private TreeNode AddChunk(TreeNode parentNode, Chunk chunk, int index = -1, bool updateErrors = true, bool beginUpdate = true)
    {
        TreeNode chunkNode;
        if (index < 0)
        {
            chunkNode = parentNode.Nodes.Add($"{parentNode.Nodes.Count}. {chunk}");
        }
        else
        {
            if (beginUpdate)
                parentNode.TreeView.BeginUpdate();
            chunkNode = parentNode.Nodes.Insert(index, $"{index}. {chunk}");
            for (int i = 0; i < parentNode.Nodes.Count; i++)
            {
                var node = parentNode.Nodes[i];
                if (node.Tag is Chunk nodeChunk)
                    node.Text = $"{node.Index}. {nodeChunk}";
            }
            if (beginUpdate)
                parentNode.TreeView.EndUpdate();
        }
        chunkNode.Tag = chunk;

#if DEBUG
        if (chunk is UnknownChunk)
        {
            var parent = parentNode;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
        }
#endif

        (Color BackColour, Color ForeColour) colours;
        if (!chunk.ValidateChunks().Any())
        {
            colours = Settings.GetChunkColour(chunk.GetType());
        }
        else
        {
            colours = Settings.GetErrorChunkColour();

            var parent = parentNode;
            while (parent != null)
            {
                parent.Expand();
                parent.BackColor = colours.BackColour;
                parent.ForeColor = colours.ForeColour;
                parent = parent.Parent;
            }
        }
        chunkNode.BackColor = colours.BackColour;
        chunkNode.ForeColor = colours.ForeColour;

        foreach (var child in chunk.Children)
            AddChunk(chunkNode, child, -1, false);

        if (updateErrors)
            UpdateErrors();

        return chunkNode;
    }

    private static readonly HashSet<string> ExcludedProperties = ["DataBytes", "DataLength", "ID", "ParentFile", "ParentChunk", "IndexInParent", "Children", "AllChildren", "HeaderSize", "Size", "Bytes"];
    private bool _afterSelectUpdating = false;
    private readonly List<ListViewItem> _listViewItems = [];
    private void TVChunks_AfterSelect(object sender, TreeViewEventArgs e)
    {
        _afterSelectUpdating = true;
        var prevFocus = SC1.ActiveControl;
        LVValues.BeginUpdate();
        _listViewItems.Clear();

        TCEditors.SuspendLayout();

        var tag = e.Node?.Tag;

        try
        {
            TSMICut1.Enabled = false;
            TSMICopyThis1.Enabled = false;
            TSMICopyType1.Enabled = false;
            TSMIPasteBefore1.Enabled = false;
            TSMIPasteAfter1.Enabled = false;
            TSMIDelete1.Enabled = false;
            TSMIDeleteThis1.Enabled = false;
            TSMIDeleteType1.Enabled = false;
            TSMIDuplicate1.Enabled = false;
            TSMIRename1.Enabled = false;

            TSMICut2.Enabled = false;
            TSMICopyThis2.Enabled = false;
            TSMICopyType2.Enabled = false;
            TSMIPasteBefore2.Enabled = false;
            TSMIPasteAfter2.Enabled = false;
            TSMIDelete2.Enabled = false;
            TSMIDeleteThis2.Enabled = false;
            TSMIDeleteType2.Enabled = false;
            TSMIDuplicate2.Enabled = false;
            TSMIRename2.Enabled = false;

            for (int i = TSMITools.DropDownItems.Count - 1; i >= 0; i--)
                if (TSMITools.DropDownItems[i] is ToolStripMenuItem child && child.Tag is IChunkHandler)
                    TSMITools.DropDownItems.RemoveAt(i);

            for (int i = CMSTVChunks.Items.Count - 1; i >= 0; i--)
                if (CMSTVChunks.Items[i] is ToolStripMenuItem child && (child.Tag is IChunkHandler || child.Tag is IFileHandler))
                    CMSTVChunks.Items.RemoveAt(i);

            if (tag is P3DFile p3dFile)
            {
                var (cancelled, errors) = ProgressHelper.Run("Validating chunks", (reportProgress, isCancellationRequested) =>
                {
                    var errors = new List<string>();

                    double index = 0;
                    foreach (var child in p3dFile.Chunks)
                    {
                        if (isCancellationRequested())
                            break;

                        errors.AddRange(child.ValidateChunks().Select(e => $"Error in chunk \"{e.Chunk!.IndexInParent}. {e.Chunk}\": {e.Message}"));

                        reportProgress((int)(index++ / p3dFile.Chunks.Count * 100));
                    }

                    return errors;
                });

                if (!cancelled)
                {
                    var (backColour, foreColour) = Settings.GetErrorChunkColour();
                    foreach (var error in errors)
                    {
                        var lviError = new ListViewItem("Validation Error");
                        lviError.SubItems.Add(error);
                        lviError.BackColor = backColour;
                        lviError.ForeColor = foreColour;
                        _listViewItems.Add(lviError);
                    }
                }

                var lvi = new ListViewItem("Size");
                lvi.SubItems.Add($"{p3dFile.Size:N0} bytes");
                _listViewItems.Add(lvi);
                HBHex.ByteProvider = new DynamicByteProvider(p3dFile.Size > int.MaxValue ? Encoding.UTF8.GetBytes("Too large") : p3dFile.Bytes);

                foreach (var (FileHandler, _, ContextMenu) in _pluginFileHandlers)
                    if (FileHandler.IsFileSupported(p3dFile))
                        CMSTVChunks.Items.Add(ContextMenu);

                for (int i = TCEditors.TabCount - 1; i >= 2; i--)
                    TCEditors.TabPages.RemoveAt(i);

                TCEditors.SelectedTab = Settings.GetLastTabPage(TCEditors, typeof(P3DFile)) ?? TPValues;

                return;
            }

            if (tag is Chunk chunk)
            {
                var chunkType = chunk.GetType();

                TSMICut1.Enabled = true;
                TSMICopyThis1.Enabled = true;
                TSMICopyType1.Enabled = true;
                TSMIPasteBefore1.Enabled = true;
                TSMIPasteAfter1.Enabled = true;
                TSMIDelete1.Enabled = true;
                TSMIDeleteThis1.Enabled = true;
                TSMIDeleteType1.Enabled = true;
                TSMIDuplicate1.Enabled = true;
                TSMIRename1.Enabled = chunkType.IsSubclassOf(typeof(NamedChunk));

                TSMICut2.Enabled = true;
                TSMICopyThis2.Enabled = true;
                TSMICopyType2.Enabled = true;
                TSMIPasteBefore2.Enabled = true;
                TSMIPasteAfter2.Enabled = true;
                TSMIDelete2.Enabled = true;
                TSMIDeleteThis2.Enabled = true;
                TSMIDeleteType2.Enabled = true;
                TSMIDuplicate2.Enabled = true;
                TSMIRename2.Enabled = chunkType.IsSubclassOf(typeof(NamedChunk));

                foreach (var (ChunkType, ToolMenu, ContextMenu) in _pluginChunkHandlers)
                {
                    if (ChunkType != typeof(Chunk) && ChunkType != chunkType)
                        continue;
                    TSMITools.DropDownItems.Add(ToolMenu);
                    CMSTVChunks.Items.Add(ContextMenu);
                }

                for (int i = TCEditors.TabCount - 1; i >= 2; i--)
                {
                    var tp = TCEditors.TabPages[i];
                    if (tp.Tag is HashSet<Type> tagTypes && tagTypes.Contains(chunkType))
                        continue;
                    TCEditors.TabPages.RemoveAt(i);
                }
                if (_pluginChunkEditors.TryGetValue(chunk.GetType(), out var editors))
                {
                    foreach (var editorTP in editors)
                    {
                        if (!TCEditors.TabPages.Contains(editorTP))
                            TCEditors.TabPages.Add(editorTP);
                        var editorControl = (EditorControl)editorTP.Controls[0];
                        editorControl.LoadChunk(chunk);
                    }
                }

                TCEditors.SelectedTab = Settings.GetLastTabPage(TCEditors, chunkType) ?? TPValues;

                foreach (var error in chunk.ValidateChunks())
                {
                    var lviError = new ListViewItem("Validation Error");
                    lviError.SubItems.Add(error.Chunk == chunk ? error.Message : $"Error in child \"{error.Chunk!.IndexInParent}. {error.Chunk}\": {error.Message}");
                    var (backColour, foreColour) = Settings.GetErrorChunkColour();
                    lviError.BackColor = backColour;
                    lviError.ForeColor = foreColour;
                    _listViewItems.Add(lviError);
                }

                var properties = PropertyHelper.GetProperties(chunkType);

                foreach (var property in properties)
                {
                    if (ExcludedProperties.Contains(property.Name))
                        continue;

                    object? value = property.GetValue(chunk);
                    if (value is byte[] byteArray)
                    {
                        var lvi = new ListViewItem(property.Name);
                        lvi.SubItems.Add($"{byteArray.Length:N0} bytes");
                        lvi.Tag = property;
                        _listViewItems.Add(lvi);
                    }
                    else if (property.IsEnumerable() && value is IEnumerable enumerable)
                    {
                        List<object> values = [.. enumerable.Cast<object>()];
                        if (values.Count == 0)
                        {
                            var lvi = new ListViewItem($"{property.Name}[<EMPTY>]");
                            lvi.SubItems.Add("<NULL>");
                            lvi.Tag = (property, 0);
                            _listViewItems.Add(lvi);
                        }
                        else
                        {
                            for (int i = 0; i < values.Count; i++)
                            {
                                var lvi = new ListViewItem($"{property.Name}[{i}]");
                                lvi.SubItems.Add(values[i]?.ToString() ?? "<NULL>");
                                lvi.Tag = (property, i);
                                _listViewItems.Add(lvi);
                            }
                        }
                    }
                    else
                    {
                        var lvi = new ListViewItem(property.Name);
                        lvi.SubItems.Add(value?.ToString() ?? "<NULL>");
                        if (property.CanWrite)
                            lvi.Tag = property;
                        else
                            lvi.BackColor = Color.Silver;
                        _listViewItems.Add(lvi);
                    }
                }

                HBHex.ByteProvider = new DynamicByteProvider(chunk.DataLength > int.MaxValue ? Encoding.UTF8.GetBytes("Too large") : chunk.DataBytes);

                return;
            }
        }
        finally
        {
            var newSize = _listViewItems.Count;
            LVValues.VirtualListSize = newSize;
            if (newSize > 0)
            {
                for (int i = 0; i < newSize; i++)
                {
                    if (LVValues.Items[i].Text != "Validation Error")
                    {
                        LVValues.Items[i].Selected = true;
                        break;
                    }
                }
                LVValues.TopItem = LVValues.Items[0];
            }

            foreach (ColumnHeader column in LVValues.Columns)
                column.Width = -2;

            LVValues.EndUpdate();

            TCEditors.ResumeLayout();
            _afterSelectUpdating = false;
            if (prevFocus != null && prevFocus.CanFocus)
                prevFocus.Focus();
        }
    }

    private void TCEditors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_afterSelectUpdating)
            return;

        var tag = TVChunks.SelectedNode?.Tag;
        if (tag == null)
            return;

        Settings.SetLastTabPage(tag.GetType(), TCEditors.SelectedTab!);
    }

    private void LVValues_Resize(object sender, EventArgs e)
    {
        LVValues.BeginUpdate();
        var selectedIndices = LVValues.SelectedIndices.Cast<int>().ToArray();
        foreach (ColumnHeader column in LVValues.Columns)
            column.Width = -2;
        foreach (int index in selectedIndices)
            LVValues.Items[index].Selected = true;
        LVValues.EndUpdate();
    }

    private void LVValues_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (sender is not ListView lv)
            return;

        if (e.Button != MouseButtons.Left)
            return;

        if (TVChunks.SelectedNode?.Tag is not Chunk chunk)
            return;

        var lvi = lv.GetItemAt(e.X, e.Y);
        if (lvi == null)
            return;

        var Updated = false;
        var clone = P3DFile.Clone();
        switch (lvi.Tag)
        {
            case PropertyInfo property:
                Updated = EditProperty(property, chunk);
                break;
            case (PropertyInfo listProperty, int index):
                Updated = EditProperty(listProperty, chunk, index);
                break;
        }

        if (Updated)
        {
            PreChange("Update Value", clone);
            UpdateChunk(TVChunks.SelectedNode, chunk);
        }
    }

    public static bool EditProperty(PropertyInfo property, object obj, int? index = null)
    {
        var objectType = obj.GetType();
        if (objectType != property.DeclaringType && !objectType.IsSubclassOf(property.DeclaringType!))
            return false;

        var oldValue = property.GetValue(obj);

        if (oldValue is byte[] byteArray)
        {
            using var byteArrayEditor = new FrmByteArrayEditor(property.Name, byteArray);
            if (byteArrayEditor.ShowDialog() != DialogResult.OK)
                return false;

            property.SetValue(obj, byteArrayEditor.Value);
            return true;
        }

        var propertyType = property.PropertyType;

        IList? list = null;
        if (typeof(IList).IsAssignableFrom(propertyType))
        {
            list = (IList)oldValue!;

            if (index == null || index.Value < 0 || index.Value >= list.Count)
                return false;

            oldValue = list[index.Value];
            if (oldValue == null)
                return false;
            propertyType = oldValue.GetType();
        }

        object? newValue = null;
        if (propertyType.HasFlagsAttribute())
        {
            using var enumFlagsEditor = new FrmEnumFlagsEditor(propertyType, property.Name, oldValue);
            if (enumFlagsEditor.ShowDialog() != DialogResult.OK)
                return false;

            newValue = enumFlagsEditor.Value;
        }
        else if (propertyType.IsEnum)
        {
            using var enumEditor = new FrmEnumEditor(propertyType, property.Name, oldValue);
            if (enumEditor.ShowDialog() != DialogResult.OK)
                return false;

            newValue = enumEditor.Value;
        }
        else if (propertyType == typeof(Color))
        {
            //using var colourEditor = new FrmColourEditor(property.Name, (Color?)oldValue);
            //if (colourEditor.ShowDialog() != DialogResult.OK)
            //    return false;

            //newValue = colourEditor.Value;

            using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
            {
                Color = (Color?)oldValue ?? Color.White,
                ShowAlphaChannel = true,
                Text = $"Edit Value: {property.Name}",
            };
            if (colorPicker.ShowDialog() != DialogResult.OK)
                return false;

            newValue = colorPicker.Color;
        }
        else if (propertyType == typeof(bool))
        {
            using var booleanEditor = new FrmBooleanEditor(property.Name, (bool?)oldValue);
            if (booleanEditor.ShowDialog() != DialogResult.OK)
                return false;

            newValue = booleanEditor.Value;
        }
        else if (propertyType == typeof(string))
        {
            var maxLengthAttribute = property.GetCustomAttribute<MaxLengthAttribute>();
            var knownValuesAttribute = property.GetCustomAttribute<KnownValuesAttribute>();

            if (knownValuesAttribute != null)
            {
                using var knownStringEditor = new FrmKnownStringEditor(property.Name, (string?)oldValue, knownValuesAttribute.Values, maxLengthAttribute?.MaxLength ?? 255);
                if (knownStringEditor.ShowDialog() != DialogResult.OK)
                    return false;

                newValue = knownStringEditor.Value;
            }
            else
            {
                using var stringEditor = new FrmStringEditor(property.Name, (string?)oldValue, maxLengthAttribute?.MaxLength ?? 255);
                if (stringEditor.ShowDialog() != DialogResult.OK)
                    return false;

                newValue = stringEditor.Value;
            }
        }
        else if (propertyType == typeof(char))
        {
            using var charEditor = new FrmCharEditor(property.Name, (char?)oldValue);
            if (charEditor.ShowDialog() != DialogResult.OK)
                return false;

            newValue = charEditor.Value;
        }
        else if (NumericTextBox.GetNumericType(propertyType) != null)
        {
            using var numericEditor = new FrmNumericEditor(property.Name, oldValue);
            if (numericEditor.ShowDialog() != DialogResult.OK)
                return false;

            if (numericEditor.Value == null)
            {
                MessageBox.Show("An invalid numeric value was entered. Value not updated.", "Error updating value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            newValue = numericEditor.Value;
        }
        else if ((propertyType.IsValueType && !propertyType.IsEnum && !propertyType.IsPrimitive) || propertyType.IsClass)
        {
            if (oldValue == null)
                return false;

            var reference = oldValue;
            using var structEditor = new FrmStructEditor(ref reference);
            if (structEditor.ShowDialog() != DialogResult.OK)
                return false;

            newValue = reference;
        }
        else
        {
            MessageBox.Show($"Unknown item type \"{propertyType}\".", "Error updating value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        if (list == null)
        {
            property.SetValue(obj, newValue);
        }
        else
        {
            list[index!.Value] = newValue;
        }
        return true;
    }

    private void TVChunks_DragEnter(object sender, DragEventArgs e)
    {
        e.Effect = e.Data?.GetDataPresent(DataFormats.FileDrop) ?? false ? DragDropEffects.Copy : DragDropEffects.None;
    }

    private void TVChunks_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data == null || !e.Data.GetDataPresent(DataFormats.FileDrop))
            return;

        var files = (string[])e.Data.GetData(DataFormats.FileDrop)!;
        if (files.Length == 0)
            return;

        if (UnsavedChanges)
        {
            var result = MessageBox.Show("There are unsaved changes. Do you want to save them?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button3);
            switch (result)
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    TSMISave.PerformClick();
                    break;
            }
        }

        LoadP3DFile(files[0]);
    }

    private FrmFind? _frmFind = null;
    private void TSMIFind_Click(object sender, EventArgs e)
    {
        if (_frmFind != null)
        {
            _frmFind.BringToFront();
            _frmFind.Focus();
        }
        else
        {
            _frmFind = new(this);
            _frmFind.FormClosing += (sender, e) =>
            {
                _frmFind?.Dispose();
                _frmFind = null;
            };
            _frmFind.Show(this);
        }
    }

    private void TSMIFindNext_Click(object sender, EventArgs e) => Find(_searchQuery);

    private string _searchQuery = string.Empty;
    public void Find(string searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
            return;

        _searchQuery = searchQuery;

        var found = FindNextNode(searchQuery);
        if (found == null)
        {
            MessageBox.Show("Reached end of file.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        TVChunks.SelectedNode = found;
        found.EnsureVisible();
    }

    private TreeNode? FindNextNode(string searchQuery)
    {
        if (TVChunks.Nodes.Count == 0) return null;

        var allNodes = new List<TreeNode>();
        foreach (TreeNode node in TVChunks.Nodes)
            CollectNodes(node, allNodes);

        if (!Settings.FindDirection)
            allNodes.Reverse();

        var startIndex = 0;
        if (TVChunks.SelectedNode != null)
        {
            startIndex = allNodes.IndexOf(TVChunks.SelectedNode);
            if (startIndex == -1) startIndex = 0;
        }

        var comparison = Settings.FindMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        for (int i = startIndex + 1; i < allNodes.Count; i++)
        {
            if (allNodes[i].Text.Contains(searchQuery, comparison))
                return allNodes[i];

            if (Settings.FindIncludeProperties && allNodes[i].Tag is Chunk chunk && SearchChunkProperties(chunk, searchQuery, comparison))
                return allNodes[i];
        }

        if (Settings.FindWrapAround)
        {
            for (int i = 0; i <= startIndex; i++)
            {
                if (allNodes[i].Text.Contains(searchQuery, comparison))
                    return allNodes[i];

                if (Settings.FindIncludeProperties && allNodes[i].Tag is Chunk chunk && SearchChunkProperties(chunk, searchQuery, comparison))
                    return allNodes[i];
            }
        }

        return null;
    }

    private static void CollectNodes(TreeNode node, List<TreeNode> list)
    {
        list.Add(node);
        foreach (TreeNode child in node.Nodes)
            CollectNodes(child, list);
    }

    private static bool SearchChunkProperties(Chunk chunk, string query, StringComparison comparison)
    {
        var type = chunk.GetType();
        var properties = PropertyHelper.GetProperties(type);
        foreach (var property in properties)
        {
            if (ExcludedProperties.Contains(property.Name))
                continue;

            object? value = property.GetValue(chunk);
            if (value == null || value is byte[])
                continue;

            if (value is not string && value is IEnumerable enumerable)
            {
                if (enumerable.Cast<object>().Any(x => (x.ToString() ?? "").Contains(query, comparison)))
                    return true;
            }
            else if (value is not string && property.PropertyType.IsClass && !property.PropertyType.IsPrimitive)
            {
                var classProperties = PropertyHelper.GetProperties(value.GetType());
                foreach (var classProperty in classProperties)
                {
                    object? value2 = classProperty.GetValue(value);
                    if (value2 == null || value2 is byte[])
                        continue;

                    var valueStr = value2.ToString();

                    if (valueStr != null && valueStr.Contains(query, comparison))
                        return true;
                }
            }
            else
            {
                var valueStr = value.ToString();

                if (valueStr != null && valueStr.Contains(query, comparison))
                    return true;
            }
        }

        return false;
    }

    private static void CopyChunks(IEnumerable<Chunk> chunks)
    {
        var chunkData = chunks.Select(x => x.Bytes).ToArray();

        var dataObject = new DataObject();
        dataObject.SetData("Pure3DChunk", chunkData);
        Clipboard.SetDataObject(dataObject, true);
    }

    private void TSMICut_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node?.Tag is not Chunk chunk)
            return;

        CopyChunks([chunk]);

        var parentNode = node.Parent;

        PreChange($"Chunk Cut");
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.RemoveAt(node.Index);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.RemoveAt(node.Index);
        UnsavedChanges = true;

        TVChunks.BeginUpdate();
        if (node.NextNode != null)
            TVChunks.SelectedNode = node.NextNode;
        else if (node.PrevNode != null)
            TVChunks.SelectedNode = node.PrevNode;
        else
            TVChunks.SelectedNode = parentNode;
        parentNode.Nodes.Remove(node);
        for (int i = 0; i < parentNode.Nodes.Count; i++)
        {
            var childNode = parentNode.Nodes[i];
            if (childNode.Tag is Chunk nodeChunk)
                childNode.Text = $"{childNode.Index}. {nodeChunk}";
        }
        UpdateErrors();
        TVChunks.EndUpdate();
    }

    private void TSMICopyThis_Click(object sender, EventArgs e)
    {
        if (TVChunks.SelectedNode?.Tag is not Chunk chunk)
            return;

        CopyChunks([chunk]);
    }

    private void TSMICopyChildren_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        switch (node.Tag)
        {
            case P3DFile p3dFile:
                CopyChunks(p3dFile.Chunks);
                break;
            case Chunk chunk:
                CopyChunks(chunk.Children);
                break;
        }
    }

    private void TSMICopyType_Click(object sender, EventArgs e)
    {
        if (TVChunks.SelectedNode?.Tag is not Chunk chunk)
            return;

        var chunkType = chunk.GetType();
        CopyChunks(P3DFile.AllChunks.Where(x => x.GetType() == chunkType));
    }

    private static List<Chunk>? GetChunksFromClipboard()
    {
        try
        {
            var data = Clipboard.GetData("Pure3DChunk");
            if (data == null)
                return null;

            if (data is not byte[][] chunkBytes)
                return null;

            var chunks = new List<Chunk>(chunkBytes.Length);
            foreach (var bytes in chunkBytes)
            {
                using var ms = new MemoryStream(bytes);
                using var br = new BinaryReader(ms);
                var chunk = ChunkLoader.LoadChunk(br);
                chunks.Add(chunk);
            }
            return chunks;
        }
        catch (ExternalException ex) when (ex.ErrorCode == unchecked((int)0x800401D0)) // CLIPBRD_E_CANT_OPEN
        {
            return null;
        }
    }

    private void TSMIPasteBefore_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node?.Tag is not Chunk)
            return;

        IList<Chunk> parentChunks;
        var parentNode = node.Parent;
        switch (parentNode.Tag)
        {
            case P3DFile p3dFile:
                parentChunks = p3dFile.Chunks;
                break;
            case Chunk chunk:
                parentChunks = chunk.Children;
                break;
            default:
                return;
        }

        var chunks = GetChunksFromClipboard();
        if (chunks == null || chunks.Count == 0)
            return;

        PreChange("Paste Before");
        UnsavedChanges = true;
        var index = node.Index;
        for (var i = chunks.Count - 1; i >= 0; i--)
        {
            parentChunks.Insert(index, chunks[i]);
            var chunkNode = AddChunk(parentNode, chunks[i], index);
            chunkNode.EnsureVisible();
        }
    }

    private void TSMIPasteAfter_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node?.Tag is not Chunk)
            return;

        IList<Chunk> parentChunks;
        var parentNode = node.Parent;
        switch (parentNode.Tag)
        {
            case P3DFile p3dFile:
                parentChunks = p3dFile.Chunks;
                break;
            case Chunk chunk:
                parentChunks = chunk.Children;
                break;
            default:
                return;
        }

        var chunks = GetChunksFromClipboard();
        if (chunks == null || chunks.Count == 0)
            return;

        PreChange("Paste After");
        UnsavedChanges = true;
        var index = node.Index + 1;
        for (var i = chunks.Count - 1; i >= 0; i--)
        {
            parentChunks.Insert(index, chunks[i]);
            var chunkNode = AddChunk(parentNode, chunks[i], index);
            chunkNode.EnsureVisible();
        }
    }

    private void TSMIPasteInside_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        IList<Chunk> parentChunks;
        switch (node.Tag)
        {
            case P3DFile p3dFile:
                parentChunks = p3dFile.Chunks;
                break;
            case Chunk chunk:
                parentChunks = chunk.Children;
                break;
            default:
                return;
        }

        var chunks = GetChunksFromClipboard();
        if (chunks == null || chunks.Count == 0)
            return;

        PreChange("Paste Inside");
        UnsavedChanges = true;
        foreach (var chunk in chunks)
        {
            parentChunks.Add(chunk);
            var chunkNode = AddChunk(node, chunk);
            chunkNode.EnsureVisible();
        }
    }

    private void TSMINewChunk_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;
        var tag = node.Tag;
        if (tag == null)
            return;

        using var frmNewChunk = new FrmNewChunk();
        if (frmNewChunk.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            var newChunks = frmNewChunk.Chunks;
            if (newChunks == null || newChunks.Count == 0)
                return;

            PreChange("New Chunk");
            UnsavedChanges = true;

            IList<Chunk> parentChunks;
            switch (node.Tag)
            {
                case P3DFile p3dFile:
                    parentChunks = p3dFile.Chunks;
                    break;
                case Chunk chunk:
                    parentChunks = chunk.Children;
                    break;
                default:
                    return;
            }

            TreeNode? chunkNode = null;
            foreach (var newChunk in newChunks)
            {
                parentChunks.Add(newChunk);
                chunkNode = AddChunk(node, newChunk);
                chunkNode.EnsureVisible();
            }
            TVChunks.SelectedNode = chunkNode;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error creating chunk: {ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TVChunks_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
            TVChunks.SelectedNode = e.Node;
    }

    private void TSMIFile_DropDownOpening(object sender, EventArgs e)
    {
        TSMIRecentFiles.DropDownItems.Clear();

        foreach (var recentFile in Settings.RecentFiles)
        {
            if (!File.Exists(recentFile))
                continue;

            var tsmi = TSMIRecentFiles.DropDownItems.Add(recentFile);
            tsmi.Click += TSMIRecentFile_Click;
            tsmi.Image = Properties.Resources.OpenFile_16x;
        }
    }

    private void TSMITools_DropDownOpening(object sender, EventArgs e)
    {
        for (int i = TSMITools.DropDownItems.Count - 1; i >= 0; i--)
            if (TSMITools.DropDownItems[i] is ToolStripMenuItem tsmi && tsmi.Tag is IFileHandler)
                TSMITools.DropDownItems.RemoveAt(i);

        bool hasFileHandlers = false;
        for (int i = _pluginFileHandlers.Count - 1; i >= 0; i--)
        {
            var (FileHandler, ToolMenu, _) = _pluginFileHandlers[i];
            if (FileHandler.IsFileSupported(P3DFile))
            {
                hasFileHandlers = true;
                TSMITools.DropDownItems.Insert(2, ToolMenu);
            }
        }

        _toolsFileChunkSeparator.Visible = hasFileHandlers && TSMITools.DropDownItems.OfType<ToolStripMenuItem>().Any(x => x.Tag is IChunkHandler);

        HandlePluginSettings(TSMITools.DropDownItems);
    }

    private void CMSTVChunks_Opening(object sender, System.ComponentModel.CancelEventArgs e) => HandlePluginSettings(CMSTVChunks.Items);

    private static void HandlePluginSettings(ToolStripItemCollection items)
    {
        foreach (var item in items.OfType<ToolStripMenuItem>())
        {
            IList<(string Name, bool Value)>? settings;
            Action<string, bool> setSetting;
            switch (item.Tag)
            {
                case IFileHandler fileHandler:
                    settings = fileHandler.GetSettings();
                    setSetting = fileHandler.SetSetting;

                    break;
                case IChunkHandler chunkHandler:
                    settings = chunkHandler.GetSettings();
                    setSetting = chunkHandler.SetSetting;

                    break;
                default:
                    continue;
            }

            item.DropDownItems.Clear();
            if (settings == null)
                continue;

            foreach (var setting in settings)
            {
                var tsmi = new ToolStripMenuItem(setting.Name)
                {
                    CheckOnClick = true,
                    Checked = setting.Value,
                };
                tsmi.Click += (s, e) => setSetting(setting.Name, tsmi.Checked);
                item.DropDownItems.Add(tsmi);
            }
        }
    }

    private void TSMIAbout_Click(object sender, EventArgs e)
    {
        using var frmAbout = new FrmAbout();
        frmAbout.ShowDialog();
    }

    private void UpdateChunk(TreeNode node, Chunk chunk, bool beginUpdate = true, bool updateErrors = true)
    {
        UnsavedChanges = true;
        if (beginUpdate)
            TVChunks.BeginUpdate();

        node.Tag = chunk;
        node.Text = $"{node.Index}. {chunk}";

        var childCount = chunk.Children.Count;
        var nodeCount = node.Nodes.Count;

        if (nodeCount > childCount)
            for (int i = nodeCount - 1; i >= childCount; i--)
                node.Nodes.RemoveAt(i);
        else if (childCount > nodeCount)
            for (int i = nodeCount; i < childCount; i++)
                AddChunk(node, chunk.Children[i], beginUpdate: false);

        for (int i = 0; i < childCount; i++)
        {
            var childNode = node.Nodes[i];
            var childChunk = chunk.Children[i];
            childNode.Tag = childChunk;

            UpdateChunk(childNode, childChunk, false, false);
        }

        if (updateErrors)
            UpdateErrors();

        if (node.IsSelected)
        {
            for (int i = _listViewItems.Count - 1; i >= 0; i--)
            {
                var lvi = _listViewItems[i];

                if (lvi.Text == "Validation Error")
                {
                    _listViewItems.RemoveAt(i);
                    continue;
                }

                switch (lvi.Tag)
                {
                    case PropertyInfo property:
                        var value = property.GetValue(chunk);
                        if (value is byte[] byteArray)
                            lvi.SubItems[1].Text = $"{byteArray.Length:N0} bytes";
                        else
                            lvi.SubItems[1].Text = value?.ToString() ?? "<NULL>";
                        break;
                    case (PropertyInfo listProperty, int index):
                        _listViewItems.RemoveAt(i);

                        if (index == 0)
                        {
                            var enumerable = (IEnumerable)listProperty.GetValue(chunk)!;
                            List<object> values = [.. enumerable.Cast<object>()];
                            if (values.Count == 0)
                            {
                                var lviItem = new ListViewItem($"{listProperty.Name}[<EMPTY>]");
                                lviItem.SubItems.Add("<NULL>");
                                lviItem.Tag = (listProperty, 0);
                                _listViewItems.Insert(i, lviItem);
                            }
                            else
                            {
                                for (var j = values.Count - 1; j >= 0; j--)
                                {
                                    var lviItem = new ListViewItem($"{listProperty.Name}[{j}]");
                                    lviItem.SubItems.Add(values[j]?.ToString() ?? "<NULL>");
                                    lviItem.Tag = (listProperty, j);
                                    _listViewItems.Insert(i, lviItem);
                                }
                            }
                        }

                        break;
                }
            }

            foreach (var error in chunk.ValidateChunks())
            {
                var lviError = new ListViewItem("Validation Error");
                lviError.SubItems.Add(error.Chunk == chunk ? error.Message : $"Error in child \"{error.Chunk!.IndexInParent}. {error.Chunk}\": {error.Message}");
                var (backColour, foreColour) = Settings.GetErrorChunkColour();
                lviError.BackColor = backColour;
                lviError.ForeColor = foreColour;
                _listViewItems.Insert(0, lviError);
            }

            LVValues.VirtualListSize = _listViewItems.Count;
            LVValues.Invalidate();

            if (_pluginChunkEditors.TryGetValue(chunk.GetType(), out var editors))
            {
                foreach (var editorTP in editors)
                {
                    if (!TCEditors.TabPages.Contains(editorTP))
                        TCEditors.TabPages.Add(editorTP);
                    var editorControl = (EditorControl)editorTP.Controls[0];
                    editorControl.LoadChunk(chunk);
                }
            }
        }

        if (beginUpdate)
            TVChunks.EndUpdate();
    }

    private void UpdateErrors()
    {
        var allNodes = new List<TreeNode>();
        foreach (TreeNode childNode in TVChunks.Nodes)
            CollectNodes(childNode, allNodes);

        bool errors = false;
        foreach (var childNode in allNodes)
        {
            if (childNode.Tag is not Chunk childChunk)
                continue;

            (Color BackColour, Color ForeColour) childColours;
            if (!childChunk.ValidateChunks().Any())
            {
                childColours = Settings.GetChunkColour(childChunk.GetType());
            }
            else
            {
                childColours = Settings.GetErrorChunkColour();
                errors = true;
            }
            childNode.BackColor = childColours.BackColour;
            childNode.ForeColor = childColours.ForeColour;
        }

        var rootNode = allNodes[0];
        (Color BackColour, Color ForeColour) = errors ? Settings.GetErrorChunkColour() : (Color.Empty, Color.Empty);
        rootNode.BackColor = BackColour;
        rootNode.ForeColor = ForeColour;
    }

    private void LVValues_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
    {
        if (e.ItemIndex >= _listViewItems.Count)
        {
            var lvi = new ListViewItem("");
            lvi.SubItems.Add("");
            e.Item = lvi;
            return;
        }

        e.Item = _listViewItems[e.ItemIndex];
    }


    private int? _SC1SplitterDistance = null;
    private void SC1_Resize(object sender, EventArgs e)
    {
        if (!_SC1SplitterDistance.HasValue)
            _SC1SplitterDistance = SC1.SplitterDistance;

        SC1.SplitterDistance = _SC1SplitterDistance.Value;
    }

    private void SC1_SplitterMoving(object sender, SplitterCancelEventArgs e)
    {
        _SC1SplitterDistance = e.SplitX;
    }

    private void TSMIDeleteThis_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        if (node.Tag is not Chunk)
            return;

        bool isShiftDown = (ModifierKeys & Keys.Shift) == Keys.Shift;
        if (!isShiftDown && MessageBox.Show($"Are you sure you want to delete the selected chunk:\n{node.Text}?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        var parentNode = node.Parent;

        PreChange("Delete Chunk");
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.RemoveAt(node.Index);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.RemoveAt(node.Index);
        UnsavedChanges = true;

        TVChunks.BeginUpdate();
        if (node.NextNode != null)
            TVChunks.SelectedNode = node.NextNode;
        else if (node.PrevNode != null)
            TVChunks.SelectedNode = node.PrevNode;
        else
            TVChunks.SelectedNode = parentNode;
        parentNode.Nodes.Remove(node);
        for (int i = 0; i < parentNode.Nodes.Count; i++)
        {
            var childNode = parentNode.Nodes[i];
            if (childNode.Tag is Chunk nodeChunk)
                childNode.Text = $"{childNode.Index}. {nodeChunk}";
        }
        UpdateErrors();
        TVChunks.EndUpdate();
    }

    private void TSMIDeleteType_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        if (node.Tag is not Chunk chunk)
            return;

        var chunkType = chunk.GetType();

        var chunkTypeName = chunkType.Name;
        if (Enum.IsDefined(typeof(ChunkIdentifier), (ChunkIdentifier)chunk.ID))
            chunkTypeName = ((ChunkIdentifier)chunk.ID).ToString().Replace("_", " ");
        else if (chunkTypeName.EndsWith("Chunk"))
            chunkTypeName = chunkTypeName[..^5];

        var parentNode = node.Parent;

        bool isShiftDown = (ModifierKeys & Keys.Shift) == Keys.Shift;
        if (!isShiftDown && MessageBox.Show($"Are you sure you want to delete all chunks of type \"{chunkTypeName}\" in:\n{parentNode.Text}?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        PreChange("Delete Type");
        UnsavedChanges = true;
        if (parentNode.Tag is Chunk parentChunk)
        {
            TVChunks.SelectedNode = parentNode;
            for (var i = parentChunk.Children.Count - 1; i >= 0; i--)
                if (parentChunk.Children[i].GetType() == chunkType)
                    parentChunk.Children.RemoveAt(i);
            UpdateChunk(parentNode, parentChunk);
        }
        else if (parentNode.Tag is P3DFile p3dFile)
        {
            for (var i = p3dFile.Chunks.Count - 1; i >= 0; i--)
                if (p3dFile.Chunks[i].GetType() == chunkType)
                    p3dFile.Chunks.RemoveAt(i);
            PopulateData();
        }
    }

    private void TSMIDeleteChildren_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        bool isShiftDown = (ModifierKeys & Keys.Shift) == Keys.Shift;
        if (!isShiftDown && MessageBox.Show($"Are you sure you want to delete all children of:\n{node.Text}?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        PreChange("Delete Children");
        UnsavedChanges = true;
        if (node.Tag is Chunk parentChunk)
            parentChunk.Children.Clear();
        else if (node.Tag is P3DFile p3dFile)
            p3dFile.Chunks.Clear();

        node.Nodes.Clear();
        UpdateErrors();
    }

    private void TSMIDuplicate_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        if (node.Tag is not Chunk chunk)
            return;

        var clone = chunk.Clone();
        var index = node.Index + 1;

        PreChange("Duplicate Chunk");
        var parentNode = node.Parent;
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.Insert(index, clone);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.Insert(index, clone);

        TVChunks.SelectedNode = AddChunk(parentNode, clone, index);
        UnsavedChanges = true;
    }

    private void TSMIRename_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        if (node.Tag is not NamedChunk chunk)
            return;

        using var stringEditor = new FrmStringEditor("Name", chunk.Name, 255);
        if (stringEditor.ShowDialog() != DialogResult.OK)
            return;

        PreChange("Rename Chunk");
        chunk.Name = stringEditor.Value;
        UpdateChunk(node, chunk);
    }

    private void TSMILittleEndian_CheckedChanged(object sender, EventArgs e)
    {
        if (TSMILittleEndian.Checked)
            TSMIBigEndian.Checked = false;
    }

    private void TSMIBigEndian_CheckedChanged(object sender, EventArgs e)
    {
        if (TSMIBigEndian.Checked)
            TSMILittleEndian.Checked = false;
    }

    private void TSMICompressed_CheckedChanged(object sender, EventArgs e)
    {
        TSMIEndianness.Enabled = !TSMICompressed.Checked;
    }

    private void PreChange(string change, P3DFile? clone = null)
    {
        UndoStack.Push(new(change, clone ?? P3DFile.Clone()));
        RedoStack.Clear();
    }

    private void TSMIUndo_Click(object sender, EventArgs e) => PerformUndoRedo(UndoStack, RedoStack, true);

    private void TSMIRedo_Click(object sender, EventArgs e) => PerformUndoRedo(RedoStack, UndoStack, false);

    private void PerformUndoRedo(Stack<UndoEntry> fromStack, Stack<UndoEntry> toStack, bool isUndo)
    {
        if (!fromStack.TryPop(out var entry))
            return;

        try
        {
            var currentFile = P3DFile.Clone();
            P3DFile = entry.OldFile;
            toStack.Push(new UndoEntry(entry.Change, currentFile));
            UnsavedChanges = true;

            var allNodes = new List<TreeNode>();
            foreach (TreeNode node in TVChunks.Nodes)
                CollectNodes(node, allNodes);

            var expandedPaths = new HashSet<string>(allNodes.Where(n => n.IsExpanded).Select(n => n.GetPathText()).Where(text => !string.IsNullOrWhiteSpace(text)));

            var selectedPath = TVChunks.SelectedNode?.GetPathText();

            PopulateData();

            RestoreTreeState(expandedPaths, selectedPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Failed to {(isUndo ? "undo" : "redo")}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void RestoreTreeState(HashSet<string> expandedPaths, string? selectedPath)
    {
        TVChunks.BeginUpdate();

        var allNodes = new List<TreeNode>();
        foreach (TreeNode node in TVChunks.Nodes)
            CollectNodes(node, allNodes);

        foreach (var node in allNodes)
        {
            var pathText = node.GetPathText();
            if (string.IsNullOrWhiteSpace(pathText))
                continue;

            if (expandedPaths.Contains(pathText))
                node.Expand();

            if (pathText == selectedPath)
                TVChunks.SelectedNode = node;
        }

        TVChunks.EndUpdate();
    }


    private void TSMIEdit_DropDownOpening(object sender, EventArgs e)
    {
        if (UndoStack.TryPeek(out var undo))
        {
            TSMIUndo.Text = $"Undo {undo.Change}";
            TSMIUndo.Enabled = true;
        }
        else
        {
            TSMIUndo.Text = "Undo";
            TSMIUndo.Enabled = false;
        }

        if (RedoStack.TryPeek(out var redo))
        {
            TSMIRedo.Text = $"Redo {redo.Change}";
            TSMIRedo.Enabled = true;
        }
        else
        {
            TSMIRedo.Text = "Redo";
            TSMIRedo.Enabled = false;
        }
    }

    private void TSMIOptions_Click(object sender, EventArgs e)
    {
        using var options = new FrmOptions();
        options.ShowDialog();
        UpdateErrors();
    }
}
