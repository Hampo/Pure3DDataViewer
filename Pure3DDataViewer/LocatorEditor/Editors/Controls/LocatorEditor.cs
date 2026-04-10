using LocatorEditor.Editors.Controls.TypeData;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Chunks;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.UndoRedo;
using Pure3DDataViewerPluginAPI.UndoRedo.Commands;
using Pure3DDataViewerPluginAPI.Utils;
using System.Numerics;

namespace LocatorEditor.Editors.Controls;

public partial class LocatorEditor : EditorControl
{
    private LocatorChunk? _locatorChunk;
    private bool _updating = false;

    public LocatorEditor()
    {
        InitializeComponent();
    }

    public override void LoadChunk(Chunk chunk)
    {
        try
        {
            _updating = true;

            if (chunk is not LocatorChunk locatorChunk)
                throw new NotSupportedException($"{nameof(LocatorEditor)} does not support chunks of type {chunk.GetType()}");
            _locatorChunk = locatorChunk;

            NTBPositionX.Value = locatorChunk.Position.X;
            NTBPositionY.Value = locatorChunk.Position.Y;
            NTBPositionZ.Value = locatorChunk.Position.Z;

            GBTypeDataEditor.Controls.Clear();
            switch (locatorChunk.TypeData)
            {
                case LocatorChunk.EventLocatorData:
                    GBTypeDataEditor.Controls.Add(new EventEditor(locatorChunk)
                    {
                        Dock = DockStyle.Fill,
                    });
                    break;
                case LocatorChunk.ScriptLocatorData:
                    GBTypeDataEditor.Controls.Add(new ScriptEditor(locatorChunk)
                    {
                        Dock = DockStyle.Fill,
                    });
                    break;
                case LocatorChunk.GenericLocatorData:
                    GBTypeDataEditor.Controls.Add(new Label()
                    {
                        Text = "Generic type locators have no data",
                        Dock = DockStyle.Fill,
                    });
                    break;
                default:
                    GBTypeDataEditor.Controls.Add(new Label()
                    {
                        Text = "Unsupported type data",
                        Dock = DockStyle.Fill,
                    });
                    break;
            }
        }
        finally
        {
            _updating = false;
        }
    }

    private bool LocationFromGame()
    {
        if (_locatorChunk == null)
            return false;

        var pos = MemoryUtils.GetPosition();

        if (!pos.HasValue)
        {
            MessageBox.Show("Failed to retrieve position from game.\nEnsure the game is open and you're in gameplay.", Name, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        var (position, rotation) = pos.Value;

        _locatorChunk.Position = position;
        if (_locatorChunk.TypeData is LocatorChunk.CarStartLocatorData carStartLocatorData)
            carStartLocatorData.Rotation = (float)rotation;

        _updating = true;
        try
        {
            NTBPositionX.Value = position.X;
            NTBPositionY.Value = position.Y;
            NTBPositionZ.Value = position.Z;

            // TODO: Rotation value
        }
        finally
        {
            _updating = false;
        }

        return true;
    }

    private void BtnFromGameIncludingTriggers_Click(object sender, EventArgs e)
    {
        if (_locatorChunk == null)
            return;

        var beforeChunk = _locatorChunk.Clone();
        if (!LocationFromGame())
            return;

        var position = _locatorChunk.Position;
        foreach (var triggerVolume in _locatorChunk.GetChunksOfType<TriggerVolumeChunk>())
        {
            var matrix = triggerVolume.Matrix;
            matrix.M41 = position.X;
            matrix.M42 = position.Y;
            matrix.M43 = position.Z;
            triggerVolume.Matrix = matrix;
        }

        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Position (w/ Triggers)", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }

    private void BtnFromGameExcludingTriggers_Click(object sender, EventArgs e)
    {
        if (_locatorChunk == null)
            return;

        var beforeChunk = _locatorChunk.Clone();
        if (!LocationFromGame())
            return;

        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Position (w/o Triggers)", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }

    private void BtnTeleportInGame_Click(object sender, EventArgs e)
    {
        if (_locatorChunk == null)
            return;

        var rot = 0f;
        if (_locatorChunk.TypeData is LocatorChunk.CarStartLocatorData carStartLocatorData)
            rot = carStartLocatorData.Rotation;

        try
        {
            MemoryUtils.Teleport(_locatorChunk.Position, rot);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Could not teleport: {ex.Message}", "Error teleporting", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void NTBPosition_Leave(object sender, EventArgs e)
    {
        if (_updating || _locatorChunk == null)
            return;

        var positionX = (float)NTBPositionX.Value!;
        var positionY = (float)NTBPositionY.Value!;
        var positionZ = (float)NTBPositionZ.Value!;

        if (MathUtils.NearlyEqual(positionX, _locatorChunk.Position.X) && MathUtils.NearlyEqual(positionY, _locatorChunk.Position.Y) && MathUtils.NearlyEqual(positionZ, _locatorChunk.Position.Z))
        {
            NTBPositionX.Value = _locatorChunk.Position.X;
            NTBPositionY.Value = _locatorChunk.Position.Y;
            NTBPositionZ.Value = _locatorChunk.Position.Z;
            return;
        }

        var beforeChunk = _locatorChunk.Clone();
        _locatorChunk.Position = new Vector3(positionX, positionY, positionZ);
        UndoRedoManager.Instance.Execute(new UpdateChunkCommand("Update Locator Position", _locatorChunk.GetChunkHierarchy()!, beforeChunk, _locatorChunk));
    }
}
