# Setup Guide

Guide ini mencakup setup semua project di repository ini pada Linux/WSL Ubuntu.

## Prerequisites

- Node.js 22+ dan npm 10+
- .NET SDK 8
- Git
- Optional: `wscat` untuk test websocket (`npm i -g wscat`)

## 1) Backend (Express)

```bash
cd backend
npm install
npm run local
```

Default backend:

- API: `http://127.0.0.1:3000`
- WS: `ws://127.0.0.1:3001`

Catatan:

- Konfigurasi env ada di `backend/apps/.env.development`
- OTP test mode menggunakan `111111` saat `USE_OTP=TEST`

## 2) Frontend (Vue)

```bash
cd frontend/apps
npm install
npm run sample
```

Frontend URL:

- `http://127.0.0.1:8080`

Catatan:

- Proxy dev frontend sudah diarahkan ke backend lokal (`127.0.0.1:3000`)
- Login sample: `test / test`, lalu OTP `111111`

## 3) C# REST API (+ WebSocket dalam project yang sama)

```bash
cd csharp-rest-api
dotnet restore
dotnet run --urls http://127.0.0.1:5001
```

Endpoints:

- `GET /` service info
- `GET /healthz`
- `GET /api/todos`
- `GET /api/todos/{id}`
- `POST /api/todos`
- `PUT /api/todos/{id}`
- `DELETE /api/todos/{id}`
- `WS /ws`

## Build Check

```bash
cd backend && npm run lint || true
cd ../frontend && npm run -s test || true
cd ../csharp-rest-api && dotnet build
```

> Beberapa script frontend/backend di template ini bersifat sample; yang wajib untuk assignment adalah app bisa dijalankan dan didemokan.
