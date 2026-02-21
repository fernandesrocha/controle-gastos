public class TransactionDto
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Value { get; set; }
    public TransactionType Type { get; set; }
    public int CategoryId { get; set; }
    public int PersonId { get; set; }
}