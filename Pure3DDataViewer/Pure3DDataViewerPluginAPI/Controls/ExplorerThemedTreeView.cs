using System.Reflection;
using System.Runtime.InteropServices;

namespace Pure3DDataViewerPluginAPI.Controls;

public class ExplorerThemedTreeView : TreeView
{
    private const int TV_FIRST = 0x1100;
    private const int TVM_SETINSERTMARK = TV_FIRST + 26;
    private const int WM_ERASEBKGND = 0x14;

    [DllImport("user32.dll")]
    private static extern nint SendMessage(nint hWnd, int msg, nint wParam, nint lParam);

    [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(nint hwnd, string pszSubAppName, string? pszSubIdList);

    [System.ComponentModel.DefaultValue(false)]
    public new bool HideSelection
    {
        get => base.HideSelection;
        set => base.HideSelection = value;
    }

    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
    [System.ComponentModel.Bindable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public new bool HotTracking
    {
        get => base.HotTracking;
        set => throw new NotSupportedException();
    }

    public ExplorerThemedTreeView()
    {
        HideSelection = false;

        if (Environment.OSVersion.Version >= new Version(6, 0))
        {
            base.HotTracking = Application.RenderWithVisualStyles;
            Microsoft.Win32.SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
        }

        typeof(TreeView).InvokeMember("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.SetProperty, null, this, [true]);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Microsoft.Win32.SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        }
        base.Dispose(disposing);
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == WM_ERASEBKGND)
        {
            m.Result = 1;
            return;
        }
        base.WndProc(ref m);
    }

    private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
    {
        if (e.Category == Microsoft.Win32.UserPreferenceCategory.VisualStyle)
        {
            base.HotTracking = Application.RenderWithVisualStyles;
        }
    }

    protected override void OnHandleCreated(EventArgs e)
    {
        base.OnHandleCreated(e);

        try
        {
            _ = SetWindowTheme(Handle, "Explorer", null);
        }
        catch (EntryPointNotFoundException)
        {
        }
        catch (DllNotFoundException)
        {
        }
        catch (NullReferenceException)
        {
        }
    }

    public void SetInsertMark(TreeNode? node, bool after)
    {
        if (node == null)
        {
            SendMessage(Handle, TVM_SETINSERTMARK, 0, 0);
            return;
        }
        SendMessage(Handle, TVM_SETINSERTMARK, after ? 1 : 0, node.Handle);
    }
}
