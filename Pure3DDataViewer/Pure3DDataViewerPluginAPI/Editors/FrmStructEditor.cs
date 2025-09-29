using NetP3DLib.P3D.Attributes;
using Pure3DDataViewerPluginAPI.Controls;
using Pure3DDataViewerPluginAPI.Extensions;
using System.Data;
using System.Diagnostics;
using System.Reflection;

namespace Pure3DDataViewerPluginAPI.Editors;
public partial class FrmStructEditor : Form
{
    private readonly object _struct;
    private readonly MemberInfo[] _members;

    public FrmStructEditor(ref object structObject)
    {
        _struct = structObject;
        _members = structObject.GetType()
            .GetMembers(BindingFlags.Public | BindingFlags.Instance)
            .Where(m => (m.MemberType == MemberTypes.Field && m is FieldInfo) ||
                        (m.MemberType == MemberTypes.Property && m is PropertyInfo property && property.GetIndexParameters().Length == 0 && property.CanWrite))
            .ToArray();

        InitializeComponent();

        SuspendLayout();

        var nameColumnWidth = 0;
        foreach (var member in _members)
        {
            try
            {
                string name = member.Name;
                object? value = GetMemberValue(member);

                AddMember(ref nameColumnWidth, member, name, value);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"There was an error loading struct values: {ex}", "Error loading values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debugger.Break();
            }
        }
        TLP1.ColumnStyles[0].Width = nameColumnWidth + 15;

        ResumeLayout(true);
        Width = TLP1.Width + 35;
        Height = TLP1.Height + BtnOK.Height + 70;
    }

    public void AddMember(ref int nameColumnWidth, MemberInfo member, string name, object? value, MemberInfo? subMember = null)
    {
        var valueType = (subMember ?? member).GetUnderlyingType();
        var nullableType = Nullable.GetUnderlyingType(valueType);
        if (nullableType == null && (subMember ?? member).MemberType == MemberTypes.Property && (subMember ?? member) is PropertyInfo property && property.IsStruct())
        {
            var structMembers = property.PropertyType.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => (m.MemberType == MemberTypes.Field && m is FieldInfo) ||
                            (m.MemberType == MemberTypes.Property && m is PropertyInfo property && property.GetIndexParameters().Length == 0 && property.CanWrite))
                .ToArray();

            foreach (var structMember in structMembers)
            {
                string structName = structMember.Name;
                object? structValue = GetMemberValue(structMember, value);

                AddMember(ref nameColumnWidth, member, $"{name}.{structName}", structValue, structMember);
            }

            return;
        }
        valueType = nullableType ?? valueType;

        Label lbl = new()
        {
            Text = $"{name}:",
            AutoSize = false,
            Dock = DockStyle.Fill,
            TextAlign = ContentAlignment.MiddleRight
        };
        nameColumnWidth = Math.Max(nameColumnWidth, TextRenderer.MeasureText(lbl.Text, lbl.Font).Width);
        TLP1.Controls.Add(lbl);

