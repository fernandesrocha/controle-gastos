// para relatório
public class CategoryTotalsDto
{
    public CategoryDto Category { get; set; } = null!;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}