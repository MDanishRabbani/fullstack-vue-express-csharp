# fullstack-vue-express-csharp

Repository ini berisi implementasi assignment:

- VueJS + ExpressJS (login sampai dashboard)
- C# REST API
- C# WebSocket realtime (terintegrasi di project REST)

## Project Structure

- `backend/` - Express template backend
- `frontend/` - Vue + Antd template frontend
- `csharp-rest-api/` - ASP.NET Core REST API + WebSocket (`/ws`)
- `docs/` - setup dan demo guide

## Quick Start

Lihat dokumentasi:

- [Setup Guide](docs/setup-guide.md)
- [Demo Guide](docs/demo-guide.md)

## Implemented Highlights

### Vue + Express

- Backend dan frontend berjalan lokal
- Konfigurasi OTP test flow stabil (`111111`)
- Login dari frontend sukses ke dashboard
- Error handling signin lebih jelas

### C# REST API

- CRUD endpoint `/api/todos`
- Audit fields: `CreatedAtUtc`, `UpdatedAtUtc`
- Health endpoint `/healthz`
- Swagger aktif saat development

### Realtime WebSocket

- Endpoint `ws://<host>/ws`
- Broadcast event saat todo create/update/delete
- Event `client.joined` dan `client.left`

## Demo Credentials (Vue + Express sample)

- Username: `test`
- Password: `test`
- OTP: `111111`

## Notes

- Repository ini disiapkan untuk demo di Linux/WSL sesuai assignment.
- Screenshot hasil demo bisa ditaruh di `docs/screenshots/`.
