using Pure3DDataViewerPluginAPIMemoryTunnel;
using System.IO.Pipes;
using System.Text.Json;

Console.WriteLine("This should only be called through a Pure3DDataViewer Plugin.");

var server = new NamedPipeServerStream(Pure3DDataViewerPluginAPI.MemoryTunnel.TunnelName, PipeDirection.InOut, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message);

Console.WriteLine("Waiting for connection...");
server.WaitForConnection();

Console.WriteLine("Connected.");

using var sr = new StreamReader(server);
using var sw = new StreamWriter(server) { AutoFlush = true };

while(server.IsConnected)
{
    var request = sr.ReadLine();

    Console.WriteLine($"Received request: {request}");

    switch (request)
    {
        case "GetPosition":
            var pos = MemoryTunnel.GetPosition();
            var result = pos != null ? JsonSerializer.Serialize(pos) : "";
            sw.WriteLine(result);
            Console.WriteLine($"Sent: {result}");
            break;
        default:
            Console.WriteLine($"Unknown request");
            break;
    }
}