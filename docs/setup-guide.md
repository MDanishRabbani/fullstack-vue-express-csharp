# Setup Guide

This guide covers setup for all projects in this repository on Linux/WSL Ubuntu.

## Prerequisites

- Node.js 22+ and npm 10+
- .NET SDK 8
- Git
- Optional: `wscat` for WebSocket testing (`npm i -g wscat`)

## 1) Backend (Express)

```bash
cd backend
npm install
npm run local
```

Default backend:

- API: `http://127.0.0.1:3000`
- WS: `ws://127.0.0.1:3001`

Notes:

- Environment config is in `backend/apps/.env.development`
- OTP test mode uses `111111` when `USE_OTP=TEST`

## 2) Frontend (Vue)

```bash
cd frontend/apps
npm install
npm run sample
```

Frontend URL:

- `http://127.0.0.1:8080`

Notes:

- Frontend dev proxy is configured to local backend (`127.0.0.1:3000`)
- Sample login: `test / test`, then OTP `111111`

## 3) C# REST API (+ WebSocket in the same project)

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

> Some frontend/backend scripts in these templates are sample-oriented; for the assignment, the key requirement is that apps run and can be demonstrated successfully.
