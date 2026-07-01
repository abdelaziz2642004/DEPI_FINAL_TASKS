# BookStore Console Application

A .NET 8 console application for managing a bookstore. It implements object-oriented domain models, validation, and advanced C# concepts like polymorphism, generics, custom extension methods, delegates, and events.

---

## How to Run

Navigate to the `BookStore` directory and run the application:

```bash
cd BookStore
dotnet run
```

---

## Features

- **Manage Books:** Add, remove, search, and list all books.
- **Support Multiple Formats:** Works with Paperback, Ebooks, and Audiobooks seamlessly using polymorphism.
- **Register Customers:** Enforces business rules like unique emails.
- **Record Purchases:** Support multiple books in a single purchase with automatic stock reduction.
- **Out of Stock Notifications:** Fires a custom event when a book's stock drops to zero.
- **Apply Custom Rules:** Execute custom lambda operations on subsets of books (e.g. category discounts, inventory restocking).
- **Advanced Filtering:** Filter books by category, author, and price range.
- **Business Analytics:** Calculate total revenue, top-selling books, and top customer spent.

---

## Implementation Map

Here is how the application meets all core tasks:

| # | Task | Implementation Location / Files |
|---|---|---|
| **1** | Domain Model Design | Defined in [Models](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Models) (IEntity, Book, PaperbackBook, Ebook, Audiobook, Customer, Purchase, PurchaseItem) |
| **2** | Console Menu UI | Implemented in [ConsoleMenu.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/UI/ConsoleMenu.cs) |
| **3** | Customers & Multiple Book Purchases | Managed by [CustomerService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/CustomerService.cs) and [PurchaseService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/PurchaseService.cs) |
| **4** | Input Validation | Extensive input parsing inside [ConsoleMenu.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/UI/ConsoleMenu.cs) and [StringExtensions.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Extensions/StringExtensions.cs) |
| **5** | Business Analytics | Logic encapsulated in [AnalyticsService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/AnalyticsService.cs) |
| **6** | Advanced Filters | Implemented in [BookService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/BookService.cs) |
| **7** | Extensible Formats (Open/Closed) | Handled dynamically using derived types on the abstract [Book](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Models/Book.cs) class |
| **8** | Generic Repository | Implemented in [Repository.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Repositories/Repository.cs) |
| **9** | Custom Rules (Delegates) | Uses `Func<Book, bool>` and `Action<Book>` in [BookService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/BookService.cs) |
| **10** | Stock Notification (Events) | Event declared in [PurchaseService.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Services/PurchaseService.cs), carrying [StockEventArgs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Events/StockEventArgs.cs), subscribed to in [ConsoleMenu.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/UI/ConsoleMenu.cs) |
| **11** | Extension Methods | Created in [StringExtensions.cs](file:///c:/Users/abdel/OneDrive/Desktop/depitasks/Task-2/BookStore/Extensions/StringExtensions.cs) (e.g. `IsBlank()`, `ToDecimal()`, `Discounted()`, `ToPrice()`) |

---

## Console Interface Preview

### Main Menu
```
  ══════════════════════════════
   BookStore Manager
  ══════════════════════════════

  [1] Books
  [2] Customers
  [3] Purchases
  [4] Analytics
  [5] Filter Books
  [6] Apply Rule to Books
  [0] Exit

  > 
```

### Book List
```
  ══════════════════════════════
   All Books
  ══════════════════════════════

  [1] The Great Gatsby by F. Scott Fitzgerald | Fiction | Paperback | $10.99 | Stock: 5 | Pages: 180
  [2] 1984 by George Orwell | Dystopian | Ebook | $7.99 | Stock: 10 | File: EPUB
  [3] Becoming by Michelle Obama | Biography | Audiobook | $25.50 | Stock: 3 | Duration: 19.5h
  [4] To Kill a Mockingbird by Harper Lee | Fiction | Paperback | $12.99 | Stock: 0 | Pages: 281

  Press Enter to continue...
```

### Analytics Dashboard
```
  ══════════════════════════════
   Analytics
  ══════════════════════════════

  Total Revenue    : $44.48
  Best-Selling Book: The Great Gatsby
  Top Customer     : John Doe ($44.48)

  Press Enter to continue...
```
