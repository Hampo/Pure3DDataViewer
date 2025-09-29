using System.Runtime.InteropServices;

namespace Pure3DDataViewerPluginAPI.Controls;

public class ExplorerThemedTreeView : TreeView
{
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
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            Microsoft.Win32.SystemEvents.UserPreferenceChanged -= SystemEvents_UserPreferenceChanged;
        }
        base.Dispose(disposing);
    }

    private void SystemEvents_UserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
    {
        if (e.Category == Microsoft.Win32.UserPreferenceCategory.VisualStyle)
        {
            base.HotTracking = Application.RenderWithVisualStyles;
        }
    }

    [DllImport("uxtheme.dll", ExactSpelling = true, CharSet = CharSet.Unicode)]
    private static extern int SetWindowTheme(nint hwnd, string pszSubAppName, string? pszSubIdList);

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
}
