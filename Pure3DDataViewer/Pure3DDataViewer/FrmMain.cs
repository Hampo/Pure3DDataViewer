using Be.Windows.Forms;
using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Enums;
using NetP3DLib.P3D.Exceptions;
using Pure3DDataViewer.UndoRedo;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Editors;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Helpers;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Collections;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Pure3DDataViewer;

public partial class FrmMain : Form
{
    private CancellationTokenSource? _cts = null;
    private P3DFile? _p3dFile = null;
    private P3DFile P3DFile
    {
        get
        {
            if (_p3dFile == null)
            {
                _p3dFile = new();
                _p3dFile.ChunkAdded += P3DFile_ChunkAdded;
                _p3dFile.ChunkRemoved += P3DFile_ChunkRemoved;
                _p3dFile.ChunksAdded += P3DFile_ChunksAdded;
                _p3dFile.ChunksRemoved += P3DFile_ChunksRemoved;
                _p3dFile.ChunksCleared += P3DFile_ChunksCleared;
                _cts = new();
            }

            return _p3dFile;
        }
        set
        {
            if (_p3dFile == value)
                return;

            _cts?.Cancel();
            _cts = new();

            if (_p3dFile != null)
            {
                _p3dFile.ChunkAdded -= P3DFile_ChunkAdded;
                _p3dFile.ChunkRemoved -= P3DFile_ChunkRemoved;
                _p3dFile.ChunksAdded -= P3DFile_ChunksAdded;
                _p3dFile.ChunksRemoved -= P3DFile_ChunksRemoved;
                _p3dFile.ChunksCleared -= P3DFile_ChunksCleared;
            }

            _p3dFile = value;

            _p3dFile.ChunkAdded += P3DFile_ChunkAdded;
            _p3dFile.ChunkRemoved += P3DFile_ChunkRemoved;
            _p3dFile.ChunksAdded += P3DFile_ChunksAdded;
            _p3dFile.ChunksRemoved += P3DFile_ChunksRemoved;
            _p3dFile.ChunksCleared += P3DFile_ChunksCleared;
        }
    }

