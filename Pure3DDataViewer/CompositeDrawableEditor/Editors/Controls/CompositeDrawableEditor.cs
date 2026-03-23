using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.UndoRedo;
using Pure3DDataViewerPluginAPI.UndoRedo.Commands;

namespace CompositeDrawableEditor.Editors.Controls;

public partial class CompositeDrawableEditor : EditorControl
{
    private CompositeDrawableChunk? _compositeDrawableChunk;
    private bool _updating = false;

    public CompositeDrawableEditor()
    {
        InitializeComponent();
    }

    public override void LoadChunk(Chunk chunk)
    {
        DGVSkinList.SuspendLayout();
        DGVPropList.SuspendLayout();
        DGVEffectList.SuspendLayout();
        CBSkeletonName.BeginUpdate();

        var currentSkeletonName = CBSkeletonName.Text;
        var selectionStart = CBSkeletonName.SelectionStart;

        try
        {
            _updating = true;
            DGVSkinList.Rows.Clear();
            DGVPropList.Rows.Clear();
            DGVEffectList.Rows.Clear();
            CBSkeletonName.Items.Clear();

            if (chunk is not CompositeDrawableChunk compositeDrawableChunk)
                throw new NotSupportedException($"{nameof(CompositeDrawableEditor)} does not support chunks of type {chunk.GetType()}");
            _compositeDrawableChunk = compositeDrawableChunk;

            TxtName.Text = compositeDrawableChunk.Name;
            CBSkeletonName.Text = compositeDrawableChunk.SkeletonName;

            var skinList = compositeDrawableChunk.GetLastChunkOfType<CompositeDrawableSkinListChunk>();
            var propList = compositeDrawableChunk.GetLastChunkOfType<CompositeDrawablePropListChunk>();
            var effectList = compositeDrawableChunk.GetLastChunkOfType<CompositeDrawableEffectListChunk>();

            Chunk? current = compositeDrawableChunk;
            while (current.ParentChunk != null)
            {
                LoadChildren(skinList, propList, effectList, current.ParentChunk.Children);
                current = current.ParentChunk;
            }
            if (current.ParentFile != null)
                LoadChildren(skinList, propList, effectList, current.ParentFile.Chunks);

            if (skinList != null)
                foreach (var skin in skinList.GetChunksOfType<CompositeDrawableSkinChunk>())
                    AddSkin(skinList, skin.Name);

            if (propList != null)
                foreach (var prop in propList.GetChunksOfType<CompositeDrawablePropChunk>())
                    AddProp(propList, prop.Name);

            if (effectList != null)
                foreach (var effect in effectList.GetChunksOfType<CompositeDrawableEffectChunk>())
                    AddEffect(effectList, effect.Name);
        }
        finally
        {
            DGVSkinList.ResumeLayout();
            DGVPropList.ResumeLayout();
            DGVEffectList.ResumeLayout();
            CBSkeletonName.EndUpdate();
            _updating = false;
        }

        CBSkeletonName.SelectionStart = CBSkeletonName.Text == currentSkeletonName ? selectionStart : CBSkeletonName.Text.Length;
    }

    private static bool RowExists(DataGridView dataGridView, object targetValue)
    {
        foreach (DataGridViewRow row in dataGridView.Rows)
        {
            if (row.IsNewRow)
                continue;

            if (Equals(row.Cells[0].Value, targetValue))
                return true;
        }
        return false;
    }

