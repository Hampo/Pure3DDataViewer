using NetP3DLib.IO;
using NetP3DLib.P3D;
using NetP3DLib.P3D.Attributes;
using NetP3DLib.P3D.Chunks;
using NetP3DLib.P3D.Enums;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Editors;
using Pure3DDataViewerPluginAPI.Extensions;
using System.ComponentModel;
using System.Reflection;

namespace Pure3DDataViewer;
public partial class FrmNewChunk : Form
{
    private static readonly Dictionary<string, ConstructorInfo> ChunkTypes;
    private static readonly Dictionary<Type, ConstructorInfo> TypeMap;

    static FrmNewChunk()
    {
        ChunkTypes = [];
        TypeMap = [];
        foreach (var chunkType in ChunkLoader.ChunkTypes)
        {
            var type = chunkType.Value.Item1;

            var constructor = type.GetConstructors().FirstOrDefault(constructor =>
            {
                var parameters = constructor.GetParameters();
                return !(parameters.Length >= 1 && parameters[0].ParameterType == typeof(EndianAwareBinaryReader));
            });
            if (constructor == null)
                continue;

            string typeName = Enum.TryParse(chunkType.Key.ToString(), out ChunkIdentifier identifier)
                && Enum.IsDefined(typeof(ChunkIdentifier), identifier)
                    ? identifier.ToString().Replace("_", " ")
                    : $"Unknown 0x{chunkType.Key:X}";
            ChunkTypes[typeName] = constructor;
            TypeMap[type] = constructor;
        }
    }

    private readonly Dictionary<string, int> MaxStringLengths;

    public IList<Chunk>? Chunks
    {
        get
        {
            var selectedConstructor = (ConstructorInfo?)CBChunkType.SelectedValue;
            if (selectedConstructor == null)
                return null;

            var parameters = new List<object>(LVValues.Items.Count);
            foreach (ListViewItem item in LVValues.Items)
            {
                if (item.Tag is ParameterInfo parameter && parameter.ParameterType == typeof(LocatorChunk.LocatorData))
                    parameters.Add(CreateLocatorData());
                else
                    parameters.Add(item.SubItems[1].Tag!);
            }

            var createX = (int)NUDCreateX.Value;
            var chunks = new List<Chunk>(createX);
            for (var i = 0; i < createX; i++)
                chunks.Add((Chunk)selectedConstructor.Invoke([.. parameters]));

            return chunks;
        }
    }

    public FrmNewChunk()
    {
        InitializeComponent();
        MaxStringLengths = [];
        CBLocatorType.DataSource = Enum.GetValues(typeof(LocatorChunk.LocatorTypes));
    }

    private void FrmNewChunk_Load(object sender, EventArgs e)
    {
        CBChunkType.DisplayMember = "Key";
        CBChunkType.ValueMember = "Value";
        CBChunkType.DataSource = new BindingSource(ChunkTypes, null);
        var type = Settings.LastNewChunkType;
        if (type != null && TypeMap.TryGetValue(type, out var constructorInfo))
            CBChunkType.SelectedValue = constructorInfo;
    }

