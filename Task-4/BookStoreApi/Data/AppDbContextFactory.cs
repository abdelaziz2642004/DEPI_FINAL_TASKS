using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace BookStoreApi.Data;

public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>();
        opts.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BookStoreApi;Trusted_Connection=True;");
        return new AppDbContext(opts.Options);
    }
}
