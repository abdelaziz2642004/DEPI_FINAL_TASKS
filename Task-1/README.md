# BookStore SQL Database

A MySQL database for running an online bookstore — tracks books, authors, categories, customers, purchases, and what was bought in each purchase.

---

## How to Run

```bash
mysql -u root -p < bookstore.sql
```

Or open the file in MySQL Workbench / DBeaver and run it all at once.

---

## ERD

```
+------------+         +---------+         +-------+
| categories |         | authors |         | books |
+------------+         +---------+         +-------+
| category_id PK |<--+ | author_id PK |<-+ | book_id       PK |
| category_name  |   | | first_name   |  | | title            |
+----------------+   | | last_name    |  | | price            |
                     | +--------------+  | | stock_quantity   |
                     +--------------------+ | category_id  FK |
                                            | author_id    FK |
                                            +--------+--------+
                                                     |
+----------+        +-----------+        +-----------+---------+
| customers|        | purchases |        |    purchase_items   |
+----------+        +-----------+        +---------------------+
| customer_id PK|<-+| purchase_id PK|<-+| item_id      PK     |
| first_name    |  || customer_id FK|  || purchase_id  FK     |
| last_name     |  || purchase_date |  || book_id      FK     |
| email (UNIQUE)|  |+--------------+  || quantity            |
| city          |  |                  || unit_price          |
| created_at    |  +------------------++---------------------+
+---------------+
```

### Relationships

| From | To | Type |
|---|---|---|
| categories | books | One-to-Many |
| authors | books | One-to-Many |
| customers | purchases | One-to-Many |
| purchases | purchase_items | One-to-Many |
| books | purchase_items | One-to-Many |

---

## Tables

### `categories`
Book genres. Each book belongs to one category.

### `authors`
Author names split into first and last. Linked to books.

### `books`
Price and stock are both constrained — no zeros or negatives allowed. Price here is the current price; what a customer actually paid is stored separately in `purchase_items.unit_price`.

### `customers`
Email must be unique. City is optional but used for analytics.

### `purchases`
One row per checkout. Linked to one customer, timestamped with `purchase_date`.

### `purchase_items`
One row per book per purchase. `unit_price` is saved at the time of purchase so old records don't change if a book's price is updated later.

---

## Tasks

| # | Task |
|---|---|
| 1 | Schema design |
| 2 | Sample data — 30 books, 25 customers, 34 purchases, 67 purchase items |
| 3 | All books sorted by price (highest to lowest) |
| 4 | Book titles in UPPERCASE, author names in lowercase |
| 5 | Every book with its category and author |
| 6 | Every customer with their total purchase count |
| 7 | Top 5 best-selling books by units sold |
| 8 | City with the most customers |
| 9 | Categories with more than 5 books |
| 10 | Books priced above the average |
| 11 | Customers who have never made a purchase |
| 12 | Total revenue per month |
| 13 | View: `book_catalog` |
| 14 | Stored procedure: `get_customer_purchases(customer_id)` |

---

## Constraints

- `price > 0` — CHECK constraint on books
- `stock_quantity >= 0` — CHECK constraint on books
- `email` — UNIQUE per customer
- `unit_price` stored in `purchase_items` preserves the historical price
- Foreign keys keep old purchase records intact even if book data changes

---

## Procedure Usage

```sql
CALL get_customer_purchases(1);
```

Returns two result sets: a full itemized history of the customer's purchases, then a summary with total purchase count and grand total spent.
