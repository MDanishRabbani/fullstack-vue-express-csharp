namespace csharp_rest_api.Services;

public interface IRealtimeNotifier
{
    Task HandleConnectionAsync(HttpContext context, CancellationToken cancellationToken = default);
    Task BroadcastAsync(string eventName, object payload, CancellationToken cancellationToken = default);
}
