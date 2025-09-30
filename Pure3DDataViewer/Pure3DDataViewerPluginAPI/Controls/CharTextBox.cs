namespace Pure3DDataViewerPluginAPI.Controls;
public class CharTextBox : TextBox
{
    public override int MaxLength => 1;

    public char? Value
    {
        get => string.IsNullOrEmpty(Text) ? null : Text[0];
        set => Text = value?.ToString() ?? string.Empty;
    }

    public CharTextBox()
    {
        KeyPress += OnKeyPress;
        KeyDown += OnKeyDown;
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
            e.Handled = true;
    }

    private void OnKeyPress(object? sender, KeyPressEventArgs e)
    {
        if (char.IsControl(e.KeyChar))
            return;

        e.Handled = true;

        if (e.KeyChar > 255)
            return;

        Text = e.KeyChar.ToString();
        SelectionStart = 1;
    }
}
