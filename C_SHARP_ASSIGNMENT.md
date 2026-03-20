# C# Assignment Runbook (Linux / WSL)

This repository now includes:

- `csharp-rest-api` (ASP.NET Core REST API + WebSocket endpoint in one project)

## 1) REST API

Run locally:

```bash
cd csharp-rest-api
dotnet run --urls http://0.0.0.0:5001
```

Key endpoints:

- `GET /` basic service info
- `GET /healthz` health check
- `GET /api/todos`
- `GET /api/todos/{id}`
- `POST /api/todos`
- `PUT /api/todos/{id}`
- `DELETE /api/todos/{id}`

Swagger UI (development):

- `http://localhost:5001/swagger`

## 2) WebSocket (same project)

Run locally:

```bash
cd csharp-rest-api
dotnet run --urls http://0.0.0.0:5001
```

Key endpoints:

- `GET /` basic service info
- `GET /healthz` health check
- `WS /ws` websocket endpoint (broadcast to all connected clients)

Quick test using `wscat`:

```bash
npx wscat -c ws://127.0.0.1:5001/ws
```

## 3) Linux Deployment (simple)

Publish binaries:

```bash
dotnet publish csharp-rest-api -c Release -o out/rest
```

Run published app:

```bash
ASPNETCORE_URLS=http://0.0.0.0:5001 ./out/rest/csharp-rest-api
```

For production, run them as managed services (systemd or containers), put them behind Nginx, and enable structured logging + monitoring.
