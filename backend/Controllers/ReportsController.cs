using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public ReportsController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/reports/person-totals - Totais por pessoa + gerais.
    [HttpGet("person-totals")]
    public async Task<ActionResult<(List<PersonTotalsDto>, GeneralTotalsDto)>> GetPersonTotals()
    {
        // Agrupa transações por pessoa e calcula totais.
        var persons = await _context.Persons.Include(p => p.Transactions).ToListAsync();
        var personTotals = persons.Select(p => new PersonTotalsDto
        {
            Person = new PersonDto { Id = p.Id, Name = p.Name, Age = p.Age },
            TotalIncome = p.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Value),
            TotalExpense = p.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Value),
            Balance = p.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Value) -
                      p.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Value)
        }).ToList();

        // Totais gerais.
        var general = new GeneralTotalsDto
        {
            TotalIncome = personTotals.Sum(pt => pt.TotalIncome),
            TotalExpense = personTotals.Sum(pt => pt.TotalExpense),
            Balance = personTotals.Sum(pt => pt.Balance)
        };

        return Ok((personTotals, general));
    }

    // GET: api/reports/category-totals - Totais por categoria + gerais.
    [HttpGet("category-totals")]
    public async Task<ActionResult<(List<CategoryTotalsDto>, GeneralTotalsDto)>> GetCategoryTotals()
    {
        // Agrupa transações por categoria e calcula totais.
        var categories = await _context.Categories.Include(c => c.Transactions).ToListAsync();
        var categoryTotals = categories.Select(c => new CategoryTotalsDto
        {
            Category = new CategoryDto { Id = c.Id, Description = c.Description, Purpose = c.Purpose },
            TotalIncome = c.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Value),
            TotalExpense = c.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Value),
            Balance = c.Transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Value) -
                      c.Transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Value)
        }).ToList();

        // Totais gerais (mesmos que por pessoa, mas recalculados para consistência).
        var general = new GeneralTotalsDto
        {
            TotalIncome = categoryTotals.Sum(ct => ct.TotalIncome),
            TotalExpense = categoryTotals.Sum(ct => ct.TotalExpense),
            Balance = categoryTotals.Sum(ct => ct.Balance)
        };

        return Ok((categoryTotals, general));
    }
}