using Pure3DDataViewerPluginAPI.Interfaces;
using System.Reflection;

namespace Pure3DDataViewer;
public static class PluginLoader
{
    public static List<IPlugin> Plugins { get; } = [];

    public static void LoadPlugins(string dir)
    {
        if (!Directory.Exists(dir))
            return;

        var dlls = Directory.GetFiles(dir, "*.dll");
        if (dlls.Length == 0)
            return;

        foreach (var dllFile in dlls)
        {
            try
            {
                var asm = Assembly.LoadFrom(dllFile);

                foreach (var type in asm.GetTypes())
                    if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        if (Activator.CreateInstance(type) is IPlugin plugin)
                            Plugins.Add(plugin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load plugin \"{Path.GetFileName(dllFile)}\": {ex}", "Load Plugins", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
