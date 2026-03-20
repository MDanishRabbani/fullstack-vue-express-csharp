using csharp_rest_api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
builder.Services.AddProblemDetails();
builder.Services.AddSingleton<ITodoService, InMemoryTodoService>();
builder.Services.AddSingleton<IRealtimeNotifier, WebSocketRealtimeNotifier>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseWebSockets();
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/healthz");
app.Map("/ws", async (HttpContext context, IRealtimeNotifier realtimeNotifier) =>
{
    await realtimeNotifier.HandleConnectionAsync(context);
});
app.MapGet("/", () => Results.Ok(new
{
    service = "csharp-rest-api",
    status = "ok",
    websocketPath = "/ws"
}));

app.Run();