    private void CBChunkType_SelectedValueChanged(object sender, EventArgs e)
    {
        var selectedConstructor = (ConstructorInfo?)CBChunkType.SelectedValue;
        if (selectedConstructor == null)
            return;

        LVValues.BeginUpdate();
        LVValues.Items.Clear();

        var parameters = selectedConstructor.GetParameters();

        bool hasLocatorData = false;
        foreach (var parameter in parameters)
        {
            bool locatorData = parameter.ParameterType == typeof(LocatorChunk.LocatorData);

            var lvi = new ListViewItem(parameter.Name.ToFirstUpper());
            if (locatorData)
            {
                hasLocatorData = true;
                lvi.SubItems.Add("Locator Data");
            }
            else
            {
                var defaultVal = parameter.ParameterType.GetDefault();

                var parameterProperty = selectedConstructor.DeclaringType!.GetProperty(parameter.Name!, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (parameterProperty != null)
                {
                    var defaultValueAttribute = parameterProperty.GetCustomAttribute<DefaultValueAttribute>();
                    if (defaultValueAttribute != null)
                        defaultVal = Convert.ChangeType(defaultValueAttribute.Value, parameter.ParameterType);

                    var maxLengthAttribute = parameterProperty.GetCustomAttribute<MaxLengthAttribute>();
                    if (maxLengthAttribute != null)
                        MaxStringLengths[parameter.Name!] = maxLengthAttribute.MaxLength;
                }

                var subItem = lvi.SubItems.Add(parameter.ParameterType.IsEnumerable() ? "Array" : $"{defaultVal}");
                subItem.Tag = defaultVal;
            }
            lvi.Tag = parameter;
            LVValues.Items.Add(lvi);
        }

        if (hasLocatorData)
        {
            GBLocatorType.Enabled = true;
            GBLocatorType.Visible = true;

            GBValues.Location = new(GBValues.Location.X, GBLocatorType.Location.Y + GBLocatorType.Height + 6);
            GBValues.Size = new(GBValues.Width, BtnOK.Location.Y - GBValues.Location.Y - 6);
        }
        else
        {
            GBLocatorType.Enabled = false;
            GBLocatorType.Visible = false;

            GBValues.Location = new(GBValues.Location.X, GBChunkType.Location.Y + GBChunkType.Height + 6);
            GBValues.Size = new(GBValues.Width, BtnOK.Location.Y - GBValues.Location.Y - 6);
        }

        foreach (ColumnHeader column in LVValues.Columns)
            column.Width = -2;

        if (LVValues.Items.Count > 0)
            LVValues.Items[0].Selected = true;

        LVValues.EndUpdate();
    }

    private void LVValues_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (sender is not ListView lv)
            return;

        if (e.Button != MouseButtons.Left)
            return;

        var lvi = lv.GetItemAt(e.X, e.Y);
        if (lvi == null)
            return;

        if (lvi.Tag is not ParameterInfo parameter)
            return;

        var oldValue = lvi.SubItems[1].Tag;

        var parameterType = parameter.ParameterType;
        var parameterName = parameter.Name!.ToFirstUpper();

        if (parameterType.IsEnumerable())
        {
            MessageBox.Show("Arrays must be edited after creation.", "Unable to update value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (parameterType == typeof(LocatorChunk.LocatorData))
        {
            MessageBox.Show("Locator Data must be edited after creation.", "Unable to update value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        object? newValue = null;
        if (parameterType.HasFlagsAttribute())
        {
            using var enumFlagsEditor = new FrmEnumFlagsEditor(parameterType, parameterName, oldValue);
            if (enumFlagsEditor.ShowDialog() != DialogResult.OK)
                return;

            newValue = enumFlagsEditor.Value;
        }
        else if (parameterType.IsEnum)
        {
            using var enumEditor = new FrmEnumEditor(parameterType, parameterName, oldValue);
            if (enumEditor.ShowDialog() != DialogResult.OK)
                return;

            newValue = enumEditor.Value;
        }
        else if (parameterType == typeof(Color))
        {
            //using var colourEditor = new FrmColourEditor(parameterName, (Color?)oldValue);
            //if (colourEditor.ShowDialog() != DialogResult.OK)
            //    return;

            //newValue = colourEditor.Value;

            using var colorPicker = new Cyotek.Windows.Forms.ColorPickerDialog()
            {
                Color = (Color?)oldValue ?? Color.White,
                ShowAlphaChannel = true,
                Text = $"Edit Value: {parameterName}",
            };
            if (colorPicker.ShowDialog() != DialogResult.OK)
                return;

            newValue = colorPicker.Color;
        }
        else if (parameterType == typeof(bool))
        {
            using var booleanEditor = new FrmBooleanEditor(parameterName, (bool?)oldValue);
            if (booleanEditor.ShowDialog() != DialogResult.OK)
                return;

            newValue = booleanEditor.Value;
        }
        else if (parameterType == typeof(string))
        {
            if (!MaxStringLengths.TryGetValue(parameter.Name!, out var maxLength))
                maxLength = -1;

            var knownValuesAttribute = parameterType.GetCustomAttribute<KnownValuesAttribute>();

            if (knownValuesAttribute != null)
            {
                using var knownStringEditor = new FrmKnownStringEditor(parameterName, (string?)oldValue, knownValuesAttribute.Values, maxLength);
                if (knownStringEditor.ShowDialog() != DialogResult.OK)
                    return;

                newValue = knownStringEditor.Value;
            }
            else
            {
                using var stringEditor = new FrmStringEditor(parameterName, (string?)oldValue, maxLength);
                if (stringEditor.ShowDialog() != DialogResult.OK)
                    return;

                newValue = stringEditor.Value;
            }
        }
        else if (parameterType == typeof(char))
        {
            using var charEditor = new FrmCharEditor(parameterName, (char?)oldValue);
            if (charEditor.ShowDialog() != DialogResult.OK)
                return;

            newValue = charEditor.Value;
        }
        else if (NumericTextBox.GetNumericType(parameterType) != null)
        {
            using var numericEditor = new FrmNumericEditor(parameterName, oldValue);
            if (numericEditor.ShowDialog() != DialogResult.OK)
                return;

            if (numericEditor.Value == null)
            {
                MessageBox.Show("An invalid numeric value was entered. Value not updated.", "Error updating value", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            newValue = numericEditor.Value;
        }
        else if ((parameterType.IsValueType && !parameterType.IsEnum && !parameterType.IsPrimitive) || parameterType.IsClass)
        {
            if (oldValue == null)
                return;

            var reference = oldValue;
            using var structEditor = new FrmStructEditor(ref reference);
            if (structEditor.ShowDialog() != DialogResult.OK)
                return;

            newValue = reference;
        }
        else
        {
            MessageBox.Show($"Unknown item type \"{parameterType}\".", "Error updating value", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        lvi.SubItems[1].Tag = newValue;
        lvi.SubItems[1].Text = $"{newValue}";
    }

    private static readonly Dictionary<LocatorChunk.LocatorTypes, ConstructorInfo> LocatorDataConstructorMap = [];
    private LocatorChunk.LocatorData CreateLocatorData()
    {
        if (CBLocatorType.SelectedValue is not LocatorChunk.LocatorTypes locatorType)
            throw new Exception("A Locator Type must be specified.");

        if (!LocatorDataConstructorMap.TryGetValue(locatorType, out var constructor))
        {
            var type = locatorType switch
            {
                LocatorChunk.LocatorTypes.Event => typeof(LocatorChunk.EventLocatorData),
                LocatorChunk.LocatorTypes.Script => typeof(LocatorChunk.ScriptLocatorData),
                LocatorChunk.LocatorTypes.Generic => typeof(LocatorChunk.GenericLocatorData),
                LocatorChunk.LocatorTypes.CarStart => typeof(LocatorChunk.CarStartLocatorData),
                LocatorChunk.LocatorTypes.Spline => typeof(LocatorChunk.SplineLocatorData),
                LocatorChunk.LocatorTypes.DynamicZone => typeof(LocatorChunk.DynamicZoneLocatorData),
                LocatorChunk.LocatorTypes.Occlusion => typeof(LocatorChunk.OcclusionLocatorData),
                LocatorChunk.LocatorTypes.InteriorEntrance => typeof(LocatorChunk.InteriorEntranceLocatorData),
                LocatorChunk.LocatorTypes.Directional => typeof(LocatorChunk.DirectionalLocatorData),
                LocatorChunk.LocatorTypes.Action => typeof(LocatorChunk.ActionLocatorData),
                LocatorChunk.LocatorTypes.FOV => typeof(LocatorChunk.FOVLocatorData),
                LocatorChunk.LocatorTypes.BreakableCamera => typeof(LocatorChunk.BreakableCameraLocatorData),
                LocatorChunk.LocatorTypes.StaticCamera => typeof(LocatorChunk.StaticCameraLocatorData),
                LocatorChunk.LocatorTypes.PedGroup => typeof(LocatorChunk.PedGroupLocatorData),
                LocatorChunk.LocatorTypes.Coin => typeof(LocatorChunk.CoinLocatorData),
                _ => throw new Exception($"Unsupported Locator Type: {locatorType}.")
            };


            constructor = type.GetConstructors().FirstOrDefault(constructor =>
            {
                var parameters = constructor.GetParameters();
                return !(parameters.Length == 1 && parameters[0].ParameterType == typeof(IList<uint>));
            }) ?? throw new Exception($"No valid constructor found for Locator Type: {locatorType}.");

            LocatorDataConstructorMap[locatorType] = constructor;
        }

        return (LocatorChunk.LocatorData)constructor.Invoke([.. constructor.GetParameters().Select(x => x.ParameterType.GetDefault())]);
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
        var selectedConstructor = (ConstructorInfo?)CBChunkType.SelectedValue;
        if (selectedConstructor == null)
            return;
        Settings.LastNewChunkType = selectedConstructor.DeclaringType;
    }
}