    private void LoadChildren(CompositeDrawableSkinListChunk? skinList, CompositeDrawablePropListChunk? propList, CompositeDrawableEffectListChunk? effectList, IList<Chunk> chunks)
    {
        foreach (var child in chunks)
        {
            switch (child)
            {
                case SkinChunk skin:
                    AddSkin(skinList, skin.Name);
                    break;
                case AnimatedObjectChunk animatedObject:
                    AddProp(propList, animatedObject.Name);
                    break;
                case CompositeDrawableChunk compositeDrawable:
                    if (compositeDrawable.Name == _compositeDrawableChunk!.Name)
                        break;

                    AddProp(propList, compositeDrawable.Name);
                    break;
                case OldBillboardQuadGroupChunk oldBillboardQuadGroup:
                    AddProp(propList, oldBillboardQuadGroup.Name);
                    break;
                case MeshChunk mesh:
                    AddProp(propList, mesh.Name);
                    break;
                case ScenegraphChunk scenegraph:
                    AddProp(propList, scenegraph.Name);
                    break;
                case StaticEntityChunk staticEntity:
                    AddProp(propList, staticEntity.Name);
                    break;
                case ParticleSystemChunk particleSystem:
                    AddEffect(effectList, particleSystem.Name);
                    break;
                case SkeletonChunk skeleton:
                    if (!CBSkeletonName.Items.Contains(skeleton.Name))
                        CBSkeletonName.Items.Add(skeleton.Name);
                    break;
            }
        }
    }

    private void AddSkin(CompositeDrawableSkinListChunk? skinList, string name)
    {
        if (RowExists(DGVSkinList, name))
            return;

        var skinChunk = skinList?.GetFirstChunkOfType<CompositeDrawableSkinChunk>(name);
        var sortOrder = skinChunk?.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
        DGVSkinList.Rows.Add(name, skinChunk != null, skinChunk?.IsTranslucent ?? false, sortOrder?.SortOrder ?? 0.5f);
    }

    private void AddProp(CompositeDrawablePropListChunk? propList, string name)
    {
        if (RowExists(DGVPropList, name))
            return;

        var propChunk = propList?.GetFirstChunkOfType<CompositeDrawablePropChunk>(name);
        var sortOrder = propChunk?.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
        DGVPropList.Rows.Add(name, propChunk != null, propChunk?.IsTranslucent ?? false, propChunk?.SkeletonJointId ?? 0, sortOrder?.SortOrder ?? 0.5f);
    }

    private void AddEffect(CompositeDrawableEffectListChunk? effectList, string name)
    {
        if (RowExists(DGVEffectList, name))
            return;

        var effectChunk = effectList?.GetFirstChunkOfType<CompositeDrawableEffectChunk>(name);
        var sortOrder = effectChunk?.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
        DGVEffectList.Rows.Add(name, effectChunk != null, effectChunk?.IsTranslucent ?? false, effectChunk?.SkeletonJointId ?? 0, sortOrder?.SortOrder ?? 0.5f);
    }

    private void DGV_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
    {
        if (sender is not DataGridView dgv)
            return;

        switch (dgv.Columns[e.ColumnIndex].HeaderText)
        {
            case "Skeleton Joint Index":
                if (!int.TryParse(e.FormattedValue?.ToString(), out _))
                {
                    e.Cancel = true;
                    dgv.Rows[e.RowIndex].ErrorText = "Skeleton Joint Index must be an integer";
                }
                break;
            case "Sort Order":
                if (!float.TryParse(e.FormattedValue?.ToString(), out _))
                {
                    e.Cancel = true;
                    dgv.Rows[e.RowIndex].ErrorText = "Sort Order must be a float";
                }
                break;
        }
    }

    private void DGV_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {
        if (sender is not DataGridView dgv)
            return;

        switch (dgv.CurrentCell.OwningColumn.HeaderText)
        {
            case "Skeleton Joint Index":
                {
                    if (e.Control is not TextBox tb)
                        return;

                    tb.KeyPress -= OnlyDigits;
                    tb.KeyPress += OnlyDigits;
                }
                break;
            case "Sort Order":
                {
                    if (e.Control is not TextBox tb)
                        return;

                    tb.KeyPress -= OnlyFloats;
                    tb.KeyPress += OnlyFloats;
                }
                break;
        }
    }

    private void OnlyDigits(object? sender, KeyPressEventArgs e)
    {
        if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            e.Handled = true;
    }

