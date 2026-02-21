// para relatório
public class PersonTotalsDto
{
    public PersonDto Person { get; set; } = null!;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}