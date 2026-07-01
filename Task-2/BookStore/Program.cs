using BookStore.Models;
using BookStore.Repositories;
using BookStore.Services;
using BookStore.UI;

var bookRepo = new Repository<Book>();
var customerRepo = new Repository<Customer>();
var purchaseRepo = new Repository<Purchase>();

try
{
    bookRepo.Add(new PaperbackBook { Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Category = "Fiction", Price = 10.99m, Stock = 5, PageCount = 180 });
    bookRepo.Add(new Ebook { Title = "1984", Author = "George Orwell", Category = "Dystopian", Price = 7.99m, Stock = 10, FileFormat = "EPUB" });
    bookRepo.Add(new Audiobook { Title = "Becoming", Author = "Michelle Obama", Category = "Biography", Price = 25.50m, Stock = 3, DurationHours = 19.5 });
    bookRepo.Add(new PaperbackBook { Title = "To Kill a Mockingbird", Author = "Harper Lee", Category = "Fiction", Price = 12.99m, Stock = 0, PageCount = 281 });

    customerRepo.Add(new Customer { FirstName = "John", LastName = "Doe", Email = "john.doe@gmail.com" });
    customerRepo.Add(new Customer { FirstName = "Jane", LastName = "Smith", Email = "jane.smith@gmail.com" });

    var bookService = new BookService(bookRepo);
    var customerService = new CustomerService(customerRepo);
    var purchaseService = new PurchaseService(purchaseRepo, bookRepo);
    var analyticsService = new AnalyticsService(purchaseRepo, bookRepo, customerRepo);

    var menu = new ConsoleMenu(bookService, customerService, purchaseService, analyticsService);
    await menu.RunAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Critical Error: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("Press any key to exit...");
    Console.ReadLine();
}
