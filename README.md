## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js](https://nodejs.org/) (v18+)

## Run Backend

```bash
cd backend/Api
dotnet run
```

The API starts at `http://localhost:5255`. Swagger UI is available at `/swagger`.

The SQLite database (`customers.db`) is created automatically on first run.

## Run Frontend

```bash
cd frontend
npm install
npm start
```

The UI starts at `http://localhost:3000`.

## Run Tests

```bash
cd backend
dotnet test
```

## API Endpoints

| Method | Endpoint              | Description           |
| ------ | --------------------- | --------------------- |
| POST   | `/api/customers`      | Create a new customer |
| GET    | `/api/customers`      | List all customers    |
| GET    | `/api/customers/{id}` | Get customer by ID  |

### Create Customer Request Body

```json
{
  "firstName": "Carlo",
  "lastName": "Caliboso",
  "email": "carlo.caliboso@email.com",
  "phoneNumber": "+63 917 123 4567",
  "signatureBase64": "data:image/png;base64,..."
}
```

## Features

- Clean architecture with dependency injection
- FluentValidation for input validation
- SQLite file-based database (no external setup)
- Canvas-based signature capture (stored as Base64)
- Error handling middleware with structured error responses
- Unit tests for customer creation logic and validation rules