        var numericType = NumericTextBox.GetNumericType(valueType);
        if (numericType != null)
        {
            NumericTextBox txt = new()
            {
                Text = value?.ToString() ?? string.Empty,
                Width = 250,
                Tag = subMember == null ? member : (member, subMember),
                NumericType = numericType.Value,
                BackColor = Color.White,
            };
            TLP1.Controls.Add(txt);
        }
        else if (valueType.IsEnum)
        {
            ComboBox cb = new()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 250,
                Tag = subMember == null ? member : (member, subMember),
            };
            TLP1.Controls.Add(cb);
            cb.DataSource = Enum.GetValues(valueType);
            cb.SelectedItem = value;
        }
        else if (valueType == typeof(Color))
        {
            ColorPicker cp = new()
            {
                Value = (Color)value!,
                Width = 250,
                Tag = subMember == null ? member : (member, subMember),
            };
            TLP1.Controls.Add(cp);
        }
        else if (valueType == typeof(bool))
        {
            CheckBox cb = new()
            {
                Text = "",
                Checked = (bool?)value ?? false,
                Width = 250,
                Tag = subMember == null ? member : (member, subMember),
            };
            TLP1.Controls.Add(cb);
        }
        else
        {
            var maxLengthAttribute = member.GetCustomAttribute<MaxLengthAttribute>();
            var knownValuesAttribute = member.GetCustomAttribute<KnownValuesAttribute>();

            if (knownValuesAttribute != null)
            {
                var autocomplete = new AutoCompleteStringCollection();
                autocomplete.AddRange(knownValuesAttribute.Values);
                ComboBox cb = new()
                {
                    AutoCompleteSource = AutoCompleteSource.CustomSource,
                    AutoCompleteMode = AutoCompleteMode.SuggestAppend,
                    AutoCompleteCustomSource = autocomplete,
                    Text = value?.ToString() ?? string.Empty,
                    Width = 250,
                    Tag = subMember == null ? member : (member, subMember),
                    MaxLength = maxLengthAttribute?.MaxLength ?? 255
                };
                cb.Items.AddRange(knownValuesAttribute.Values);
                TLP1.Controls.Add(cb);
            }
            else
            {
                TextBox txt = new()
                {
                    Text = value?.ToString() ?? string.Empty,
                    Width = 250,
                    Tag = subMember == null ? member : (member, subMember),
                    MaxLength = maxLengthAttribute?.MaxLength ?? 255
                };
                TLP1.Controls.Add(txt);
            }
        }
    }

    private object? GetMemberValue(MemberInfo member, object? obj = null)
    {
        if (member is FieldInfo field)
            return field.GetValue(obj ?? _struct);

        if (member is PropertyInfo property)
            return property.GetMethod?.Invoke(obj ?? _struct, null);

        return null;
    }

    private void SetMemberValue(MemberInfo member, object? value, object? obj = null)
    {
        if (member is FieldInfo field)
            field.SetValue(obj ?? _struct, value);

        if (member is PropertyInfo property)
            property.SetMethod?.Invoke(obj ?? _struct, [value]);
    }

    private void BtnOK_Click(object sender, EventArgs e)
    {
        foreach (var textBox in TLP1.Controls.OfType<TextBox>())
        {
            if (textBox.Tag is MemberInfo member)
            {
                var memberType = member.GetUnderlyingType();

                var nullableType = Nullable.GetUnderlyingType(memberType);
                if (nullableType != null && string.IsNullOrEmpty(textBox.Text))
                {
                    SetMemberValue(member, null);
                }
                else
                {
                    object value = Convert.ChangeType(textBox.Text, nullableType ?? memberType);
                    SetMemberValue(member, value);
                }
            }
            else if (textBox.Tag is (MemberInfo parentMember, MemberInfo structMember))
            {
                var structMemberType = structMember.GetUnderlyingType();

                object? originalObject = GetMemberValue(parentMember)!;

                var nullableType = Nullable.GetUnderlyingType(structMemberType);
                if (nullableType != null && string.IsNullOrEmpty(textBox.Text))
                {
                    SetMemberValue(structMember, null, originalObject);
                }
                else
                {
                    object value = Convert.ChangeType(textBox.Text, nullableType ?? structMemberType);
                    SetMemberValue(structMember, value, originalObject);
                }

                SetMemberValue(parentMember, originalObject);
            }
        }

        foreach (var comboBox in TLP1.Controls.OfType<ComboBox>())
        {
            if (comboBox.Tag is MemberInfo member)
            {
                SetMemberValue(member, comboBox.SelectedItem ?? comboBox.Text);
            }
            else if (comboBox.Tag is (MemberInfo parentMember, MemberInfo structMember))
            {
                object? originalObject = GetMemberValue(parentMember)!;

                SetMemberValue(structMember, comboBox.SelectedItem ?? comboBox.Text, originalObject);

                SetMemberValue(parentMember, originalObject);
            }
        }

        foreach (var colorPicker in TLP1.Controls.OfType<ColorPicker>())
        {
            if (colorPicker.Tag is MemberInfo member)
            {
                SetMemberValue(member, colorPicker.Value);
            }
            else if (colorPicker.Tag is (MemberInfo parentMember, MemberInfo structMember))
            {
                object? originalObject = GetMemberValue(parentMember)!;

                SetMemberValue(structMember, colorPicker.Value, originalObject);

                SetMemberValue(parentMember, originalObject);
            }
        }

        foreach (var checkBox in TLP1.Controls.OfType<CheckBox>())
        {
            if (checkBox.Tag is MemberInfo member)
            {
                SetMemberValue(member, checkBox.Checked);
            }
            else if (checkBox.Tag is (MemberInfo parentMember, MemberInfo structMember))
            {
                object? originalObject = GetMemberValue(parentMember)!;

                SetMemberValue(structMember, checkBox.Checked, originalObject);

                SetMemberValue(parentMember, originalObject);
            }
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void FrmStructEditor_Shown(object sender, EventArgs e)
    {
        if (TLP1.Controls.Count >= 2)
            TLP1.Controls[1].Focus();
    }
}
