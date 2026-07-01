using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStoreEF.Data;

public class BookStoreContextFactory : IDesignTimeDbContextFactory<BookStoreContext>
{
    public BookStoreContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<BookStoreContext>();
        opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookStoreEF;Trusted_Connection=True;");
        return new BookStoreContext(opts.Options);
    }
}
