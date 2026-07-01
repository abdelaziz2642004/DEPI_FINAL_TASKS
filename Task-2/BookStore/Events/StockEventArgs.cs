namespace BookStore.Events;

public class StockEventArgs : EventArgs
{
    public int BookId { get; }
    public string Title { get; }

    public StockEventArgs(int bookId, string title)
    {
        BookId = bookId;
        Title = title;
    }
}
