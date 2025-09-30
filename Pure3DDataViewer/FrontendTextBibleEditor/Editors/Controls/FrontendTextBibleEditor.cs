using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Editors;

namespace FrontendTextBibleEditor.Editors.Controls;
public partial class FrontendTextBibleEditor : EditorControl//<FrontendTextBibleChunk>
{
    private FrontendTextBibleChunk? _frontendTextBibleChunk;

    private IReadOnlyList<FrontendLanguageChunk>? _languageChunks;

    private Dictionary<uint, Dictionary<uint, string>>? _knownNameMap;

    private HashSet<Entry>? _entries;
    private Dictionary<uint, Dictionary<string, string>>? _values;

    public FrontendTextBibleEditor()
    {
        InitializeComponent();
    }

    public override void LoadChunk(Chunk chunk)
    {
        if (chunk is not FrontendTextBibleChunk frontendTextBibleChunk)
            throw new NotSupportedException($"{nameof(FrontendTextBibleEditor)} does not support chunks of type {chunk.GetType()}");
        _frontendTextBibleChunk = frontendTextBibleChunk;
        _languageChunks = chunk.GetChunksOfType<FrontendLanguageChunk>();
        _knownNameMap = [];
        _entries = [];
        _values = [];
        BtnUpdate.Enabled = false;

        LVValues.BeginUpdate();
        LVValues.Items.Clear();

        foreach (var languageChunk in _languageChunks)
        {
            if (!_knownNameMap.TryGetValue(languageChunk.Modulo, out var map))
            {
                map = [];
                foreach (var name in KnownNames.Names)
                    map[languageChunk.GetNameHash(name)] = name;
            }

            foreach (var entry in languageChunk.Entries)
            {
                if (!map.TryGetValue(entry.Hash, out var displayName))
                    displayName = $"0x{entry.Hash:X}";
                _entries.Add(new(entry.Hash, displayName));


                if (!_values.TryGetValue(entry.Hash, out var values))
                {
                    values = [];
                    _values[entry.Hash] = values;
                }
                values[languageChunk.Name] = entry.Value;
            }

            LVValues.Items.Add(languageChunk.Name).SubItems.Add(string.Empty);
        }

        LVValues.EndUpdate();

        CBEntry.DataSource = _entries.ToList();
        CBEntry.DisplayMember = nameof(Entry.DisplayName);
        CBEntry.ValueMember = nameof(Entry.Hash);
    }

    private void BtnUpdate_Click(object sender, EventArgs e)
    {
        if (_frontendTextBibleChunk == null)
            return;
        if (_languageChunks == null)
            return;
        if (_values == null)
            return;

        foreach (var languageChunk in _languageChunks)
        {
            foreach (var value in _values)
            {
                if (!value.Value.TryGetValue(languageChunk.Name, out var valueStr))
                    valueStr = string.Empty;
                languageChunk.SetValue(value.Key, valueStr);
            }
        }

        OnUpdatedChunk(_frontendTextBibleChunk);
        BtnUpdate.Enabled = false;
    }

    private void CBEntry_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_values == null)
            return;
        LVValues.BeginUpdate();
        try
        {
            if (CBEntry.SelectedItem is not Entry entry)
            {
                foreach (ListViewItem lvi in LVValues.Items)
                    lvi.SubItems[1].Text = string.Empty;
                return;
            }

            for (int i = 0; i < LVValues.Items.Count; i++)
            {
                var lvi = LVValues.Items[i];
                if (!_values[entry.Hash].TryGetValue(lvi.Text, out var value))
                    value = string.Empty;
                lvi.SubItems[1].Text = value;
            }
        }
        finally
        {
            foreach (ColumnHeader column in LVValues.Columns)
                column.Width = -2;
            LVValues.EndUpdate();
        }
    }

    private void LVValues_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (_values == null)
            return;
        if (CBEntry.SelectedItem is not Entry entry)
            return;
        
        if (sender is not ListView lv)
            return;

        if (e.Button != MouseButtons.Left)
            return;

        var lvi = lv.GetItemAt(e.X, e.Y);
        if (lvi == null)
            return;

        using var stringEditor = new FrmStringEditor($"{entry.DisplayName} ({lvi.Text})", lvi.SubItems[1].Text, int.MaxValue);
        if (stringEditor.ShowDialog() != DialogResult.OK)
            return;

        lvi.SubItems[1].Text = stringEditor.Value;
        _values[entry.Hash][lvi.Text] = stringEditor.Value;
        BtnUpdate.Enabled = true;
    }

    private readonly struct Entry
    {
        public uint Hash { get; }
        public string DisplayName { get; }

        public Entry(uint hash, string displayName)
        {
            Hash = hash;
            DisplayName = displayName;
        }
    }
}
