using BookStoreEF.Data;
using BookStoreEF.Models;

namespace BookStoreEF.Data;

public static class Seeder
{
    public static void Seed(BookStoreContext db)
    {
        if (db.Categories.Any())
            return;

        var categories = new[]
        {
            new Category { CategoryName = "Fiction" },
            new Category { CategoryName = "Science" },
            new Category { CategoryName = "History" },
            new Category { CategoryName = "Self-Help" },
            new Category { CategoryName = "Technology" },
            new Category { CategoryName = "Biography" },
            new Category { CategoryName = "Philosophy" },
            new Category { CategoryName = "Children" },
            new Category { CategoryName = "Mystery" },
            new Category { CategoryName = "Economics" }
        };
        db.Categories.AddRange(categories);
        db.SaveChanges();

        var authors = new[]
        {
            new Author { FirstName = "George", LastName = "Orwell" },
            new Author { FirstName = "Yuval", LastName = "Harari" },
            new Author { FirstName = "James", LastName = "Clear" },
            new Author { FirstName = "Stephen", LastName = "King" },
            new Author { FirstName = "Malcolm", LastName = "Gladwell" },
            new Author { FirstName = "Agatha", LastName = "Christie" },
            new Author { FirstName = "Walter", LastName = "Isaacson" },
            new Author { FirstName = "Nassim", LastName = "Taleb" },
            new Author { FirstName = "J.K.", LastName = "Rowling" },
            new Author { FirstName = "Michelle", LastName = "Obama" },
            new Author { FirstName = "Fyodor", LastName = "Dostoevsky" },
            new Author { FirstName = "Cal", LastName = "Newport" },
            new Author { FirstName = "Daniel", LastName = "Kahneman" },
            new Author { FirstName = "Roald", LastName = "Dahl" },
            new Author { FirstName = "Frank", LastName = "Herbert" }
        };
        db.Authors.AddRange(authors);
        db.SaveChanges();

        var fiction = categories[0];
        var science = categories[1];
        var selfHelp = categories[3];
        var biography = categories[5];
        var philosophy = categories[6];
        var children = categories[7];
        var mystery = categories[8];
        var economics = categories[9];

        var orwell = authors[0];
        var harari = authors[1];
        var clear = authors[2];
        var king = authors[3];
        var gladwell = authors[4];
        var christie = authors[5];
        var isaacson = authors[6];
        var taleb = authors[7];
        var rowling = authors[8];
        var obama = authors[9];
        var dostoevsky = authors[10];
        var newport = authors[11];
        var kahneman = authors[12];
        var dahl = authors[13];
        var herbert = authors[14];

        var books = new[]
        {
            new Book { Title = "1984", Price = 12.99m, StockQuantity = 45, Category = fiction, Author = orwell },
            new Book { Title = "Animal Farm", Price = 9.49m, StockQuantity = 60, Category = fiction, Author = orwell },
            new Book { Title = "Sapiens", Price = 17.50m, StockQuantity = 30, Category = science, Author = harari },
            new Book { Title = "Homo Deus", Price = 15.99m, StockQuantity = 25, Category = science, Author = harari },
            new Book { Title = "Atomic Habits", Price = 21.00m, StockQuantity = 80, Category = selfHelp, Author = clear },
            new Book { Title = "The Shining", Price = 14.25m, StockQuantity = 20, Category = mystery, Author = king },
            new Book { Title = "It", Price = 18.75m, StockQuantity = 15, Category = mystery, Author = king },
            new Book { Title = "Outliers", Price = 13.50m, StockQuantity = 35, Category = selfHelp, Author = gladwell },
            new Book { Title = "The Tipping Point", Price = 11.99m, StockQuantity = 40, Category = selfHelp, Author = gladwell },
            new Book { Title = "Murder on the Orient Express", Price = 10.00m, StockQuantity = 55, Category = mystery, Author = christie },
            new Book { Title = "And Then There Were None", Price = 9.99m, StockQuantity = 50, Category = mystery, Author = christie },
            new Book { Title = "Steve Jobs", Price = 19.99m, StockQuantity = 22, Category = biography, Author = isaacson },
            new Book { Title = "Leonardo da Vinci", Price = 22.50m, StockQuantity = 18, Category = biography, Author = isaacson },
            new Book { Title = "The Black Swan", Price = 16.00m, StockQuantity = 28, Category = economics, Author = taleb },
            new Book { Title = "Antifragile", Price = 18.00m, StockQuantity = 20, Category = economics, Author = taleb },
            new Book { Title = "Harry Potter and the Sorcerers Stone", Price = 14.99m, StockQuantity = 100, Category = fiction, Author = rowling },
            new Book { Title = "Harry Potter and the Chamber of Secrets", Price = 13.99m, StockQuantity = 90, Category = fiction, Author = rowling },
            new Book { Title = "Becoming", Price = 20.00m, StockQuantity = 40, Category = biography, Author = obama },
            new Book { Title = "Crime and Punishment", Price = 11.50m, StockQuantity = 30, Category = philosophy, Author = dostoevsky },
            new Book { Title = "The Brothers Karamazov", Price = 13.00m, StockQuantity = 22, Category = philosophy, Author = dostoevsky },
            new Book { Title = "Deep Work", Price = 16.50m, StockQuantity = 38, Category = selfHelp, Author = newport },
            new Book { Title = "Digital Minimalism", Price = 14.00m, StockQuantity = 33, Category = selfHelp, Author = newport },
            new Book { Title = "Thinking Fast and Slow", Price = 17.99m, StockQuantity = 27, Category = science, Author = kahneman },
            new Book { Title = "Charlie and the Chocolate Factory", Price = 8.99m, StockQuantity = 75, Category = children, Author = dahl },
            new Book { Title = "The BFG", Price = 7.50m, StockQuantity = 65, Category = children, Author = dahl },
            new Book { Title = "Dune", Price = 19.00m, StockQuantity = 42, Category = fiction, Author = herbert },
            new Book { Title = "Dune Messiah", Price = 17.00m, StockQuantity = 30, Category = fiction, Author = herbert },
            new Book { Title = "A Brief History of Time", Price = 15.00m, StockQuantity = 35, Category = science, Author = harari },
            new Book { Title = "The Alchemist", Price = 10.50m, StockQuantity = 55, Category = philosophy, Author = dostoevsky },
            new Book { Title = "Meditations", Price = 9.00m, StockQuantity = 48, Category = philosophy, Author = dostoevsky }
        };
        db.Books.AddRange(books);
        db.SaveChanges();

        var customers = new[]
        {
            new Customer { FirstName = "Lena",    LastName = "Marchetti", Email = "lena.marchetti@gmail.com",   City = "New York",     CreatedAt = new DateOnly(2023, 1, 10) },
            new Customer { FirstName = "Omar",    LastName = "Fathi",     Email = "omar.fathi@outlook.com",     City = "London",       CreatedAt = new DateOnly(2023, 2, 14) },
            new Customer { FirstName = "Priya",   LastName = "Nair",      Email = "priya.nair@yahoo.com",       City = "Mumbai",       CreatedAt = new DateOnly(2023, 3, 5) },
            new Customer { FirstName = "Jake",    LastName = "Kowalski",  Email = "jake.kow@gmail.com",         City = "Chicago",      CreatedAt = new DateOnly(2023, 3, 20) },
            new Customer { FirstName = "Sofia",   LastName = "Bernal",    Email = "sofia.bernal@hotmail.com",   City = "Madrid",       CreatedAt = new DateOnly(2023, 4, 1) },
            new Customer { FirstName = "Chen",    LastName = "Wei",       Email = "chen.wei88@gmail.com",       City = "Shanghai",     CreatedAt = new DateOnly(2023, 4, 18) },
            new Customer { FirstName = "Amara",   LastName = "Diallo",    Email = "amara.diallo@mail.com",      City = "Paris",        CreatedAt = new DateOnly(2023, 5, 2) },
            new Customer { FirstName = "Lucas",   LastName = "Ferreira",  Email = "lucas.ferr@gmail.com",       City = "Sao Paulo",    CreatedAt = new DateOnly(2023, 5, 15) },
            new Customer { FirstName = "Hana",    LastName = "Suzuki",    Email = "hana.suzuki@jp.com",         City = "Tokyo",        CreatedAt = new DateOnly(2023, 6, 10) },
            new Customer { FirstName = "Ethan",   LastName = "Brooks",    Email = "ethan.brooks@gmail.com",     City = "New York",     CreatedAt = new DateOnly(2023, 6, 22) },
            new Customer { FirstName = "Nia",     LastName = "Owusu",     Email = "nia.owusu@outlook.com",      City = "Accra",        CreatedAt = new DateOnly(2023, 7, 8) },
            new Customer { FirstName = "Marco",   LastName = "Ricci",     Email = "marco.ricci@libero.it",      City = "Rome",         CreatedAt = new DateOnly(2023, 7, 19) },
            new Customer { FirstName = "Fatima",  LastName = "Al-Hassan", Email = "fatima.alh@gmail.com",       City = "Dubai",        CreatedAt = new DateOnly(2023, 8, 3) },
            new Customer { FirstName = "Yusuf",   LastName = "Demir",     Email = "yusuf.demir@hotmail.com",    City = "Istanbul",     CreatedAt = new DateOnly(2023, 8, 25) },
            new Customer { FirstName = "Clara",   LastName = "Dupont",    Email = "clara.dupont@laposte.net",   City = "Paris",        CreatedAt = new DateOnly(2023, 9, 12) },
            new Customer { FirstName = "Ravi",    LastName = "Sharma",    Email = "ravi.sharma@gmail.com",      City = "Mumbai",       CreatedAt = new DateOnly(2023, 9, 30) },
            new Customer { FirstName = "Ingrid",  LastName = "Larsen",    Email = "ingrid.l@outlook.no",        City = "Oslo",         CreatedAt = new DateOnly(2023, 10, 14) },
            new Customer { FirstName = "Carlos",  LastName = "Mendez",    Email = "c.mendez@gmail.com",         City = "Mexico City",  CreatedAt = new DateOnly(2023, 10, 28) },
            new Customer { FirstName = "Yuki",    LastName = "Tanaka",    Email = "yuki.tanaka@docomo.jp",      City = "Tokyo",        CreatedAt = new DateOnly(2023, 11, 5) },
            new Customer { FirstName = "Abby",    LastName = "Thompson",  Email = "abby.t@gmail.com",           City = "New York",     CreatedAt = new DateOnly(2023, 11, 20) },
            new Customer { FirstName = "Kwame",   LastName = "Asante",    Email = "k.asante@gmail.com",         City = "London",       CreatedAt = new DateOnly(2023, 12, 1) },
            new Customer { FirstName = "Miriam",  LastName = "Cohen",     Email = "miriam.c@walla.co.il",       City = "Tel Aviv",     CreatedAt = new DateOnly(2023, 12, 15) },
            new Customer { FirstName = "Diego",   LastName = "Vargas",    Email = "diego.v@gmail.com",          City = "Buenos Aires", CreatedAt = new DateOnly(2024, 1, 7) },
            new Customer { FirstName = "Aisha",   LastName = "Mansour",   Email = "aisha.m@outlook.com",        City = "Cairo",        CreatedAt = new DateOnly(2024, 1, 22) },
            new Customer { FirstName = "Nour",    LastName = "El-Amin",   Email = "nour.elamin@gmail.com",      City = "Chicago",      CreatedAt = new DateOnly(2024, 2, 10) }
        };
        db.Customers.AddRange(customers);
        db.SaveChanges();

        var b = books;
        var c = customers;

        var purchases = new List<Purchase>
        {
            new() { Customer = c[0],  PurchaseDate = new DateTime(2024,  1,  5, 10, 15, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 2, UnitPrice = 21.00m }, new() { Book = b[15], Quantity = 1, UnitPrice = 14.99m } } },
            new() { Customer = c[0],  PurchaseDate = new DateTime(2024,  3, 12, 14, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[2],  Quantity = 1, UnitPrice = 17.50m }, new() { Book = b[22], Quantity = 1, UnitPrice = 17.99m } } },
            new() { Customer = c[1],  PurchaseDate = new DateTime(2024,  1, 18,  9,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[9],  Quantity = 2, UnitPrice = 10.00m }, new() { Book = b[10], Quantity = 1, UnitPrice = 9.99m } } },
            new() { Customer = c[2],  PurchaseDate = new DateTime(2024,  2,  2, 11, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[0],  Quantity = 1, UnitPrice = 12.99m }, new() { Book = b[7],  Quantity = 2, UnitPrice = 13.50m } } },
            new() { Customer = c[2],  PurchaseDate = new DateTime(2024,  4, 20, 16,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 1, UnitPrice = 21.00m }, new() { Book = b[20], Quantity = 1, UnitPrice = 16.50m } } },
            new() { Customer = c[3],  PurchaseDate = new DateTime(2024,  2, 14, 13, 20, 0), Items = new List<PurchaseItem> { new() { Book = b[15], Quantity = 3, UnitPrice = 14.99m }, new() { Book = b[16], Quantity = 2, UnitPrice = 13.99m } } },
            new() { Customer = c[4],  PurchaseDate = new DateTime(2024,  3,  1,  8, 50, 0), Items = new List<PurchaseItem> { new() { Book = b[11], Quantity = 1, UnitPrice = 19.99m }, new() { Book = b[17], Quantity = 1, UnitPrice = 20.00m } } },
            new() { Customer = c[5],  PurchaseDate = new DateTime(2024,  3,  8, 17, 10, 0), Items = new List<PurchaseItem> { new() { Book = b[25], Quantity = 2, UnitPrice = 19.00m }, new() { Book = b[2],  Quantity = 1, UnitPrice = 17.50m } } },
            new() { Customer = c[5],  PurchaseDate = new DateTime(2024,  5, 22, 12,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 1, UnitPrice = 21.00m }, new() { Book = b[3],  Quantity = 1, UnitPrice = 15.99m } } },
            new() { Customer = c[6],  PurchaseDate = new DateTime(2024,  3, 25, 10, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[6],  Quantity = 1, UnitPrice = 18.75m }, new() { Book = b[5],  Quantity = 1, UnitPrice = 14.25m } } },
            new() { Customer = c[7],  PurchaseDate = new DateTime(2024,  4,  5, 15, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[22], Quantity = 1, UnitPrice = 17.99m }, new() { Book = b[13], Quantity = 1, UnitPrice = 16.00m } } },
            new() { Customer = c[8],  PurchaseDate = new DateTime(2024,  4, 11,  9, 20, 0), Items = new List<PurchaseItem> { new() { Book = b[23], Quantity = 2, UnitPrice = 8.99m  }, new() { Book = b[24], Quantity = 1, UnitPrice = 7.50m } } },
            new() { Customer = c[9],  PurchaseDate = new DateTime(2024,  4, 28, 11,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 2, UnitPrice = 21.00m }, new() { Book = b[21], Quantity = 1, UnitPrice = 14.00m } } },
            new() { Customer = c[9],  PurchaseDate = new DateTime(2024,  6,  3, 14, 15, 0), Items = new List<PurchaseItem> { new() { Book = b[14], Quantity = 1, UnitPrice = 18.00m }, new() { Book = b[25], Quantity = 1, UnitPrice = 19.00m } } },
            new() { Customer = c[10], PurchaseDate = new DateTime(2024,  5,  7, 10,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[7],  Quantity = 1, UnitPrice = 13.50m }, new() { Book = b[8],  Quantity = 2, UnitPrice = 11.99m } } },
            new() { Customer = c[11], PurchaseDate = new DateTime(2024,  5, 14, 16, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[0],  Quantity = 1, UnitPrice = 12.99m }, new() { Book = b[1],  Quantity = 2, UnitPrice = 9.49m } } },
            new() { Customer = c[12], PurchaseDate = new DateTime(2024,  5, 29,  9, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[12], Quantity = 1, UnitPrice = 22.50m }, new() { Book = b[6],  Quantity = 1, UnitPrice = 18.75m } } },
            new() { Customer = c[13], PurchaseDate = new DateTime(2024,  6, 10, 13,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[15], Quantity = 2, UnitPrice = 14.99m }, new() { Book = b[4],  Quantity = 1, UnitPrice = 21.00m } } },
            new() { Customer = c[14], PurchaseDate = new DateTime(2024,  6, 18, 11, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[26], Quantity = 1, UnitPrice = 17.00m }, new() { Book = b[19], Quantity = 1, UnitPrice = 13.00m } } },
            new() { Customer = c[15], PurchaseDate = new DateTime(2024,  7,  2, 10, 15, 0), Items = new List<PurchaseItem> { new() { Book = b[2],  Quantity = 2, UnitPrice = 17.50m }, new() { Book = b[27], Quantity = 1, UnitPrice = 15.00m } } },
            new() { Customer = c[16], PurchaseDate = new DateTime(2024,  7, 15, 14,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[20], Quantity = 1, UnitPrice = 16.50m }, new() { Book = b[21], Quantity = 1, UnitPrice = 14.00m } } },
            new() { Customer = c[17], PurchaseDate = new DateTime(2024,  7, 28,  9, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[18], Quantity = 1, UnitPrice = 11.50m }, new() { Book = b[28], Quantity = 1, UnitPrice = 10.50m } } },
            new() { Customer = c[18], PurchaseDate = new DateTime(2024,  8,  5, 12, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 3, UnitPrice = 21.00m }, new() { Book = b[15], Quantity = 1, UnitPrice = 14.99m } } },
            new() { Customer = c[19], PurchaseDate = new DateTime(2024,  8, 19, 15,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[11], Quantity = 1, UnitPrice = 19.99m }, new() { Book = b[17], Quantity = 1, UnitPrice = 20.00m } } },
            new() { Customer = c[20], PurchaseDate = new DateTime(2024,  9,  3, 10,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[9],  Quantity = 1, UnitPrice = 10.00m }, new() { Book = b[10], Quantity = 2, UnitPrice = 9.99m } } },
            new() { Customer = c[21], PurchaseDate = new DateTime(2024,  9, 17, 13, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[29], Quantity = 2, UnitPrice = 9.00m  }, new() { Book = b[18], Quantity = 1, UnitPrice = 11.50m } } },
            new() { Customer = c[22], PurchaseDate = new DateTime(2024, 10,  1, 11,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[12], Quantity = 1, UnitPrice = 22.50m }, new() { Book = b[3],  Quantity = 1, UnitPrice = 15.99m } } },
            new() { Customer = c[23], PurchaseDate = new DateTime(2024, 10, 15,  9, 15, 0), Items = new List<PurchaseItem> { new() { Book = b[15], Quantity = 1, UnitPrice = 14.99m }, new() { Book = b[4],  Quantity = 2, UnitPrice = 21.00m } } },
            new() { Customer = c[0],  PurchaseDate = new DateTime(2024, 10, 28, 14, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[2],  Quantity = 1, UnitPrice = 17.50m }, new() { Book = b[22], Quantity = 1, UnitPrice = 17.99m } } },
            new() { Customer = c[2],  PurchaseDate = new DateTime(2024, 11,  5, 10, 30, 0), Items = new List<PurchaseItem> { new() { Book = b[25], Quantity = 1, UnitPrice = 19.00m }, new() { Book = b[14], Quantity = 1, UnitPrice = 18.00m } } },
            new() { Customer = c[5],  PurchaseDate = new DateTime(2024, 11, 20, 16,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[4],  Quantity = 2, UnitPrice = 21.00m }, new() { Book = b[7],  Quantity = 1, UnitPrice = 13.50m } } },
            new() { Customer = c[9],  PurchaseDate = new DateTime(2024, 12,  2,  9,  0, 0), Items = new List<PurchaseItem> { new() { Book = b[0],  Quantity = 1, UnitPrice = 12.99m }, new() { Book = b[6],  Quantity = 1, UnitPrice = 18.75m } } },
            new() { Customer = c[1],  PurchaseDate = new DateTime(2024, 12, 15, 13, 45, 0), Items = new List<PurchaseItem> { new() { Book = b[11], Quantity = 1, UnitPrice = 19.99m }, new() { Book = b[13], Quantity = 1, UnitPrice = 16.00m } } },
            new() { Customer = c[3],  PurchaseDate = new DateTime(2024, 12, 20, 11, 15, 0), Items = new List<PurchaseItem> { new() { Book = b[15], Quantity = 2, UnitPrice = 14.99m }, new() { Book = b[16], Quantity = 1, UnitPrice = 13.99m } } }
        };

        db.Purchases.AddRange(purchases);
        db.SaveChanges();
    }
}
