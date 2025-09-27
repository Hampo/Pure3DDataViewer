namespace Pure3DDataViewer.Controls;
public class NumericTextBox : TextBox
{
    public enum NumericTypes
    {
        SByte,
        Byte,
        Short,
        UShort,
        Int,
        UInt,
        Long,
        ULong,
        Float,
        Double,
        Decimal
    }
    private static readonly Dictionary<Type, NumericTypes> _numericTypeMap = new()
    {
        { typeof(sbyte), NumericTypes.SByte },
        { typeof(byte), NumericTypes.Byte },
        { typeof(short), NumericTypes.Short },
        { typeof(ushort), NumericTypes.UShort },
        { typeof(int), NumericTypes.Int },
        { typeof(uint), NumericTypes.UInt },
        { typeof(long), NumericTypes.Long },
        { typeof(ulong), NumericTypes.ULong },
        { typeof(float), NumericTypes.Float },
        { typeof(double), NumericTypes.Double },
        { typeof(decimal), NumericTypes.Decimal },
    };
    public static NumericTypes? GetNumericType(Type? numericType)
    {
        if (numericType == null || !_numericTypeMap.TryGetValue(numericType, out var value))
            return null;
        return value;
    }
    private static readonly HashSet<NumericTypes> UnsignedTypes =
        [
            NumericTypes.Byte,
            NumericTypes.UShort,
            NumericTypes.UInt,
            NumericTypes.ULong,
        ];
    private static readonly HashSet<NumericTypes> DecimalTypes =
        [
            NumericTypes.Float,
            NumericTypes.Double,
            NumericTypes.Decimal,
        ];

    private NumericTypes _numericType = NumericTypes.Int;
    [System.ComponentModel.Browsable(true)]
    [System.ComponentModel.DefaultValue(NumericTypes.Int)]
    public NumericTypes NumericType
    {
        get => _numericType;
        set
        {
            if (_numericType == value)
                return;

            _numericType = value;
            switch (_numericType)
            {
                case NumericTypes.SByte:
                    if (!sbyte.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Byte:
                    if (!byte.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Short:
                    if (!short.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.UShort:
                    if (!ushort.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Int:
                    if (!int.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.UInt:
                    if (!uint.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Long:
                    if (!long.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.ULong:
                    if (!ulong.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Float:
                    if (!float.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Double:
                    if (!double.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
                case NumericTypes.Decimal:
                    if (!decimal.TryParse(Text, out _))
                        Text = string.Empty;
                    break;
            }
            _numericType = value;
        }
    }

    [System.ComponentModel.Browsable(true)]
    public Color ValidColor { get; set; } = Color.White;

    [System.ComponentModel.Browsable(true)]
    public Color InvalidColor { get; set; } = Color.Pink;

    public object? Value
    {
        get
        {
            return GetValue(Text);
        }
        set => Text = value?.ToString() ?? string.Empty;
    }

    internal object? GetValue(string text)
    {
        switch (NumericType)
        {
            case NumericTypes.SByte:
                if (sbyte.TryParse(text, out var sbyteVal))
                    return sbyteVal;
                break;
            case NumericTypes.Byte:
                if (byte.TryParse(text, out var byteVal))
                    return byteVal;
                break;
            case NumericTypes.Short:
                if (short.TryParse(text, out var shortVal))
                    return shortVal;
                break;
            case NumericTypes.UShort:
                if (ushort.TryParse(text, out var ushortVal))
                    return ushortVal;
                break;
            case NumericTypes.Int:
                if (int.TryParse(text, out var intVal))
                    return intVal;
                break;
            case NumericTypes.UInt:
                if (uint.TryParse(text, out var uintVal))
                    return uintVal;
                break;
            case NumericTypes.Long:
                if (long.TryParse(text, out var longVal))
                    return longVal;
                break;
            case NumericTypes.ULong:
                if (ulong.TryParse(text, out var ulongVal))
                    return ulongVal;
                break;
            case NumericTypes.Float:
                if (float.TryParse(text, out var floatVal))
                    return floatVal;
                break;
            case NumericTypes.Double:
                if (double.TryParse(text, out var doubleVal))
                    return doubleVal;
                break;
            case NumericTypes.Decimal:
                if (decimal.TryParse(text, out var decimalVal))
                    return decimalVal;
                break;
        }
        return null;
    }

    public NumericTextBox()
    {
        KeyPress += OnKeyPress;
        KeyDown += OnKeyDown;
        TextChanged += OnTextChanged;
    }
    protected override void WndProc(ref Message m)
    {
        const int WM_PASTE = 0x0302; // Windows message for paste operation

        // Intercept the WM_PASTE message and ignore it
        if (m.Msg == WM_PASTE)
        {
            // Optionally, you can display a message or log the paste attempt
            // MessageBox.Show("Pasting is disabled.");
            return;
        }

        // Call base method for default processing of other messages
        base.WndProc(ref m);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if(e.Control && e.KeyCode == Keys.V || e.Shift && e.KeyCode == Keys.Insert)
        {
            e.Handled = true;

            var start = SelectionStart;
            var clipboardText = Clipboard.GetText();
            string newText = $"{Text[..start]}{clipboardText}{Text[(SelectionStart + SelectionLength)..]}";
            object? newValue = GetValue(newText);
            if (newValue != null)
            {
                Value = newValue;
                SelectionStart = start + clipboardText.Length;
            }    
        }
    }

    private void OnKeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            return;

        switch (e.KeyChar)
        {
            case '-':
                if (SelectionStart != 0)
                {
                    e.Handled = true;
                    return;
                }

                if (UnsignedTypes.Contains(NumericType))
                {
                    e.Handled = true;
                    return;
                }

                break;
            case '.':
                if (Text.Contains('.'))
                {
                    e.Handled = true;
                    return;
                }

                if (!DecimalTypes.Contains(NumericType))
                {
                    e.Handled = true;
                    return;
                }

                break;
        }

        if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '.')
        {
            e.Handled = true;
            return;
        }
    }

    private void OnTextChanged(object? sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Text))
        {
            BackColor = ValidColor;
            return;
        }

        bool isValid = false;

        switch (NumericType)
        {
            case NumericTypes.SByte:
                isValid = sbyte.TryParse(Text, out _);
                break;
            case NumericTypes.Byte:
                isValid = byte.TryParse(Text, out _);
                break;
            case NumericTypes.Short:
                isValid = short.TryParse(Text, out _);
                break;
            case NumericTypes.UShort:
                isValid = ushort.TryParse(Text, out _);
                break;
            case NumericTypes.Int:
                isValid = int.TryParse(Text, out _);
                break;
            case NumericTypes.UInt:
                isValid = uint.TryParse(Text, out _);
                break;
            case NumericTypes.Long:
                isValid = long.TryParse(Text, out _);
                break;
            case NumericTypes.ULong:
                isValid = ulong.TryParse(Text, out _);
                break;
            case NumericTypes.Float:
                isValid = float.TryParse(Text, out _);
                break;
            case NumericTypes.Double:
                isValid = double.TryParse(Text, out _);
                break;
            case NumericTypes.Decimal:
                isValid = decimal.TryParse(Text, out _);
                break;
        }

        BackColor = isValid ? ValidColor : InvalidColor;
    }
}
