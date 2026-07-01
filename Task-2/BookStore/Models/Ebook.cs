namespace BookStore.Models;

public class Ebook : Book
{
    public string FileFormat { get; set; } = "PDF";

    public override string Format => "Ebook";

    public override string GetDetails()
        => $"{base.ToString()} | File: {FileFormat}";
}
