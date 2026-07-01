using BookStoreEF.Models;
using Microsoft.EntityFrameworkCore;

namespace BookStoreEF.Data;

public class BookStoreContext : DbContext
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Purchase> Purchases => Set<Purchase>();
    public DbSet<PurchaseItem> PurchaseItems => Set<PurchaseItem>();

    public BookStoreContext(DbContextOptions<BookStoreContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Category>(e =>
        {
            e.ToTable("categories");
            e.HasKey(c => c.CategoryId);
            e.Property(c => c.CategoryId).HasColumnName("category_id");
            e.Property(c => c.CategoryName).HasColumnName("category_name").HasMaxLength(100).IsRequired();
        });

        mb.Entity<Author>(e =>
        {
            e.ToTable("authors");
            e.HasKey(a => a.AuthorId);
            e.Property(a => a.AuthorId).HasColumnName("author_id");
            e.Property(a => a.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
            e.Property(a => a.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
            e.Ignore(a => a.FullName);
        });

        mb.Entity<Book>(e =>
        {
            e.ToTable("books", t =>
            {
                t.HasCheckConstraint("price_must_be_positive", "price > 0");
                t.HasCheckConstraint("stock_cant_go_negative", "stock_quantity >= 0");
            });
            e.HasKey(b => b.BookId);
            e.Property(b => b.BookId).HasColumnName("book_id");
            e.Property(b => b.Title).HasColumnName("title").HasMaxLength(255).IsRequired();
            e.Property(b => b.Price).HasColumnName("price").HasColumnType("decimal(10,2)");
            e.Property(b => b.StockQuantity).HasColumnName("stock_quantity").HasDefaultValue(0);
            e.Property(b => b.CategoryId).HasColumnName("category_id");
            e.Property(b => b.AuthorId).HasColumnName("author_id");

            e.HasOne(b => b.Category)
             .WithMany(c => c.Books)
             .HasForeignKey(b => b.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        mb.Entity<Customer>(e =>
        {
            e.ToTable("customers");
            e.HasKey(c => c.CustomerId);
            e.Property(c => c.CustomerId).HasColumnName("customer_id");
            e.Property(c => c.FirstName).HasColumnName("first_name").HasMaxLength(50).IsRequired();
            e.Property(c => c.LastName).HasColumnName("last_name").HasMaxLength(50).IsRequired();
            e.Property(c => c.Email).HasColumnName("email").HasMaxLength(150).IsRequired();
            e.Property(c => c.City).HasColumnName("city").HasMaxLength(100);
            e.Property(c => c.CreatedAt).HasColumnName("created_at");
            e.HasIndex(c => c.Email).IsUnique();
        });

        mb.Entity<Purchase>(e =>
        {
            e.ToTable("purchases");
            e.HasKey(p => p.PurchaseId);
            e.Property(p => p.PurchaseId).HasColumnName("purchase_id");
            e.Property(p => p.PurchaseDate).HasColumnName("purchase_date");
            e.Property(p => p.CustomerId).HasColumnName("customer_id");

            e.HasOne(p => p.Customer)
             .WithMany(c => c.Purchases)
             .HasForeignKey(p => p.CustomerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        mb.Entity<PurchaseItem>(e =>
        {
            e.ToTable("purchase_items", t =>
            {
                t.HasCheckConstraint("qty_positive", "quantity > 0");
                t.HasCheckConstraint("item_price_positive", "unit_price > 0");
            });
            e.HasKey(pi => pi.ItemId);
            e.Property(pi => pi.ItemId).HasColumnName("item_id");
            e.Property(pi => pi.Quantity).HasColumnName("quantity").HasDefaultValue(1);
            e.Property(pi => pi.UnitPrice).HasColumnName("unit_price").HasColumnType("decimal(10,2)");
            e.Property(pi => pi.PurchaseId).HasColumnName("purchase_id");
            e.Property(pi => pi.BookId).HasColumnName("book_id");

            e.HasOne(pi => pi.Purchase)
             .WithMany(p => p.Items)
             .HasForeignKey(pi => pi.PurchaseId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(pi => pi.Book)
             .WithMany(b => b.PurchaseItems)
             .HasForeignKey(pi => pi.BookId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
