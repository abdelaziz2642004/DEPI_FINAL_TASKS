namespace BookStoreEF.Models;

public class PurchaseItem
{
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public int PurchaseId { get; set; }
    public Purchase Purchase { get; set; } = null!;

    public int BookId { get; set; }
    public Book Book { get; set; } = null!;
}
