public class Discount
{
    public string Description { get; }
    public decimal Amount { get; }

    public Discount(string description, decimal amount)
    {
        Description = description;
        Amount = amount;
    }
}