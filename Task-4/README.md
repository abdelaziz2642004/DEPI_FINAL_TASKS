# BookStore Web API — Task 4

A .NET 8 Web API for the bookstore with JWT authentication, two roles (customer / admin), EF Core, Swagger, CORS, and global error handling.

---

## Requirements

- .NET 8 SDK
- SQL Server LocalDB (ships with Visual Studio)
- `dotnet-ef` global tool: `dotnet tool install --global dotnet-ef`

---

## How to Run

```bash
cd Task-4/BookStoreApi
dotnet run
```

The API starts at **http://localhost:5050**.  
Swagger UI is available at **http://localhost:5050** (root).

On first run the app:
1. Applies all pending migrations (creates the database).
2. Starts and waits for requests.

---

## How to Register the First Admin

1. Register a normal account:
```http
POST /api/auth/register
{ "username": "admin", "email": "admin@bookstore.com", "password": "Admin123!" }
```

2. Promote to admin directly in the database:
```bash
sqlcmd -S "(localdb)\mssqllocaldb" -d BookStoreApi -Q "UPDATE Users SET Role='admin' WHERE Email='admin@bookstore.com'"
```

3. Log in again to get a fresh token with the admin role:
```http
POST /api/auth/login
{ "email": "admin@bookstore.com", "password": "Admin123!" }
```

---

## How to Test the API

1. Open **http://localhost:5050** in your browser — Swagger UI loads.
2. Use `POST /api/auth/login` to get a token.
3. Click **Authorize** (top right), paste the token as `Bearer <token>`.
4. All protected endpoints are now available in the UI.

---

## API Endpoints

### Auth
| Method | URL | Auth | Description |
|--------|-----|------|-------------|
| POST | /api/auth/register | None | Register a new customer |
| POST | /api/auth/login | None | Login, get JWT |

### Books
| Method | URL | Auth | Description |
|--------|-----|------|-------------|
| GET | /api/books | None | List books (search, filter, paginate) |
| GET | /api/books/{id} | None | Get a single book |
| POST | /api/books | Admin | Create a book |
| PUT | /api/books/{id} | Admin | Update a book |
| DELETE | /api/books/{id} | Admin | Delete (409 if has orders) |

**Query params for GET /api/books:** `search`, `categoryId`, `authorId`, `minPrice`, `maxPrice`, `page`, `pageSize`

### Categories
| Method | URL | Auth | Description |
|--------|-----|------|-------------|
| GET | /api/categories | None | List all |
| GET | /api/categories/{id} | None | Get one |
| POST | /api/categories | Admin | Create |
| PUT | /api/categories/{id} | Admin | Update |
| DELETE | /api/categories/{id} | Admin | Delete |

### Authors
| Method | URL | Auth | Description |
|--------|-----|------|-------------|
| GET | /api/authors | None | List all |
| GET | /api/authors/{id} | None | Get one |
| POST | /api/authors | Admin | Create |
| PUT | /api/authors/{id} | Admin | Update |
| DELETE | /api/authors/{id} | Admin | Delete |

### Orders
| Method | URL | Auth | Description |
|--------|-----|------|-------------|
| POST | /api/orders | Customer | Place an order |
| GET | /api/orders/my | Customer | My orders |
| GET | /api/orders/all | Admin | All orders |
| GET | /api/orders/{id} | Customer/Admin | Single order (customer sees own only) |

---

## How to Apply Migrations Manually

```bash
dotnet ef database update
```

Add a new migration after changing the model:

```bash
dotnet ef migrations add <Name> --output-dir Migrations
```

---

## Task Coverage

| # | Requirement | Implementation |
|---|---|---|
| 1 | API endpoint design | REST routes across 5 controllers |
| 2 | JWT auth with register/login | AuthService + JwtBearer middleware |
| 3 | Customer / Admin roles | `[Authorize(Roles = "admin")]` on write endpoints |
| 4 | Books endpoints with browse/filter | BooksController + BookService with LINQ |
| 5 | Categories & Authors CRUD | CategoriesController, AuthorsController |
| 6 | Orders endpoints | OrdersController — place, my orders, admin all |
| 7 | Validation with clear errors | DataAnnotations on all DTOs, 400 responses |
| 8 | DTOs — no raw entities returned | All controllers return DTO objects only |
| 9 | Pagination, search, filter on books | `BookQueryDto` → `Skip/Take` + `Where` |
| 10 | Swagger with JWT support | Swashbuckle + Bearer security definition |
| 11 | CORS for frontend on localhost | `WithOrigins("http://localhost:3000", ...)` |
| 12 | Global error handling | `GlobalExceptionMiddleware` → clean JSON |
| 13 | Logging | ILogger in AuthService + OrderService |
| 14 | Thin controllers / DI services | All logic in Service layer, injected via DI |

## Status Codes Used

| Code | When |
|------|------|
| 200 | Success |
| 201 | Resource created |
| 204 | Deleted successfully |
| 400 | Validation error or bad input |
| 401 | Not authenticated |
| 403 | Authenticated but not authorized |
| 404 | Resource not found |
| 409 | Conflict (e.g. delete a book with existing orders) |
| 500 | Unexpected error (caught by global middleware, clean JSON returned) |
