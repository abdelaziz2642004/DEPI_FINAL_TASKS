namespace BookStore.Models;

public class PaperbackBook : Book
{
    public int PageCount { get; set; }

    public override string Format => "Paperback";

    public override string GetDetails()
        => $"{base.ToString()} | Pages: {PageCount}";
}
