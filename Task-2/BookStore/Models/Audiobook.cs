namespace BookStore.Models;

public class Audiobook : Book
{
    public double DurationHours { get; set; }

    public override string Format => "Audiobook";

    public override string GetDetails()
        => $"{base.ToString()} | Duration: {DurationHours}h";
}
