using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace csharp_rest_api.Services;

public sealed class WebSocketRealtimeNotifier : IRealtimeNotifier
{
    private readonly ConcurrentDictionary<Guid, WebSocket> _clients = new();

    public async Task HandleConnectionAsync(HttpContext context, CancellationToken cancellationToken = default)
    {
        if (!context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("WebSocket requests only.", cancellationToken);
            return;
        }

        using var socket = await context.WebSockets.AcceptWebSocketAsync();
        var clientId = Guid.NewGuid();
        _clients[clientId] = socket;

        await BroadcastAsync("client.joined", new { clientId }, cancellationToken);
        await ReceiveLoopAsync(clientId, socket, cancellationToken);
    }

    public async Task BroadcastAsync(string eventName, object payload, CancellationToken cancellationToken = default)
    {
        var envelope = JsonSerializer.Serialize(new
        {
            @event = eventName,
            data = payload,
            sentAtUtc = DateTime.UtcNow
        });

        var bytes = Encoding.UTF8.GetBytes(envelope);
        var message = new ArraySegment<byte>(bytes);

        foreach (var client in _clients.ToArray())
        {
            var socket = client.Value;
            if (socket.State != WebSocketState.Open)
            {
                _clients.TryRemove(client.Key, out _);
                continue;
            }

            await socket.SendAsync(message, WebSocketMessageType.Text, true, cancellationToken);
        }
    }

    private async Task ReceiveLoopAsync(Guid clientId, WebSocket socket, CancellationToken cancellationToken)
    {
        var buffer = new byte[4096];

        try
        {
            while (socket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                if (result.MessageType != WebSocketMessageType.Text)
                {
                    continue;
                }

                var text = Encoding.UTF8.GetString(buffer, 0, result.Count);
                await BroadcastAsync("chat.message", new { clientId, text }, cancellationToken);
            }
        }
        finally
        {
            _clients.TryRemove(clientId, out _);

            if (socket.State == WebSocketState.Open || socket.State == WebSocketState.CloseReceived)
            {
                await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "closing", cancellationToken);
            }

            await BroadcastAsync("client.left", new { clientId }, cancellationToken);
        }
    }
}
