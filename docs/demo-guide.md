# Demo Guide

## A) Demo Vue + Express

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
- Expected: redirect ke Dashboard

## B) Demo C# REST + WebSocket (single project)

### 1. Start app

```bash
cd csharp-rest-api
dotnet run --urls http://127.0.0.1:5101
```

### 2. Health & REST checks

```bash
curl -s http://127.0.0.1:5101/healthz
curl -s http://127.0.0.1:5101/api/todos
```

### 3. Open 2 websocket clients

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

Expected di client A/B:

- `todo.created`
- `todo.updated`
- `todo.deleted`

## C) Talking points (interview)

- REST layer untuk CRUD data
- WebSocket layer untuk event realtime tanpa polling
- `CreatedAtUtc` dan `UpdatedAtUtc` pada todo untuk audit trail ringan
