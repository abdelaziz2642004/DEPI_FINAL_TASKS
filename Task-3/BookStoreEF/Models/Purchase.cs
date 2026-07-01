namespace BookStoreEF.Models;

public class Purchase
{
    public int PurchaseId { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.Now;

    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    public ICollection<PurchaseItem> Items { get; set; } = new List<PurchaseItem>();
}
