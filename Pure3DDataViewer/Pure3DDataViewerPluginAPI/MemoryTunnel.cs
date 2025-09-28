using Pure3DDataViewerPluginAPI.Models;
using System.Diagnostics;
using System.IO.Pipes;
using System.Text.Json;

namespace Pure3DDataViewerPluginAPI;
public static class MemoryTunnel
{
    public const string TunnelName = "Pure3DDataViewerPluginAPIMemoryTunnel";
    private static readonly string TunnelExePath = Path.Combine(AppContext.BaseDirectory, "Pure3DDataViewerPluginAPIMemoryTunnel.exe");

    public static bool TunnelExeExists => File.Exists(TunnelExePath);

    private static string? Send(string request)
    {
        if (!TunnelExeExists)
            throw new Exception($"Tunnel executable not found at \"{TunnelExePath}\".");

        foreach (var p in Process.GetProcessesByName("Pure3DDataViewerPluginAPIMemoryTunnel"))
            p.Kill();

        var tunnelStartInfo = new ProcessStartInfo
        {
            FileName = TunnelExePath,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = false,
            RedirectStandardInput = false
        };

        var tunnelProcess = Process.Start(tunnelStartInfo);
        if (tunnelProcess == null)
            return null;

        NamedPipeClientStream? pipeClient = null;
        try
        {

            int retries = 10;
            while (true)
            {
                try
                {
                    pipeClient = new NamedPipeClientStream(".", TunnelName, PipeDirection.InOut);
                    pipeClient.Connect(1000);
                    break;
                }
                catch
                {
                    if (--retries == 0)
                        return null;
                    Thread.Sleep(200);
                }
            }

            using var sr = new StreamReader(pipeClient);
            using var sw = new StreamWriter(pipeClient) { AutoFlush = true };

            sw.WriteLine(request);
            var json = sr.ReadLine();

            return json;
        }
        catch
        {
            return null;
        }
        finally
        {
            pipeClient?.Close();
            if (!tunnelProcess.HasExited)
                tunnelProcess.Kill();
            tunnelProcess.Dispose();
        }
    }

    public static PositionAndRotation? GetPosition()
    {
        var json = Send("GetPosition");
        if (string.IsNullOrWhiteSpace(json))
            return null;

        return JsonSerializer.Deserialize<PositionAndRotation>(json);
    }
}