    private void OnlyFloats(object? sender, KeyPressEventArgs e)
    {
        if (sender is not TextBox tb)
            return;

        if (char.IsControl(e.KeyChar))
            return;

        if (char.IsDigit(e.KeyChar))
            return;

        if (e.KeyChar == '.' && !tb.Text.Contains('.'))
            return;

        if (e.KeyChar == '-' && tb.SelectionStart == 0 && !tb.Text.Contains('-'))
            return;

        e.Handled = true;
    }

    private void DGV_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        if (sender is not DataGridView dgv)
            return;

        if (dgv.CurrentCell.ValueType == typeof(bool) && dgv.IsCurrentCellDirty)
            dgv.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DGVSkinList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (_updating || e.RowIndex == -1)
            return;

        var row = DGVSkinList.Rows[e.RowIndex];

        var skinName = (string)row.Cells[0].Value;
        var included = (bool)row.Cells[1].Value;
        var translucent = (bool)row.Cells[2].Value;
        var sortOrder = float.Parse(row.Cells[3].Value.ToString()!);

        var skinList = _compositeDrawableChunk!.GetLastChunkOfType<CompositeDrawableSkinListChunk>();
        var skinChunk = skinList?.GetFirstChunkOfType<CompositeDrawableSkinChunk>(skinName);

