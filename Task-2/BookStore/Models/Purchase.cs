namespace BookStore.Models;

public class Purchase : IEntity
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime Date { get; set; } = DateTime.Now;
    public List<PurchaseItem> Items { get; set; } = new();

    public decimal Total => Items.Sum(i => i.LineTotal);
}
