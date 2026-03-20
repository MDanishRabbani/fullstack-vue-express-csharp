# Demo Guide

## A) Vue + Express Demo

### 1. Start backend

```bash
cd backend
npm run local
```

### 2. Start frontend

```bash
cd frontend/apps
npm run sample
```

### 3. Demo login

- Open `http://127.0.0.1:8080`
- Username: `test`
- Password: `test`
- OTP: `111111`
- Expected: redirect to Dashboard

## B) C# REST + WebSocket Demo (single project)

### 1. Start app

```bash
cd csharp-rest-api
dotnet run --urls http://127.0.0.1:5101
```

### 2. Health and REST checks

```bash
curl -s http://127.0.0.1:5101/healthz
curl -s http://127.0.0.1:5101/api/todos
```

### 3. Open 2 WebSocket clients

Terminal A:

```bash
wscat -c ws://127.0.0.1:5101/ws
```

Terminal B:

```bash
wscat -c ws://127.0.0.1:5101/ws
```

### 4. Trigger realtime events from REST

```bash
curl -s -X POST http://127.0.0.1:5101/api/todos \
  -H "content-type: application/json" \
  -d '{"title":"Realtime demo","isDone":false}'

curl -s -X PUT http://127.0.0.1:5101/api/todos/1 \
  -H "content-type: application/json" \
  -d '{"title":"Updated from REST","isDone":true}'

curl -s -X DELETE http://127.0.0.1:5101/api/todos/2
```

Expected on both clients:

- `todo.created`
- `todo.updated`
- `todo.deleted`

## C) Interview Talking Points

- REST layer handles CRUD operations
- WebSocket layer handles realtime event delivery without polling
- `CreatedAtUtc` and `UpdatedAtUtc` provide simple audit metadata for todo items
