# BookStore Data Layer — EF Core

A .NET 9 console application that moves the bookstore from raw SQL to Entity Framework Core with SQL Server. Code-First migrations, LINQ queries, and data seeding are all included.

---

## Requirements

- .NET 9 SDK
- SQL Server or SQL Server LocalDB (comes with Visual Studio)
- `dotnet-ef` global tool

```bash
dotnet tool install --global dotnet-ef
```

---

## How to Run

```bash
cd BookStoreEF
dotnet run
```

On first run the app will:
1. Apply all pending migrations (creates the database automatically).
2. Seed the database with sample data if it is empty.
3. Run all queries and print results to the console.

---

## How to Apply Migrations Manually

```bash
cd BookStoreEF
dotnet ef database update
```

To create a new migration after changing the model:

```bash
dotnet ef migrations add <MigrationName> --output-dir Migrations
```

To roll back to a previous migration:

```bash
dotnet ef database update <MigrationName>
```

---

## Project Structure

```
BookStoreEF/
├── Models/
│   ├── Author.cs
│   ├── Book.cs
│   ├── Category.cs
│   ├── Customer.cs
│   ├── Purchase.cs
│   └── PurchaseItem.cs
├── Data/
│   ├── BookStoreContext.cs       ← DbContext + Fluent API config
│   ├── BookStoreContextFactory.cs← Design-time factory for dotnet-ef
│   └── Seeder.cs                 ← First-run seed data
├── Queries/
│   └── BookStoreQueries.cs       ← All 12 query tasks
├── Migrations/
│   ├── 20260630170156_InitialCreate.cs
│   └── ...
└── Program.cs                    ← Entry point
```

---

## Task Coverage

| # | Task | Where |
|---|---|---|
| **1** | EF Core setup, DbContext, connection string | [BookStoreContext.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-3/BookStoreEF/Data/BookStoreContext.cs), [Program.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-3/BookStoreEF/Program.cs) |
| **2** | Code-First migration with all constraints | [Migrations/](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-3/BookStoreEF/Migrations) |
| **3** | Seed data on first run | [Seeder.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-3/BookStoreEF/Data/Seeder.cs) |
| **4** | All books with category + author in one query | `ListAllBooks()` — uses `Include()` |
| **5** | Top 5 best-selling books | `Top5BestSellers()` — groups by BookId, sums quantity |
| **6** | Every customer with purchase count | `CustomersWithPurchaseCount()` — projects `c.Purchases.Count` |
| **7** | Categories with more than 5 books | `CategoriesWithMoreThan5Books()` |
| **8** | Books above average price | `BooksAboveAveragePrice()` — subquery via `Average()` |
| **9** | Customers who never purchased | `CustomersWithNoPurchases()` — `!c.Purchases.Any()` |
| **10** | Revenue grouped by month | `RevenueByMonth()` — groups by Year + Month |
| **11** | Keyword title search (case-insensitive) | `SearchBooks()` — `Contains()` translates to SQL LIKE |
| **12** | Paginated book list | `GetBooksPaged()` — `Skip().Take()` |
| **13** | Add, update, delete a book | `AddUpdateDelete()` |
| **14** | N+1 problem fixed | All list queries use `Include()` or projections — no lazy load |
| **15** | Read-only queries skip tracking | `AsNoTracking()` on every read query |

---

## Database Schema

```
categories ──< books >── authors
                │
                └──< purchase_items >── purchases >── customers
```

### Constraints (enforced at DB level via migration)

| Table | Constraint |
|---|---|
| books | `price > 0` CHECK |
| books | `stock_quantity >= 0` CHECK |
| books | FK to categories with `RESTRICT` on delete |
| books | FK to authors with `RESTRICT` on delete |
| customers | `email` UNIQUE index |
| purchase_items | `quantity > 0` CHECK |
| purchase_items | `unit_price > 0` CHECK |
| purchase_items | `unit_price` stored at purchase time (preserves historical price) |