        var beforeChunk = _compositeDrawableChunk.Clone();
        switch (e.ColumnIndex)
        {
            case 1: // Included
                if (included)
                {
                    if (skinChunk != null)
                        return;

                    if (skinList == null)
                    {
                        skinList = new();
                        _compositeDrawableChunk.Children.Add(skinList);
                    }

                    var chunk = new CompositeDrawableSkinChunk(skinName, translucent);
                    chunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));
                    skinList.Children.Add(chunk);
                }
                else
                {
                    if (skinChunk != null)
                        skinList!.Children.RemoveAt(skinChunk.IndexInParent);
                }
                break;
            case 2: // Translucent
                if (!included || skinChunk == null)
                    return;

                skinChunk.IsTranslucent = translucent;

                break;
            case 3: // Sort Order
                if (!included || skinChunk == null)
                    return;

                var sortOrderChunk = skinChunk.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
                if (sortOrderChunk != null)
                    sortOrderChunk.SortOrder = sortOrder;
                else
                    skinChunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));

                break;
        }
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Composite Drawable Skin List", _compositeDrawableChunk!.GetChunkHierarchy()!, beforeChunk, _compositeDrawableChunk));
    }

    private void DGVPropList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (_updating || e.RowIndex == -1)
            return;

        var row = DGVPropList.Rows[e.RowIndex];

        var propName = (string)row.Cells[0].Value;
        var included = (bool)row.Cells[1].Value;
        var translucent = (bool)row.Cells[2].Value;
        var skeletonJointIndex = uint.Parse(row.Cells[3].Value.ToString()!);
        var sortOrder = float.Parse(row.Cells[4].Value.ToString()!);

        var propList = _compositeDrawableChunk!.GetLastChunkOfType<CompositeDrawablePropListChunk>();
        var propChunk = propList?.GetFirstChunkOfType<CompositeDrawablePropChunk>(propName);

        var beforeChunk = _compositeDrawableChunk.Clone();
        switch (e.ColumnIndex)
        {
            case 1: // Included
                if (included)
                {
                    if (propChunk != null)
                        return;

                    if (propList == null)
                    {
                        propList = new();
                        _compositeDrawableChunk.Children.Add(propList);
                    }

                    var chunk = new CompositeDrawablePropChunk(propName, translucent, skeletonJointIndex);
                    chunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));
                    propList.Children.Add(chunk);
                }
                else
                {
                    if (propChunk != null)
                        propList!.Children.RemoveAt(propChunk.IndexInParent);
                }
                break;
            case 2: // Translucent
                if (!included || propChunk == null)
                    return;

                propChunk.IsTranslucent = translucent;

                break;
            case 3: // Skeleton Joint Index
                if (!included || propChunk == null)
                    return;

                propChunk.SkeletonJointId = skeletonJointIndex;

                break;
            case 4: // Sort Order
                if (!included || propChunk == null)
                    return;

                var sortOrderChunk = propChunk.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
                if (sortOrderChunk != null)
                    sortOrderChunk.SortOrder = sortOrder;
                else
                    propChunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));

                break;
        }
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Composite Drawable Prop List", _compositeDrawableChunk!.GetChunkHierarchy()!, beforeChunk, _compositeDrawableChunk));
    }

    private void DGVEffectList_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (_updating || e.RowIndex == -1)
            return;

        var row = DGVEffectList.Rows[e.RowIndex];

        var effectName = (string)row.Cells[0].Value;
        var included = (bool)row.Cells[1].Value;
        var translucent = (bool)row.Cells[2].Value;
        var skeletonJointIndex = uint.Parse(row.Cells[3].Value.ToString()!);
        var sortOrder = float.Parse(row.Cells[4].Value.ToString()!);

        var effectList = _compositeDrawableChunk!.GetLastChunkOfType<CompositeDrawableEffectListChunk>();
        var effectChunk = effectList?.GetFirstChunkOfType<CompositeDrawableEffectChunk>(effectName);

        var beforeChunk = _compositeDrawableChunk.Clone();
        switch (e.ColumnIndex)
        {
            case 1: // Included
                if (included)
                {
                    if (effectChunk != null)
                        return;

                    if (effectList == null)
                    {
                        effectList = new();
                        _compositeDrawableChunk.Children.Add(effectList);
                    }

                    var chunk = new CompositeDrawableEffectChunk(effectName, translucent, skeletonJointIndex);
                    chunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));
                    effectList.Children.Add(chunk);
                }
                else
                {
                    if (effectChunk != null)
                        effectList!.Children.RemoveAt(effectChunk.IndexInParent);
                }
                break;
            case 2: // Translucent
                if (!included || effectChunk == null)
                    return;

                effectChunk.IsTranslucent = translucent;

                break;
            case 3: // Skeleton Joint Index
                if (!included || effectChunk == null)
                    return;

                effectChunk.SkeletonJointId = skeletonJointIndex;

                break;
            case 4: // Sort Order
                if (!included || effectChunk == null)
                    return;

                var sortOrderChunk = effectChunk.GetLastChunkOfType<CompositeDrawableSortOrderChunk>();
                if (sortOrderChunk != null)
                    sortOrderChunk.SortOrder = sortOrder;
                else
                    effectChunk.Children.Add(new CompositeDrawableSortOrderChunk(sortOrder));

                break;
        }
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Composite Drawable Effect List", _compositeDrawableChunk!.GetChunkHierarchy()!, beforeChunk, _compositeDrawableChunk));
    }

    private void TxtName_Leave(object sender, EventArgs e)
    {
        if (_updating || _compositeDrawableChunk == null || _compositeDrawableChunk.Name == TxtName.Text)
            return;

        var beforeChunk = _compositeDrawableChunk.Clone();
        _compositeDrawableChunk.Name = TxtName.Text;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Composite Drawable Name", _compositeDrawableChunk!.GetChunkHierarchy()!, beforeChunk, _compositeDrawableChunk));
    }

    private void CBSkeletonName_Leave(object sender, EventArgs e)
    {
        if (_updating || _compositeDrawableChunk == null || _compositeDrawableChunk.SkeletonName == CBSkeletonName.Text)
            return;

        var beforeChunk = _compositeDrawableChunk.Clone();
        _compositeDrawableChunk.SkeletonName = CBSkeletonName.Text;
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Composite Drawable Skeleton Name", _compositeDrawableChunk!.GetChunkHierarchy()!, beforeChunk, _compositeDrawableChunk));
    }
}
