using Be.Windows.Forms;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Editors;
using Pure3DDataViewerPluginAPI.Events;
using Pure3DDataViewerPluginAPI.Extensions;
using Pure3DDataViewerPluginAPI.Interfaces;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Pure3DDataViewer;

public partial class FrmMain : Form
{
    private P3DFile P3DFile = new();
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

    private readonly Dictionary<Type, List<IChunkEditor>> _pluginChunkEditors = [];

    private void UpdateText()
    {
        StringBuilder text = new();
        if (_unsavedChanges)
            text.Append('*');
        if (!string.IsNullOrEmpty(_lastPath))
            text.Append($"{_lastPath} - ");
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

                    var tsmi = new ToolStripMenuItem(fileHandler.Name)
                    {
                        Image = fileHandler.Image,
                        Tag = fileHandler
                    };
                    tsmi.Click += TSMIPlugin_Click;
                    TSMITools.DropDownItems.Add(tsmi);

                    tsmi = new ToolStripMenuItem(fileHandler.Name)
                    {
                        Image = fileHandler.Image,
                        Tag = fileHandler
                    };
                    tsmi.Click += TSMIPlugin_Click;
                    CMSTVChunks.Items.Add(tsmi);
                }
            }
            TSMITools.DropDownItems.Add(new ToolStripSeparator());

            foreach (var plugin in PluginLoader.Plugins)
            {
                var chunkHandlers = plugin.GetChunkHandlers();
                if (chunkHandlers == null)
                    continue;

                foreach (var chunkHandler in chunkHandlers)
                {
                    if (chunkHandler == null)
                        continue;

                    var tsmi = new ToolStripMenuItem(chunkHandler.Name)
                    {
                        Image = chunkHandler.Image,
                        Tag = chunkHandler
                    };
                    tsmi.Click += TSMIPlugin_Click;
                    TSMITools.DropDownItems.Add(tsmi);

                    tsmi = new ToolStripMenuItem(chunkHandler.Name)
                    {
                        Image = chunkHandler.Image,
                        Tag = chunkHandler
                    };
                    tsmi.Click += TSMIPlugin_Click;
                    CMSTVChunks.Items.Add(tsmi);
                }
            }

            foreach (var plugin in PluginLoader.Plugins)
            {
                var chunkEditors = plugin.GetChunkEditors();
                if (chunkEditors == null)
                    continue;

                foreach (var chunkEditor in chunkEditors)
                {
                    var type = chunkEditor.ChunkType;

                    chunkEditor.UpdatedChunk += IChunkEditor_UpdatedChunk;

                    if (!_pluginChunkEditors.TryGetValue(type, out var editors))
                    {
                        _pluginChunkEditors[type] = [chunkEditor];
                        continue;
                    }
                    editors.Add(chunkEditor);
                }
            }
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

        var parent = node.Parent!;
        var index = node.Index;
        var chunk = e.Chunk;

        parent.Nodes.RemoveAt(index);
        TVChunks.SelectedNode = AddChunk(parent, chunk, index);

        UnsavedChanges = true;

        node.Text = $"{node.Index}. {chunk}";

        UnsavedChanges = true;

        TVChunks_AfterSelect(TVChunks, new(node));
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
        switch (handler)
        {
            case IFileHandler fileHandler:
                try
                {
                    switch (fileHandler.Handle(P3DFile))
                    {
                        case Pure3DDataViewerPluginAPI.Enums.FileCallbackResult.Modified:
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
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.Modified:
                            var parent = node.Parent!;
                            var index = node.Index;

                            parent.Nodes.RemoveAt(index);
                            TVChunks.SelectedNode = AddChunk(parent, chunk, index);

                            UnsavedChanges = true;
                            break;
                        case Pure3DDataViewerPluginAPI.Enums.ChunkCallbackResult.Deleted:
                            var parentNode = node.Parent;

                            if (parentNode.Tag is Chunk parentChunk)
                                parentChunk.Children.RemoveAt(node.Index);
                            else if (parentNode.Tag is P3DFile parentFile)
                                parentFile.Chunks.RemoveAt(node.Index);
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

    private void FrmMain_KeyDown(object sender, KeyEventArgs e)
    {
        if ((e.Modifiers & Keys.Control) == 0)
            return;
        switch (e.KeyCode)
        {
            case Keys.N:
                e.SuppressKeyPress = true;
                e.Handled = true;
                TSMINew.PerformClick();
                break;
            case Keys.O:
                e.SuppressKeyPress = true;
                e.Handled = true;
                TSMIOpen.PerformClick();
                break;
            case Keys.S:
                e.SuppressKeyPress = true;
                e.Handled = true;
                if ((e.Modifiers & Keys.Shift) == 0)
                    TSMISave.PerformClick();
                else
                    TSMISaveAs.PerformClick();
                break;
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
        P3DFile = new P3DFile();
        LastPath = string.Empty;
        PopulateData();
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

    private void TSMISave_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(LastPath))
        {
            try
            {
                P3DFile.Write(LastPath);
                UnsavedChanges = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error writing P3D file: {ex}", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return;
        }

        using var sfd = new SaveFileDialog() { Title = "Save File", Filter = "P3D files|*.p3d|All files|*.*" };
        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            P3DFile.Write(sfd.FileName);
            UnsavedChanges = false;
            LastPath = sfd.FileName;
            Settings.AddRecentFile(LastPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error writing P3D file: {ex}", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void TSMISaveAs_Click(object sender, EventArgs e)
    {

        using var sfd = new SaveFileDialog() { Title = "Save File", Filter = "P3D files|*.p3d|All files|*.*" };
        if (!string.IsNullOrEmpty(LastPath))
        {
            sfd.InitialDirectory = Path.GetDirectoryName(LastPath);
            sfd.FileName = Path.GetFileName(LastPath);
            if (!Path.GetExtension(LastPath).Equals(".p3d", StringComparison.OrdinalIgnoreCase))
                sfd.FilterIndex = 2;
        }
        if (sfd.ShowDialog() != DialogResult.OK)
            return;

        try
        {
            P3DFile.Write(sfd.FileName);
            UnsavedChanges = false;
            LastPath = sfd.FileName;
            Settings.AddRecentFile(LastPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error writing save file: {ex}", "Error saving file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
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
            P3DFile = p3dFile;
            LastPath = filePath;
            PopulateData();
            UnsavedChanges = false;
            Settings.AddRecentFile(LastPath);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading P3D file: {ex}", "Error opening file", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void PopulateData()
    {
        TVChunks.BeginUpdate();
        TVChunks.Nodes.Clear();

        var rootNode = TVChunks.Nodes.Add(string.IsNullOrWhiteSpace(LastPath) ? "Untitled" : LastPath);
        foreach (var chunk in P3DFile.Chunks)
            AddChunk(rootNode, chunk);
        rootNode.Tag = P3DFile;
        rootNode.Expand();
        TVChunks.SelectedNode = rootNode;

        TVChunks.EndUpdate();
    }

    private static TreeNode AddChunk(TreeNode parentNode, Chunk chunk, int index = -1)
    {
        TreeNode chunkNode;
        if (index < 0)
        {
            chunkNode = parentNode.Nodes.Add($"{parentNode.Nodes.Count}. {chunk}");
        }
        else
        {
            parentNode.TreeView.BeginUpdate();
            chunkNode = parentNode.Nodes.Insert(index, $"{index}. {chunk}");
            for (int i = 0; i < parentNode.Nodes.Count; i++)
            {
                var node = parentNode.Nodes[i];
                if (node.Tag is Chunk nodeChunk)
                    node.Text = $"{node.Index}. {nodeChunk}";
            }
            parentNode.TreeView.EndUpdate();
        }
        chunkNode.Tag = chunk;
        if (chunk is UnknownChunk)
        {
            chunkNode.BackColor = Color.LightGoldenrodYellow;

#if DEBUG
            parentNode.Expand();
            var parent = parentNode.Parent;
            while (parent != null)
            {
                parent.Expand();
                parent = parent.Parent;
            }
#endif
        }

        foreach (var child in chunk.Children)
            AddChunk(chunkNode, child);

        return chunkNode;
    }

    private static readonly HashSet<string> ExcludedProperties = ["DataBytes", "DataLength", "ID", "Children", "HeaderSize", "Size", "Bytes"];
    private readonly Dictionary<Type, (int Index, string Name)> LastEditorTab = [];
    private bool _afterSelectUpdating = false;
    private void TVChunks_AfterSelect(object sender, TreeViewEventArgs e)
    {
        _afterSelectUpdating = true;
        LVValues.BeginUpdate();
        LVValues.Items.Clear();

        TCEditors.SuspendLayout();

        for (int i = TCEditors.TabCount - 1; i >= 2; i--)
        {
            var tp = TCEditors.TabPages[i];
            TCEditors.TabPages.RemoveAt(i);
            tp.Dispose();
        }

        var tag = e.Node?.Tag;

        try
        {
            TSMICut1.Enabled = false;
            TSMICopyThis1.Enabled = false;
            TSMICopyType1.Enabled = false;
            TSMIPasteBefore1.Enabled = false;
            TSMIPasteAfter1.Enabled = false;

            TSMICut2.Enabled = false;
            TSMICopyThis2.Enabled = false;
            TSMICopyType2.Enabled = false;
            TSMIPasteBefore2.Enabled = false;
            TSMIPasteAfter2.Enabled = false;

            foreach (var child in TSMITools.DropDownItems.OfType<ToolStripMenuItem>())
                if (child.Tag is IChunkHandler)
                    child.Visible = false;

            foreach (var child in CMSTVChunks.Items.OfType<ToolStripMenuItem>())
                if (child.Tag is IChunkHandler || child.Tag is IFileHandler)
                    child.Visible = false;

            if (tag is P3DFile p3dFile)
            {
                var lvi = new ListViewItem("Size");
                lvi.SubItems.Add($"{p3dFile.Size:N0} bytes");
                LVValues.Items.Add(lvi);
                HBHex.ByteProvider = new DynamicByteProvider(p3dFile.Bytes);

                foreach (var child in CMSTVChunks.Items.OfType<ToolStripMenuItem>())
                    if (child.Tag is IFileHandler)
                        child.Visible = true;

                return;
            }

            if (tag is Chunk chunk)
            {
                TSMICut1.Enabled = true;
                TSMICopyThis1.Enabled = true;
                TSMICopyType1.Enabled = true;
                TSMIPasteBefore1.Enabled = true;
                TSMIPasteAfter1.Enabled = true;

                TSMICut2.Enabled = true;
                TSMICopyThis2.Enabled = true;
                TSMICopyType2.Enabled = true;
                TSMIPasteBefore2.Enabled = true;
                TSMIPasteAfter2.Enabled = true;

                foreach (var child in TSMITools.DropDownItems.OfType<ToolStripMenuItem>())
                    if (child.Tag is IChunkHandler chunkHandler)
                        child.Visible = chunkHandler.ChunkType == null || chunkHandler.ChunkType == chunk.GetType();
                foreach (var child in CMSTVChunks.Items.OfType<ToolStripMenuItem>())
                    if (child.Tag is IChunkHandler chunkHandler)
                        child.Visible = chunkHandler.ChunkType == null || chunkHandler.ChunkType == chunk.GetType();

                if (_pluginChunkEditors.TryGetValue(chunk.GetType(), out var editors))
                {
                    foreach (var editor in editors)
                    {
                        var editorControl = editor.GetEditor(chunk);
                        editorControl.Dock = DockStyle.Fill;

                        var tp = new TabPage(editor.Name)
                        {
                            Tag = editor
                        };
                        tp.Controls.Add(editorControl);
                        TCEditors.TabPages.Add(tp);
                    }
                }

                var type = chunk.GetType();
                var properties = type.GetProperties().OrderBy(x => x.DeclaringType == type);

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
                        LVValues.Items.Add(lvi);
                    }
                    else if (property.IsEnumerable() && value is IEnumerable enumerable)
                    {
                        List<object> values = [.. enumerable.Cast<object>()];
                        for (int i = 0; i < values.Count; i++)
                        {
                            var lvi = new ListViewItem($"{property.Name}[{i}]");
                            lvi.SubItems.Add(values[i]?.ToString() ?? "<NULL>");
                            lvi.Tag = (property, i);
                            LVValues.Items.Add(lvi);
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
                        LVValues.Items.Add(lvi);
                    }
                }
                HBHex.ByteProvider = new DynamicByteProvider(chunk.DataBytes);

                return;
            }
        }
        finally
        {
            foreach (ColumnHeader column in LVValues.Columns)
                column.Width = -2;
            if (LVValues.Items.Count > 0)
                LVValues.Items[0].Selected = true;
            LVValues.EndUpdate();

            if (tag != null && LastEditorTab.TryGetValue(tag.GetType(), out var lastEditor) && TCEditors.TabCount > lastEditor.Index && TCEditors.TabPages[lastEditor.Index].Text == lastEditor.Name)
                TCEditors.SelectedIndex = lastEditor.Index;

            TCEditors.ResumeLayout();
            _afterSelectUpdating = false;
        }
    }

    private void TCEditors_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_afterSelectUpdating)
            return;

        var tag = TVChunks.SelectedNode?.Tag;
        if (tag == null)
            return;

        LastEditorTab[tag.GetType()] = (TCEditors.SelectedIndex, TCEditors.SelectedTab!.Text);
    }

    private void LVValues_Resize(object sender, EventArgs e)
    {
        LVValues.BeginUpdate();
        var selectedIndices = LVValues.SelectedIndices;
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

        var lviIndex = lvi.Index;
        var Updated = false;
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
            TVChunks.SelectedNode.Text = $"{TVChunks.SelectedNode.Index}. {chunk}";

            UnsavedChanges = true;

            TVChunks_AfterSelect(sender, new(TVChunks.SelectedNode));
            if (lv.Items.Count > lviIndex)
            {
                lv.Items[lviIndex].Selected = true;
                lv.EnsureVisible(lviIndex);
            }
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

    private void TVChunks_KeyDown(object sender, KeyEventArgs e)
    {
        var node = TVChunks.SelectedNode;
        if (node == null)
            return;

        switch (e.KeyCode)
        {
            case Keys.Delete:
                if (node.Tag is not Chunk chunk)
                    return;

                if (!e.Shift && MessageBox.Show("Are you sure you want to delete the selected chunk?", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                    return;

                var parentNode = node.Parent;

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
                TVChunks.EndUpdate();

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
            MessageBox.Show("Reached end of file", "No match found", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        var properties = type.GetProperties().OrderBy(x => x.DeclaringType == type);
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
                var classProperties = value.GetType().GetProperties();
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

        List<Chunk> parentChunks;
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

        var index = node.Index;
        UnsavedChanges = true;
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

        List<Chunk> parentChunks;
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

        List<Chunk> parentChunks;
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
            var newChunk = frmNewChunk.Chunk;
            if (newChunk == null)
                return;

            UnsavedChanges = true;

            List<Chunk> parentChunks;
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

            parentChunks.Add(newChunk);
            var chunkNode = AddChunk(node, newChunk);
            chunkNode.EnsureVisible();
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

    private void TSMITools_DropDownOpening(object sender, EventArgs e) => HandlePluginSettings(TSMITools.DropDownItems);

    private void CMSTVChunks_Opening(object sender, System.ComponentModel.CancelEventArgs e) => HandlePluginSettings(CMSTVChunks.Items);

    private void HandlePluginSettings(ToolStripItemCollection items)
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
}
