using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHealthChecks();

var app = builder.Build();
var sockets = new ConcurrentDictionary<Guid, WebSocket>();

app.UseWebSockets();

app.MapGet("/", () => Results.Ok(new
{
    service = "csharp-websocket-app",
    websocketPath = "/ws",
    status = "ok"
}));

app.MapHealthChecks("/healthz");

app.Map("/ws", async context =>
{
    if (!context.WebSockets.IsWebSocketRequest)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        await context.Response.WriteAsync("WebSocket requests only.");
        return;
    }

    using var socket = await context.WebSockets.AcceptWebSocketAsync();
    var id = Guid.NewGuid();
    sockets[id] = socket;

    await BroadcastAsync(sockets, $"[join] client:{id}");
    await ReceiveLoopAsync(id, socket, sockets);
});

static async Task ReceiveLoopAsync(Guid id, WebSocket socket, ConcurrentDictionary<Guid, WebSocket> sockets)
{
    var buffer = new byte[4096];
    try
    {
        while (socket.State == WebSocketState.Open)
        {
            var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }

            if (result.MessageType != WebSocketMessageType.Text)
            {
                continue;
            }

            var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
            await BroadcastAsync(sockets, $"[{id}] {text}");
        }
    }
    finally
    {
        sockets.TryRemove(id, out _);
        if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
        {
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", CancellationToken.None);
        }
        await BroadcastAsync(sockets, $"[left] client:{id}");
    }
}

static async Task BroadcastAsync(ConcurrentDictionary<Guid, WebSocket> sockets, string message)
{
    var payload = Encoding.UTF8.GetBytes(message);
    var segment = new ArraySegment<byte>(payload);

    foreach (var client in sockets.ToArray())
    {
        if (client.Value.State != WebSocketState.Open)
        {
            sockets.TryRemove(client.Key, out _);
            continue;
        }

        await client.Value.SendAsync(segment, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}

app.Run();