    private FileSystemWatcher? _watcher = null;
    private readonly CommandManager _undoRedoManager = new();

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
    private readonly Dictionary<Type, List<IChunkEditor>> _pluginChunkEditors = [];
    private readonly ToolStripSeparator _toolsFileChunkSeparator = new();
    private readonly BindingList<Editor> _editors = [];

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
        typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGVValues, [true]);

        _editors.Add(new("Values", DGVValues));
        _editors.Add(new("Hex Viewer", HBHex));
        CBEditor.DataSource = _editors;
        CBEditor.DisplayMember = "Name";
        CBEditor.ValueMember = "Control";
    }

    private void FrmMain_Load(object sender, EventArgs e)
    {
        string version = Application.ProductVersion.Split('+')[0];
        while (version.EndsWith(".0"))
            version = version[..^2];
        _Text = $"{Text} v{version}";
#if DEBUG
        _Text += "-Debug";
#endif
        UpdateText();

        Theming.ApplyTheme(this, Settings.DarkMode ? Theming.ThemeMode.Dark : Theming.ThemeMode.Light, Settings.LargeFont ? Theming.FontMode.Large : Theming.FontMode.Normal);
        SC1.SplitterDistance = Settings.SplitterDistance;

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
                    editor.Dock = DockStyle.Fill;
                    editor.Visible = false;
                    editor.Name = chunkEditor.GetType().FullName;
                    PnlEditors.Controls.Add(editor);

                    foreach (var type in types)
                    {
                        if (!_pluginChunkEditors.TryGetValue(type, out var editors))
                        {
                            _pluginChunkEditors[type] = [chunkEditor];
                            continue;
                        }
                        editors.Add(chunkEditor);
                    }
                }
            }
        }

        switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
        {
            case Endianness.Little:
                TSMILittleEndian.Checked = true;
                break;
            case Endianness.Big:
                TSMIBigEndian.Checked = true;
                break;
        }

        string[] args = Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            string file = args[1];
            if (File.Exists(file) && LoadP3DFile(file))
                return;
        }

        PopulateData();
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
                            break;
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.Deleted:
                            var parentNode = node.Parent;

                            if (parentNode.Tag is Chunk parentChunk)
                                parentChunk.Children.RemoveAt(node.Index);
                            else if (parentNode.Tag is P3DFile parentFile)
                                parentFile.Chunks.RemoveAt(node.Index);
                            PreChange($"{chunkHandler.Name}", clone);
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
        _undoRedoManager.Clear();
        P3DFile = new P3DFile();
        LastPath = string.Empty;
        PopulateData();

        _watcher?.Dispose();

        TSMICompressed.Checked = false;
        switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
        {
            case Endianness.Little:
                TSMILittleEndian.Checked = true;
                break;
            case Endianness.Big:
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

    private static readonly HashSet<string> ExcludedProperties = ["DataBytes", "DataLength", "ID", "ParentFile", "ParentChunk", "IndexInParent", "Children", "AllChildren", "HeaderSize", "Size", "Bytes"];
    private bool _afterSelectUpdating = false;
    private void TVChunks_AfterSelect(object sender, TreeViewEventArgs e)
    {
        _afterSelectUpdating = true;
        var prevFocus = SC1.ActiveControl;
        DGVValues.SuspendLayout();
        DGVValues.Rows.Clear();

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
                    var errors = new List<InvalidP3DException>();

                    double index = 0;
                    foreach (var child in p3dFile.Chunks)
                    {
                        if (isCancellationRequested())
                            break;

                        errors.AddRange(child.ValidateChunks());

                        reportProgress((int)(index++ / p3dFile.Chunks.Count * 100));
                    }

                    return errors;
                });

                if (!cancelled)
                {
                    var (backColour, foreColour) = Settings.GetErrorChunkColour();
                    foreach (var error in errors)
                    {
                        var rowIndex = DGVValues.Rows.Add("Validation Error", $"Error in chunk \"{error.Chunk!.IndexInParent}. {error.Chunk}\": {error.Message}");
                        var row = DGVValues.Rows[rowIndex];
                        row.Tag = error.Chunk;

                        row.DefaultCellStyle.BackColor = backColour;
                        row.DefaultCellStyle.ForeColor = foreColour;
                    }
                }

                DGVValues.Rows.Add("Size", $"{p3dFile.Size:N0} bytes");

                if (HBHex.ByteProvider is ChunkByteProvider oldProvider)
                    oldProvider.Dispose();
                HBHex.ByteProvider = new DynamicByteProvider(p3dFile.Size > int.MaxValue ? Encoding.UTF8.GetBytes("Too large") : p3dFile.Bytes);

                foreach (var (FileHandler, _, ContextMenu) in _pluginFileHandlers)
                    if (FileHandler.IsFileSupported(p3dFile))
                        CMSTVChunks.Items.Add(ContextMenu);

                for (int i = _editors.Count - 1; i >= 2; i--)
                    _editors.RemoveAt(i);

                CBEditor.SelectedIndex = Settings.GetLastEditor(_editors, typeof(P3DFile));

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

                for (int i = _editors.Count - 1; i >= 2; i--)
                    if (!_editors[i].ChunkEditor?.ChunkTypes.Contains(chunkType) ?? false)
                        _editors.RemoveAt(i);

                if (_pluginChunkEditors.TryGetValue(chunkType, out var editors))
                    foreach (var editor in editors)
                        if (!_editors.Any(x => x.ChunkEditor == editor))
                            _editors.Add(new(editor.Name, editor.Editor, editor));

                CBEditor.SelectedIndex = Settings.GetLastEditor(_editors, chunkType);

                var (backColour, foreColour) = Settings.GetErrorChunkColour();
                foreach (var error in chunk.ValidateChunk())
                {
                    var rowIndex = DGVValues.Rows.Add("Validation Error", error.Message);
                    var row = DGVValues.Rows[rowIndex];

                    row.DefaultCellStyle.BackColor = backColour;
                    row.DefaultCellStyle.ForeColor = foreColour;
                }
                foreach (var child in chunk.Children)
                {
                    foreach (var error in child.ValidateChunks())
                    {
                        var rowIndex = DGVValues.Rows.Add("Validation Error", $"Error in child \"{error.Chunk!.IndexInParent}. {error.Chunk}\": {error.Message}");
                        var row = DGVValues.Rows[rowIndex];
                        row.Tag = error.Chunk;

                        row.DefaultCellStyle.BackColor = backColour;
                        row.DefaultCellStyle.ForeColor = foreColour;
                    }
                }

                var properties = PropertyHelper.GetProperties(chunkType);

                foreach (var property in properties)
                {
                    if (ExcludedProperties.Contains(property.Name))
                        continue;

                    object? value = property.GetValue(chunk);
                    if (value is byte[] byteArray)
                    {
                        var rowIndex = DGVValues.Rows.Add(property.Name, $"{byteArray.Length:N0} bytes");
                        DGVValues.Rows[rowIndex].Tag = property;
                    }
                    else if (property.IsEnumerable() && value is IEnumerable enumerable)
                    {
                        List<object> values = [.. enumerable.Cast<object>()];
                        if (values.Count == 0)
                        {
                            var rowIndex = DGVValues.Rows.Add($"{property.Name}[<EMPTY>]", "<NULL>");
                            DGVValues.Rows[rowIndex].Tag = (property, 0);
                        }
                        else
                        {
                            for (int i = 0; i < values.Count; i++)
                            {
                                var rowIndex = DGVValues.Rows.Add($"{property.Name}[{i}]", values[i]?.ToString() ?? "<NULL>");
                                DGVValues.Rows[rowIndex].Tag = (property, i);
                            }
                        }
                    }
                    else
                    {
                        var rowIndex = DGVValues.Rows.Add($"{property.Name}", value?.ToString() ?? "<NULL>");
                        if (property.CanWrite)
                            DGVValues.Rows[rowIndex].Tag = property;
                        else
                            DGVValues.Rows[rowIndex].ReadOnly = true;
                    }
                }

                if (HBHex.ByteProvider is ChunkByteProvider oldProvider)
                    oldProvider.Dispose();
                HBHex.ByteProvider = new ChunkByteProvider(chunk);

                return;
            }
        }
        finally
        {
            var rowCount = DGVValues.RowCount;
            if (rowCount > 0)
            {
                for (int i = 0; i < rowCount; i++)
                {
                    var row = DGVValues.Rows[i];
                    if (row.Cells[1].Value as string != "Validation Error")
                    {
                        row.Selected = true;
                        break;
                    }
                }
            }

            AutoSizeSmart();

            DGVValues.ResumeLayout();

            _afterSelectUpdating = false;
            if (prevFocus != null && prevFocus.CanFocus)
                prevFocus.Focus();

            if (tag is Chunk chunk)
                foreach (var editorControl in PnlEditors.Controls.OfType<EditorControl>().Where(x => x.Visible))
                    editorControl.LoadChunk(chunk);
        }
    }

    private bool _autosizePending = false;
    private void DGVValues_Resize(object sender, EventArgs e)
    {
        if (_autosizePending)
            return;

        _autosizePending = true;

        DGVValues.BeginInvoke(() =>
        {
            _autosizePending = false;
            AutoSizeSmart();
        });
    }

    private void AutoSizeSmart()
    {
        if (!DGVValues.IsHandleCreated || DGVValues.Columns.Count == 0 || DGVValues.IsDisposed)
            return;

        DGVValues.SuspendLayout();

        int contentWidth = DGVValues.RowHeadersVisible ? DGVValues.RowHeadersWidth : 0;
        foreach (DataGridViewColumn col in DGVValues.Columns)
        {
            contentWidth += col.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
        }

        if (contentWidth < DGVValues.DisplayRectangle.Width)
        {
            DGVValues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        else
        {
            DGVValues.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        DGVValues.ResumeLayout();
    }

    private async void DGVValues_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.RowIndex == -1)
            return;

        if (e.Button != MouseButtons.Left)
            return;

        var row = DGVValues.Rows[e.RowIndex];
        if (row.Tag is Chunk errorChunk)
        {
            var chunkNode = await GetTreeNodeFromChunk(errorChunk);
            TVChunks.SelectedNode = chunkNode;
            return;
        }

        if (TVChunks.SelectedNode?.Tag is not Chunk chunk)
            return;


        var Updated = false;
        var clone = P3DFile.Clone();
        switch (row.Tag)
        {
            case PropertyInfo property:
                Updated = EditProperty(property, chunk);
                break;
            case (PropertyInfo listProperty, int index):
                Updated = EditProperty(listProperty, chunk, index);
                break;
        }

        if (Updated)
            PreChange("Update Value", clone);
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

    private readonly Stack<TreeNode> _autoExpandedNodes = [];
    private void TmrTVHover_Tick(object sender, EventArgs e)
    {
        if (TmrTVHover.Tag is TreeNode node && !node.IsExpanded)
        {
            TVChunks.BeginUpdate();
            var topNode = TVChunks.TopNode;

            node.Expand();
            _autoExpandedNodes.Push(node);

            TVChunks.TopNode = topNode;
            TVChunks.EndUpdate();
        }
        TmrTVHover.Stop();
    }

    private void TVChunks_ItemDrag(object sender, ItemDragEventArgs e)
    {
        if (e.Item != null && e.Item != TVChunks.Nodes[0])
            DoDragDrop(e.Item, DragDropEffects.Move);
    }

    private void TVChunks_DragEnter(object sender, DragEventArgs e)
    {
        if (e.Data == null)
            e.Effect = DragDropEffects.None;
        else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            e.Effect = DragDropEffects.Copy;
        else if (e.Data.GetDataPresent(typeof(TreeNode)) && ((TreeNode)e.Data.GetData(typeof(TreeNode))!).Tag is Chunk)
            e.Effect = DragDropEffects.Move;
        else
            e.Effect = DragDropEffects.None;
    }

    private void TVChunks_DragOver(object sender, DragEventArgs e)
    {
        if (e.Data == null || !e.Data.GetDataPresent(typeof(TreeNode)) || ((TreeNode)e.Data.GetData(typeof(TreeNode))!).Tag is not Chunk)
            return;

        var targetPoint = TVChunks.PointToClient(new(e.X, e.Y));
        var targetNode = TVChunks.GetNodeAt(targetPoint);

        while (_autoExpandedNodes.Count > 0)
        {
            var lastExpanded = _autoExpandedNodes.Peek();

            if (lastExpanded == targetNode)
                break;

            if (targetNode != null && ContainsNode(lastExpanded, targetNode))
                break;

            lastExpanded.Collapse();
            _autoExpandedNodes.Pop();
        }
        targetNode = TVChunks.GetNodeAt(targetPoint);

        if (targetNode != TmrTVHover.Tag)
        {
            TmrTVHover.Tag = targetNode;
            TmrTVHover.Stop();

            if (targetNode != null && !targetNode.IsExpanded)
                TmrTVHover.Start();
        }

        if (targetNode == null)
        {
            TVChunks.SetInsertMark(null, false);
            e.Effect = DragDropEffects.None;
        }
        else
        {
            var bounds = targetNode.Bounds;
            float relativeY = targetPoint.Y - bounds.Top;
            float threshold = bounds.Height * 0.25f;

            if (relativeY < threshold)
                TVChunks.SetInsertMark(targetNode, false);
            else if (relativeY > bounds.Height - threshold)
                TVChunks.SetInsertMark(targetNode, true);
            else
                TVChunks.SetInsertMark(null, false);

            TVChunks.SelectedNode = targetNode;
            e.Effect = DragDropEffects.Move;
        }
    }

    private void TVChunks_DragDrop(object sender, DragEventArgs e)
    {
        if (e.Data == null)
            return;

        if (e.Data.GetDataPresent(DataFormats.FileDrop))
        {

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

            return;
        }

        if (e.Data.GetDataPresent(typeof(TreeNode)) && ((TreeNode)e.Data.GetData(typeof(TreeNode))!).Tag is Chunk draggedChunk)
        {
            var targetPoint = TVChunks.PointToClient(new(e.X, e.Y));
            var targetNode = TVChunks.GetNodeAt(targetPoint);
            if (targetNode == null)
            {
                TVChunks.SetInsertMark(null, false);
                return;
            }

            var draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode))!;
            if (targetNode == draggedNode)
            {
                TVChunks.SetInsertMark(null, false);
                return;
            }
            if (ContainsNode(draggedNode, targetNode))
            {
                TVChunks.SetInsertMark(null, false);
                return;
            }

            TreeNode newParentNode;
            int newIndex;

            var bounds = targetNode.Bounds;
            float relativeY = targetPoint.Y - bounds.Top;
            float threshold = bounds.Height * 0.25f;

            if (relativeY < threshold)
            {
                newParentNode = targetNode.Parent;
                newIndex = targetNode.Index;
            }
            else if (relativeY > bounds.Height - threshold)
            {
                newParentNode = targetNode.Parent;
                newIndex = targetNode.Index + 1;
            }
            else
            {
                newParentNode = targetNode;
                newIndex = targetNode.Nodes.Count;
            }

            PreChange("Move Chunk");
            UnsavedChanges = true;
            MoveChunkData(draggedChunk, newParentNode.Tag, newIndex);
            TVChunks.SelectedNode = draggedNode;
            newParentNode.Expand();

            TVChunks.SetInsertMark(null, false);
            _autoExpandedNodes.Clear();

            return;
        }
    }

    private void TVChunks_DragLeave(object sender, EventArgs e)
    {
        TVChunks.SetInsertMark(null, false);
        while (_autoExpandedNodes.Count > 0)
        {
            var node = _autoExpandedNodes.Pop();
            node.Collapse();
        }
    }

    private static bool ContainsNode(TreeNode parent, TreeNode potentialChild)
    {
        if (potentialChild.Parent == null)
            return false;
        if (potentialChild.Parent == parent)
            return true;
        return ContainsNode(parent, potentialChild.Parent);
    }

    private static void MoveChunkData(Chunk chunk, object targetTag, int index)
    {
        if (chunk.ParentFile != null)
            chunk.ParentFile.Chunks.RemoveAt(chunk.IndexInParent);
        else
            chunk.ParentChunk?.Children.RemoveAt(chunk.IndexInParent);

        switch (targetTag)
        {
            case P3DFile parentFile:
                if (index > parentFile.Chunks.Count)
                    index = parentFile.Chunks.Count;

                parentFile.Chunks.Insert(index, chunk);

                break;
            case Chunk parentChunk:
                if (index > parentChunk.Children.Count)
                    index = parentChunk.Children.Count;

                parentChunk.Children.Insert(index, chunk);

                break;
        }
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

    private async void TSMIFindNext_Click(object sender, EventArgs e) => await Find(_searchQuery);

    private string _searchQuery = string.Empty;
    public async Task Find(string searchQuery)
    {
        if (string.IsNullOrEmpty(searchQuery))
            return;

        _searchQuery = searchQuery;

        var found = await FindNextNode(searchQuery);
        if (found == null)
        {
            MessageBox.Show("Reached end of file.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        TVChunks.SelectedNode = found;
        found.EnsureVisible();
    }

    private async Task<TreeNode?> FindNextNode(string searchQuery)
    {
        if (P3DFile == null || P3DFile.Chunks.Count == 0)
            return null;

        IList<Chunk> allChunks = P3DFile.AllChunks;

        if (!Settings.FindDirection)
            allChunks = [..allChunks.Reverse()];

        var startIndex = 0;
        if (TVChunks.SelectedNode?.Tag is Chunk selectedChunk)
        {
            startIndex = 1;

            var currentChunk = selectedChunk;
            while (currentChunk.ParentChunk != null)
            {
                startIndex++;
                for (var i = 0; i < currentChunk.IndexInParent; i++)
                    startIndex += currentChunk.ParentChunk.Children[i].AllChildren.Count;

                currentChunk = currentChunk.ParentChunk;
            }

            for (var i = 0; i < currentChunk.IndexInParent; i++)
                startIndex += currentChunk.ParentFile!.Chunks[i].AllChildren.Count + 1;
        }

        var comparison = Settings.FindMatchCase ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;

        Chunk? foundChunk = null;

        for (int i = startIndex + 1; i < allChunks.Count; i++)
        {
            var chunk = allChunks[i];
            if (chunk.ToString().Contains(searchQuery, comparison))
            {
                foundChunk = chunk;
                break;
            }

            if (Settings.FindIncludeProperties && SearchChunkProperties(chunk, searchQuery, comparison))
            {
                foundChunk = chunk;
                break;
            }
        }

        if (foundChunk == null && Settings.FindWrapAround)
        {
            for (int i = 0; i <= startIndex; i++)
            {
                var chunk = allChunks[i];
                if (chunk.ToString().Contains(searchQuery, comparison))
                {
                    foundChunk = chunk;
                    break;
                }

                if (Settings.FindIncludeProperties && SearchChunkProperties(chunk, searchQuery, comparison))
                {
                    foundChunk = chunk;
                    break;
                }
            }
        }

        if (foundChunk == null)
            return null;

        return await GetTreeNodeFromChunk(foundChunk);
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
        var index = node.Index;

        PreChange("Chunk Cut");
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.RemoveAt(index);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.RemoveAt(index);
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
                using var ms = new MemoryStream(bytes, false);
                using var br = new EndianAwareBinaryReader(ms);
                var chunk = ChunkLoader.LoadChunk(br, out _);
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
            parentNode.Expand();
            parentNode.Nodes[index].EnsureVisible();
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
            parentNode.Expand();
            parentNode.Nodes[index].EnsureVisible();
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
            node.Expand();
            node.Nodes[chunk.IndexInParent].EnsureVisible();
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
                node.Expand();
                node.Nodes[newChunk.IndexInParent].EnsureVisible();
                _undoRedoManager.Execute(new AddChunkCommand("New Chunk", GetChunkHierarchy(newChunk)!, newChunk));
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

    private void TSMIDeleteThis_Click(object sender, EventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        if (node.Tag is not Chunk chunk)
            return;

        bool isShiftDown = (ModifierKeys & Keys.Shift) == Keys.Shift;
        if (!isShiftDown && MessageBox.Show($"Are you sure you want to delete the selected chunk:\n{node.Text}?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
            return;

        var parentNode = node.Parent;

        var hierarchy = GetChunkHierarchy(chunk)!;
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.RemoveAt(node.Index);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.RemoveAt(node.Index);
        _undoRedoManager.Execute(new DeleteChunkCommand("Delete Chunk", hierarchy, chunk));
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

        if (parentNode.Tag is Chunk parentChunk)
        {
            TVChunks.SelectedNode = parentNode;
            var indices = new List<int>(parentChunk.Children.Count);
            for (var i = 0; i < parentChunk.Children.Count; i++)
                if (parentChunk.Children[i].GetType() == chunkType)
                    indices.Add(i);

            var beforeChunk = chunk.Clone();
            parentChunk.Children.RemoveAtIndices(indices);
            _undoRedoManager.Execute(new UpdateChunkCommand("Delete Type", GetChunkHierarchy(parentChunk)!, beforeChunk, parentChunk));
        }
        else if (parentNode.Tag is P3DFile p3dFile)
        {
            var indices = new List<int>(p3dFile.Chunks.Count);
            for (var i = 0; i < p3dFile.Chunks.Count; i++)
                if (p3dFile.Chunks[i].GetType() == chunkType)
                    indices.Add(i);

            var beforeFile = p3dFile.Clone();
            p3dFile.Chunks.RemoveAtIndices(indices);
            _undoRedoManager.Execute(new FileCommand("Delete Type", beforeFile, p3dFile));
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

        if (node.Tag is Chunk parentChunk)
        {
            var beforeChunk = parentChunk.Clone();
            parentChunk.Children.Clear();
            _undoRedoManager.Execute(new UpdateChunkCommand("Delete Children", GetChunkHierarchy(parentChunk)!, beforeChunk, parentChunk));
        }
        else if (node.Tag is P3DFile p3dFile)
        {
            var beforeFile = p3dFile.Clone();
            p3dFile.Chunks.Clear();
            _undoRedoManager.Execute(new FileCommand("Delete Children", beforeFile, p3dFile));
        }

        foreach (TreeNode childNode in node.Nodes)
            UnsubscribeNode(childNode);
        node.Nodes.Clear();
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

        var parentNode = node.Parent;
        if (parentNode.Tag is Chunk parentChunk)
            parentChunk.Children.Insert(index, clone);
        else if (parentNode.Tag is P3DFile p3dFile)
            p3dFile.Chunks.Insert(index, clone);

        _undoRedoManager.Execute(new AddChunkCommand("Duplicate Chunk", GetChunkHierarchy(clone)!, clone));

        TVChunks.SelectedNode = parentNode.Nodes[index];
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

        var beforeChunk = chunk.Clone();
        chunk.Name = stringEditor.Value;
        _undoRedoManager.Execute(new UpdateChunkCommand("Rename Chunk", GetChunkHierarchy(chunk)!, beforeChunk, chunk));
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

    private void TSMIUndo_Click(object sender, EventArgs e) => _undoRedoManager.Undo(P3DFile);

    private void TSMIRedo_Click(object sender, EventArgs e) => _undoRedoManager.Redo(P3DFile);

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
        var undoChange = _undoRedoManager.UndoChange;
        if (!string.IsNullOrEmpty(undoChange))
        {
            TSMIUndo.Text = $"Undo {undoChange}";
            TSMIUndo.Enabled = true;
        }
        else
        {
            TSMIUndo.Text = "Undo";
            TSMIUndo.Enabled = false;
        }

        var redoChange = _undoRedoManager.RedoChange;
        if (!string.IsNullOrEmpty(redoChange))
        {
            TSMIRedo.Text = $"Redo {redoChange}";
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
        UpdateChunkColours();
    }

    private void CBEditor_SelectedIndexChanged(object sender, EventArgs e)
    {
        var editor = (Editor?)CBEditor.SelectedItem;
        if (!editor.HasValue)
            return;

        var tag = TVChunks.SelectedNode?.Tag;
        if (tag == null)
            return;

        var editorType = editor.Value.Control.GetType();
        foreach (Control control in PnlEditors.Controls)
        {
            var controlType = control.GetType();
            var visible = controlType == editorType;
            control.Visible = visible;
            if (visible && control is EditorControl editorControl && tag is Chunk chunk)
                editorControl.LoadChunk(chunk);
        }

        if (!_afterSelectUpdating)
            Settings.SetLastEditor(tag.GetType(), editor.Value.Control);
    }

    public bool LoadP3DFile(string filePath)
    {
        if (!File.Exists(filePath))
        {
            MessageBox.Show($"Could not find P3D file: {filePath}", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        try
        {
            var p3dFile = new P3DFile(filePath);

            _undoRedoManager.Clear();
            UnsavedChanges = false;

            LastPath = filePath;
            P3DFile = p3dFile;

            PopulateData();

            Settings.AddRecentFile(LastPath);

            _watcher?.Dispose();
            _watcher = new FileSystemWatcher(Path.GetDirectoryName(filePath)!)
            {
                Filter = Path.GetFileName(filePath)!,
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true,
            };
            _watcher.Changed += FileChanged;

            var originalBytes = File.ReadAllBytes(filePath);
            var originalSignature = BitConverter.ToUInt32(originalBytes);
            switch (originalSignature)
            {
                case P3DFile.COMPRESSED_SIGNATURE:
                    TSMICompressed.Checked = true;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case Endianness.Little:
                            TSMILittleEndian.Checked = true;
                            break;
                        case Endianness.Big:
                            TSMIBigEndian.Checked = true;
                            break;
                    }
                    break;
                case P3DFile.COMPRESSED_SIGNATURE_SWAP:
                    TSMICompressed.Checked = false; // TODO: When compressed endian supported, change this
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case Endianness.Little:
                            TSMIBigEndian.Checked = true;
                            break;
                        case Endianness.Big:
                            TSMILittleEndian.Checked = true;
                            break;
                    }
                    UnsavedChanges = true;
                    MessageBox.Show($"Detected that the opened file is both compressed and has an endian that doesn't match the system's.\nIt is currently not possible to compress a file in a swapped endian.\nSaving will either remove compression or flip endian.", "Compression and endian mismatch detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;
                case P3DFile.SIGNATURE_SWAP:
                    TSMICompressed.Checked = false;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case Endianness.Little:
                            TSMIBigEndian.Checked = true;
                            break;
                        case Endianness.Big:
                            TSMILittleEndian.Checked = true;
                            break;
                    }
                    break;
                case P3DFile.SIGNATURE:
                    TSMICompressed.Checked = false;
                    switch (NetP3DLib.P3D.Extensions.BinaryExtensions.DefaultEndian)
                    {
                        case Endianness.Little:
                            TSMILittleEndian.Checked = true;
                            break;
                        case Endianness.Big:
                            TSMIBigEndian.Checked = true;
                            break;
                    }
                    break;
            }

            var newBytes = new byte[p3dFile.Size];
            using var ms = new MemoryStream(newBytes);
            if (!TSMICompressed.Checked)
                p3dFile.Write(ms, TSMILittleEndian.Checked ? Endianness.Little : Endianness.Big, false);
            else
                newBytes = LZR_Compression.CompressFile(p3dFile, false, false);

            if (!originalBytes.SequenceEqual(newBytes))
            {
                UnsavedChanges = true;
                MessageBox.Show($"Detected that the opened file has changed values.\n\nThis is likely caused by one of the following:\n- The file contains chunks with incorrect property values that were auto corrected.\n- Some Radical files released with SHAR contain incorrect chunk headers.\n- The file contains different string padding than expected.\n\nSaving is recommended, but will result in a modified file.", "Changes detected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading P3D file: {ex}", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }
    }

    private void P3DFile_ChunkAdded(Chunk newChunk)
    {
        if (TVChunks.Nodes.Count > 0)
        {
            UnsavedChanges = true;
            InsertChunkNode(TVChunks.Nodes[0], newChunk);

            if (!TVChunks.Nodes[0].IsExpanded)
                TVChunks.Nodes[0].Expand();
        }
    }

    private void P3DFile_ChunkRemoved(Chunk removedChunk, int oldIndex)
    {
        if (TVChunks.Nodes.Count > 0)
        {
            UnsavedChanges = true;
            RemoveChunkNode(TVChunks.Nodes[0], removedChunk, oldIndex);
        }
    }

    private void P3DFile_ChunksAdded(IReadOnlyList<Chunk> newChunks)
    {
        if (TVChunks.Nodes.Count > 0)
        {
            UnsavedChanges = true;
            TVChunks.BeginUpdate();
            foreach (var newChunk in newChunks)
                InsertChunkNode(TVChunks.Nodes[0], newChunk, false);
            UpdateChunkIndices(TVChunks.Nodes[0], newChunks[0].IndexInParent);

            if (!TVChunks.Nodes[0].IsExpanded)
                TVChunks.Nodes[0].Expand();
            TVChunks.EndUpdate();
        }
    }

    private void P3DFile_ChunksRemoved(IReadOnlyList<(Chunk chunk, int oldIndex)> removedChunks)
    {
        if (TVChunks.Nodes.Count > 0)
        {
            UnsavedChanges = true;
            TVChunks.BeginUpdate();
            var firstIndex = 0;
            foreach (var (removedChunk, oldIndex) in removedChunks.OrderByDescending(x => x.oldIndex))
            {
                RemoveChunkNode(TVChunks.Nodes[0], removedChunk, oldIndex, false);
                firstIndex = oldIndex;
            }
            UpdateChunkIndices(TVChunks.Nodes[0], firstIndex);
            TVChunks.EndUpdate();
        }
    }

    private void P3DFile_ChunksCleared(IReadOnlyList<(Chunk chunk, int oldIndex)> children)
    {
        if (TVChunks.Nodes.Count > 0)
        {
            UnsavedChanges = true;
            TVChunks.BeginUpdate();
            foreach (TreeNode node in TVChunks.Nodes[0].Nodes)
                UnsubscribeNode(node);
            TVChunks.Nodes[0].Nodes.Clear();
            TVChunks.EndUpdate();
        }
    }

    private void InsertChunkNode(TreeNode parentNode, Chunk newChild, bool updateChunkIndices = true)
    {
        if (newChild.IndexInParent == -1)
            return;

        if (InvokeRequired)
        {
            BeginInvoke(() => InsertChunkNode(parentNode, newChild));
            return;
        }

        if (!parentNode.IsExpanded)
        {
            if (parentNode.Nodes.Count == 0)
                parentNode.Nodes.Add("<<DUMMY>>");

            return;
        }

        TVChunks.BeginUpdate();
        if (parentNode.Nodes.Count == 1 && parentNode.Nodes[0].Text == "<<DUMMY>>")
            parentNode.Nodes.Clear();

        int index = newChild.IndexInParent;

        var childNode = new TreeNode($"{index}. {newChild}")
        {
            Tag = newChild
        };
        ApplyNodeStyling(childNode, newChild, newChild.ValidateChunks().Any());

        if (newChild.Children.Count > 0)
            childNode.Nodes.Add("<<DUMMY>>");

        SubscribeNode(childNode, newChild);

        try
        {
            if (index >= parentNode.Nodes.Count)
                parentNode.Nodes.Add(childNode);
            else
                parentNode.Nodes.Insert(index, childNode);
            if (updateChunkIndices)
                UpdateChunkIndices(parentNode, index);
        }
        finally
        {
            TVChunks.EndUpdate();
        }
    }

    private void RemoveChunkNode(TreeNode parentNode, Chunk removedChild, int oldIndex, bool updateChunkIndices = true)
    {
        if (InvokeRequired)
        {
            BeginInvoke(() => RemoveChunkNode(parentNode, removedChild, oldIndex));
            return;
        }

        if (!parentNode.IsExpanded)
        {
            bool isParentEmpty = parentNode.Tag is P3DFile p3d ? p3d.Chunks.Count == 0 :
                                 parentNode.Tag is Chunk chunk ? chunk.Children.Count == 0 : true;

            if (isParentEmpty)
                parentNode.Nodes.Clear();
            return;
        }

        if (oldIndex < 0 || oldIndex >= parentNode.Nodes.Count)
            return;

        TreeNode removedNode = parentNode.Nodes[oldIndex];

        TVChunks.BeginUpdate();
        try
        {
            int removedIndex = removedNode.Index;
            UnsubscribeNode(removedNode);

            if (removedNode.NextNode != null)
                TVChunks.SelectedNode = removedNode.NextNode;
            else if (removedNode.PrevNode != null)
                TVChunks.SelectedNode = removedNode.PrevNode;
            else
                TVChunks.SelectedNode = parentNode;

            removedNode.Remove();
            if (updateChunkIndices)
                UpdateChunkIndices(parentNode, removedIndex);
        }
        finally
        {
            TVChunks.EndUpdate();
        }
    }

    private void PopulateData()
    {
        TVChunks.BeginUpdate();
        try
        {
            foreach (TreeNode node in TVChunks.Nodes)
                UnsubscribeNode(node);
            TVChunks.Nodes.Clear();

            var rootNode = new TreeNode(string.IsNullOrWhiteSpace(LastPath) ? "Untitled" : LastPath)
            {
                Tag = P3DFile
            };

            if (P3DFile.Chunks.Count > 0)
                rootNode.Nodes.Add("<<DUMMY>>");

            var errors = P3DFile.Chunks.Any(x => x.ValidateChunks().Any());
            if (errors)
            {
                var (back, fore) = Settings.GetErrorChunkColour();

                rootNode.BackColor = back;
                rootNode.ForeColor = fore;
            }

            TVChunks.Nodes.Add(rootNode);
            rootNode.Expand();
            TVChunks.SelectedNode = rootNode;
        }
        finally
        {
            TVChunks.EndUpdate();
        }
    }

    private int _loadingChunks = 0;
    private async void TVChunks_BeforeExpand(object sender, TreeViewCancelEventArgs e)
    {
        var node = e.Node;
        if (node == null || !(node.Nodes.Count == 1 && node.Nodes[0].Text == "<<DUMMY>>"))
            return;

        _loadingChunks++;
        node.Nodes.Clear();

        var token = _cts!.Token;

        try
        {
            IList<Chunk> childChunks;
            if (node.Tag is P3DFile p3dFile)
                childChunks = p3dFile.Chunks;
            else if (node.Tag is Chunk chunk)
                childChunks = chunk.Children;
            else
                return;

            const int batchSize = 50;
            for (var i = 0; i < childChunks.Count; i += batchSize)
            {
                token.ThrowIfCancellationRequested();

                var currentBatchSize = Math.Min(batchSize, childChunks.Count - i);

                var newNodes = new List<TreeNode>(currentBatchSize);
                var errorColours = Settings.GetErrorChunkColour();
                var chunkColours = new Dictionary<Type, (Color BackColour, Color ForeColour)>();
                for (var j = 0; j < currentBatchSize; j++)
                {
                    token.ThrowIfCancellationRequested();

                    var index = i + j;
                    var child = childChunks[index];
                    if (child.IndexInParent == -1)
                        continue;
                    var childNode = new TreeNode($"{index}. {child}")
                    {
                        Tag = child
                    };
                    var childType = child.GetType();

                    (Color BackColour, Color ForeColour) colours;
                    if (child.ValidateChunks().Any())
                    {
                        colours = errorColours;
                    }
                    else if (!chunkColours.TryGetValue(childType, out colours))
                    {
                        colours = Settings.GetChunkColour(childType);
                        chunkColours[childType] = colours;
                    }

                    if (childNode.BackColor != colours.BackColour)
                        childNode.BackColor = colours.BackColour;

                    if (childNode.ForeColor != colours.ForeColour)
                        childNode.ForeColor = colours.ForeColour;

                    SubscribeNode(childNode, child);

                    if (child.Children.Count > 0)
                        childNode.Nodes.Add("<<DUMMY>>");

                    newNodes.Add(childNode);
                }

                node.Nodes.AddRange([.. newNodes]);

                if (i + batchSize < childChunks.Count)
                    await Task.Delay(1, token);
            }
        }
        catch (OperationCanceledException)
        { }
        finally
        {
            _loadingChunks--;
            if (_loadingChunks == 0)
            {

            }
        }
    }

    private static void ApplyNodeStyling(TreeNode node, Chunk chunk, bool hasError)
    {
        var (back, fore) = hasError ? Settings.GetErrorChunkColour() : Settings.GetChunkColour(chunk.GetType());

        node.BackColor = back;
        node.ForeColor = fore;
    }

    private void UpdateChunkColours()
    {
        TVChunks.BeginUpdate();

        var errorColours = Settings.GetErrorChunkColour();
        var chunkColours = new Dictionary<Type, (Color BackColour, Color ForeColour)>();
        bool ValidateAndColour(TreeNode node)
        {
            if (node.Tag is not Chunk chunk)
                return false;
            var chunkType = chunk.GetType();

            bool childrenHaveErrors = false;
            foreach (TreeNode child in node.Nodes)
                childrenHaveErrors |= ValidateAndColour(child);

            bool selfHasErrors = (node.Nodes.Count == 1 && node.Nodes[0].Text == "<<DUMMY>>") ? chunk.ValidateChunks().Any() : chunk.ValidateChunk().Any();

            bool branchHasErrors = selfHasErrors || childrenHaveErrors;

            (Color BackColour, Color ForeColour) colours;
            if (branchHasErrors)
            {
                colours = errorColours;
            }
            else if (!chunkColours.TryGetValue(chunkType, out colours))
            {
                colours = Settings.GetChunkColour(chunkType);
                chunkColours[chunkType] = colours;
            }

            if (node.BackColor != colours.BackColour)
                node.BackColor = colours.BackColour;

            if (node.ForeColor != colours.ForeColour)
                node.ForeColor = colours.ForeColour;

            return branchHasErrors;
        }

        var rootNode = TVChunks.Nodes[0];
        bool globalErrorFound = false;
        foreach (TreeNode node in rootNode.Nodes)
            globalErrorFound |= ValidateAndColour(node);

        var (back, fore) = globalErrorFound ? errorColours : (Color.Empty, Color.Empty);

        if (rootNode.BackColor != back)
            rootNode.BackColor = back;

        if (rootNode.ForeColor != fore)
            rootNode.ForeColor = fore;

        TVChunks.EndUpdate();
    }

    private readonly Dictionary<TreeNode, Action> _nodeCleanups = [];
    private void SubscribeNode(TreeNode node, Chunk chunk)
    {
        void OnPropertyChanged(string propertyName)
        {
            UnsavedChanges = true;

            TVChunks.BeginUpdate();

            var text = $"{node.Index}. {chunk}";
            if (node.Text != text)
                node.Text = text;

            if (node.IsSelected)
            {
                var firstRowIndex = DGVValues.RowCount > 0 ? DGVValues.FirstDisplayedScrollingRowIndex : -1;
                var selectedIndex = DGVValues.SelectedRows.Count > 0 ? DGVValues.SelectedRows[0].Index : -1;

                DGVValues.SuspendLayout();

                for (int i = DGVValues.RowCount - 1; i >= 0; i--)
                {
                    var row = DGVValues.Rows[i];

                    if (row.Cells[1].Value as string == "Validation Error")
                    {
                        DGVValues.Rows.RemoveAt(i);
                        continue;
                    }

                    switch (row.Tag)
                    {
                        case PropertyInfo property:
                            var value = property.GetValue(chunk);
                            if (value is byte[] byteArray)
                                row.Cells[1].Value = $"{byteArray.Length:N0} bytes";
                            else
                                row.Cells[1].Value = value?.ToString() ?? "<NULL>";
                            break;
                        case (PropertyInfo listProperty, int index):
                            DGVValues.Rows.RemoveAt(i);

                            if (index == 0)
                            {
                                var enumerable = (IEnumerable)listProperty.GetValue(chunk)!;
                                List<object> values = [.. enumerable.Cast<object>()];
                                if (values.Count == 0)
                                {
                                    DGVValues.Rows.Insert(i, $"{listProperty.Name}[<EMPTY>]", "<NULL>");
                                    DGVValues.Rows[i].Tag = (listProperty, 0);
                                }
                                else
                                {
                                    for (int j = values.Count - 1; j >= 0; j--)
                                    {
                                        DGVValues.Rows.Insert(i, $"{listProperty.Name}[{j}]", values[j]?.ToString() ?? "<NULL>");
                                        DGVValues.Rows[i].Tag = (listProperty, j);
                                    }
                                }
                            }

                            break;
                    }
                }

                var (backColour, foreColour) = Settings.GetErrorChunkColour();
                foreach (var error in chunk.ValidateChunk())
                {
                    var rowIndex = DGVValues.Rows.Add("Validation Error", error.Message);
                    var row = DGVValues.Rows[rowIndex];

                    row.DefaultCellStyle.BackColor = backColour;
                    row.DefaultCellStyle.ForeColor = foreColour;
                }
                foreach (var child in chunk.Children)
                {
                    foreach (var error in child.ValidateChunks())
                    {
                        var rowIndex = DGVValues.Rows.Add("Validation Error", $"Error in child \"{error.Chunk!.IndexInParent}. {error.Chunk}\": {error.Message}");
                        var row = DGVValues.Rows[rowIndex];
                        row.Tag = error.Chunk;

                        row.DefaultCellStyle.BackColor = backColour;
                        row.DefaultCellStyle.ForeColor = foreColour;
                    }
                }

                if (selectedIndex >= 0)
                    DGVValues.Rows[Math.Min(selectedIndex, DGVValues.RowCount - 1)].Selected = true;
                if (firstRowIndex >= 0 && DGVValues.RowCount > 0)
                    DGVValues.FirstDisplayedScrollingRowIndex = Math.Min(firstRowIndex, DGVValues.RowCount - 1);

                DGVValues.ResumeLayout();

                AutoSizeSmart();

                var chunkType = chunk.GetType();
                if (_pluginChunkEditors.TryGetValue(chunkType, out var editors))
                    foreach (var editor in editors)
                        if (!_editors.Any(x => x.Name == editor.Name))
                            _editors.Add(new(editor.Name, editor.Editor));

                CBEditor.SelectedIndex = Settings.GetLastEditor(_editors, chunkType);
                if (CBEditor.SelectedIndex == 1)
                    HBHex.Invalidate();

                foreach (var editorControl in PnlEditors.Controls.OfType<EditorControl>().Where(x => x.Visible))
                    editorControl.LoadChunk(chunk);
            }

            UpdateChunkColours();

            TVChunks.EndUpdate();
        }
        void OnChildAdded(Chunk newChild) => InsertChunkNode(node, newChild);
        void OnChildRemoved(Chunk removedChild, int oldIndex) => RemoveChunkNode(node, removedChild, oldIndex);
        void OnChildrenAdded(IReadOnlyList<Chunk> children)
        {
            TVChunks.BeginUpdate();
            foreach (var child in children)
                InsertChunkNode(node, child, false);
            UpdateChunkIndices(node, children[0].IndexInParent);
            TVChunks.EndUpdate();
        }
        void OnChildrenRemoved(IReadOnlyList<(Chunk chunk, int oldIndex)> children)
        {
            TVChunks.BeginUpdate();
            var firstIndex = 0;
            foreach (var (removedChild, oldIndex) in children.OrderByDescending(x => x.oldIndex))
            {
                RemoveChunkNode(node, removedChild, oldIndex, false);
                firstIndex = oldIndex;
            }
            UpdateChunkIndices(node, firstIndex);
            TVChunks.EndUpdate();
        }
        void OnChildrenCleared(IReadOnlyList<(Chunk chunk, int oldIndex)> children)
        {
            TVChunks.BeginUpdate();
            foreach (TreeNode node in node.Nodes)
                UnsubscribeNode(node);
            node.Nodes.Clear();
            TVChunks.EndUpdate();
        }

        chunk.PropertyChanged += OnPropertyChanged;
        chunk.ChildAdded += OnChildAdded;
        chunk.ChildRemoved += OnChildRemoved;
        chunk.ChildrenAdded += OnChildrenAdded;
        chunk.ChildrenRemoved += OnChildrenRemoved;
        chunk.ChildrenCleared += OnChildrenCleared;

        _nodeCleanups[node] = () =>
        {
            chunk.PropertyChanged -= OnPropertyChanged;
            chunk.ChildAdded -= OnChildAdded;
            chunk.ChildRemoved -= OnChildRemoved;
            chunk.ChildrenAdded -= OnChildrenAdded;
            chunk.ChildrenRemoved -= OnChildrenRemoved;
            chunk.ChildrenCleared -= OnChildrenCleared;
        };
    }

    private void UnsubscribeNode(TreeNode node)
    {
        if (_nodeCleanups.Remove(node, out var cleanup))
            cleanup();

        foreach (TreeNode childNode in node.Nodes)
            UnsubscribeNode(childNode);
    }

    private void UpdateChunkIndices(TreeNode parentNode, int startIndex)
    {
        TVChunks.BeginUpdate();
        for (var i = startIndex; i < parentNode.Nodes.Count; i++)
        {
            var node = parentNode.Nodes[i];
            if (node.Tag is Chunk chunk)
            {
                var text = $"{i}. {chunk}";
                if (node.Text != text)
                    node.Text = text;
            }
        }
        TVChunks.EndUpdate();
    }

    private List<int>? GetChunkHierarchy(Chunk chunk)
    {
        if (chunk == null)
            return null;

        if (chunk.ParentFile == null && chunk.ParentChunk == null)
            return null;

        var indices = new List<int>();
        var current = chunk;
        while (current != null && current.ParentChunk != null)
        {
            indices.Add(current.IndexInParent);
            current = current.ParentChunk;
        }

        if (current?.ParentFile == null)
            return null;
        indices.Add(current.IndexInParent);

        return indices;
    }

    private async Task<TreeNode?> GetTreeNodeFromChunk(Chunk chunk, int timeoutMs = 5000)
    {
        var indices = GetChunkHierarchy(chunk);
        if (indices == null)
            return null;

        var node = TVChunks.Nodes[0];
        for (var i = indices.Count - 1; i >= 0; i--)
        {
            var idx = indices[i];

            var startTime = DateTime.UtcNow;

            node.Expand();
            while (node.Nodes.Count <= idx)
            {
                if ((DateTime.UtcNow - startTime).TotalMilliseconds > timeoutMs)
                    return null;

                await Task.Delay(50);
            }

            node = node.Nodes[idx];
        }

        return node;
    }

    private void SC1_SplitterMoved(object sender, SplitterEventArgs e) => Settings.SplitterDistance = SC1.SplitterDistance;

    public readonly struct Editor
    {
        public string Name { get; }
        public Control Control { get; }
        public IChunkEditor? ChunkEditor { get; }

        public Editor(string name, Control control, IChunkEditor? chunkEditor = null)
        {
            Name = name;
            Control = control;
            ChunkEditor = chunkEditor;
        }

        public override readonly string ToString() => Name;
    }
}
