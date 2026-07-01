using BookStoreEF.Data;
using BookStoreEF.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContext<BookStoreContext>(opts =>
    opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookStoreEF;Trusted_Connection=True;"));

var provider = services.BuildServiceProvider();

using var scope = provider.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<BookStoreContext>();

Console.WriteLine("Applying migrations...");
db.Database.Migrate();

Console.WriteLine("Seeding data...");
Seeder.Seed(db);

BookStoreQueries.ListAllBooks(db);
BookStoreQueries.Top5BestSellers(db);
BookStoreQueries.CustomersWithPurchaseCount(db);
BookStoreQueries.CategoriesWithMoreThan5Books(db);
BookStoreQueries.BooksAboveAveragePrice(db);
BookStoreQueries.CustomersWithNoPurchases(db);
BookStoreQueries.RevenueByMonth(db);
BookStoreQueries.SearchBooks(db, "harry");
BookStoreQueries.GetBooksPaged(db, page: 1, pageSize: 5);
BookStoreQueries.AddUpdateDelete(db);
BookStoreQueries.NPlus1Fixed(db);
BookStoreQueries.ReadOnlyQueryDemo(db);

Console.WriteLine("\nDone.");
