using System.Reflection;

namespace Pure3DDataViewer;
partial class FrmAbout : Form
{
    private static readonly string[] Libraries = [
        "- NetP3DLib:\r\nhttps://github.com/Hampo/NetP3DLib",
        "- SHARMemory:\r\nhttps://github.com/Hampo/SHARMemory",
        "- Be.HexEditor:\r\nhttps://www.nuget.org/packages/Be.Windows.Forms.HexBox.Net8",
        "- Cyotek ColorPicker:\r\nhttps://www.nuget.org/packages/Cyotek.Windows.Forms.ColorPicker",
        "- DirectXTexNet:\r\nhttps://www.nuget.org/packages/DirectXTexNet",
    ];

    public FrmAbout()
    {
        InitializeComponent();
        this.Text = String.Format("About {0}", AssemblyTitle);
        this.labelProductName.Text = AssemblyProduct;
        this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
        this.labelCopyright.Text = AssemblyCopyright;
        this.labelCompanyName.Text = AssemblyCompany;
        this.textBoxDescription.Text = AssemblyDescription;

        textBoxDescription.Text += $"\r\n\r\nLibraries:\r\n{string.Join("\r\n\r\n", Libraries)}";
    }

    #region Assembly Attribute Accessors

    public string AssemblyTitle
    {
        get
        {
            var assembly = Assembly.GetExecutingAssembly();

            // Prefer the [AssemblyTitle] attribute if present
            var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            if (titleAttribute != null && !string.IsNullOrWhiteSpace(titleAttribute.Title))
            {
                return titleAttribute.Title;
            }

            // Fallback: use the file name without extension
            return assembly.GetName().Name ?? "Unknown";
        }
    }

    public string AssemblyVersion
    {
        get
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version?.ToString() ?? "1.0.0.0";
        }
    }

    public string AssemblyDescription
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyDescriptionAttribute)attributes[0]).Description;
        }
    }

    public string AssemblyProduct
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyProductAttribute)attributes[0]).Product;
        }
    }

    public string AssemblyCopyright
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
        }
    }

    public string AssemblyCompany
    {
        get
        {
            object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyCompanyAttribute), false);
            if (attributes.Length == 0)
            {
                return "";
            }
            return ((AssemblyCompanyAttribute)attributes[0]).Company;
        }
    }
    #endregion
}
