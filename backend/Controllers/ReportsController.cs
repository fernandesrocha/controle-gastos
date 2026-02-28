using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public ReportsController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/reports/person-totals - Totais por pessoa + gerais
    [HttpGet("person-totals")]
    public async Task<ActionResult<object>> GetPersonTotals() // Mudado para object
    {
        try
        {
            var totalTransactions = await _context.Transactions.CountAsync();
            Console.WriteLine($"Total de transações no banco: {totalTransactions}");

            var persons = await _context.Persons
                .Include(p => p.Transactions)
                .ToListAsync();

            var loadedTransactions = persons.Sum(p => p.Transactions.Count);
            Console.WriteLine($"Transações carregadas via Include: {loadedTransactions}");

            var personTotals = persons.Select(p => new PersonTotalsDto
            {
                Person = new PersonDto { Id = p.Id, Name = p.Name, Age = p.Age },
                TotalIncome = p.Transactions.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Value),
                TotalExpense = p.Transactions.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Value),
                Balance = p.Transactions.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Value) -
                          p.Transactions.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Value)
            }).ToList();

            Console.WriteLine($"PersonTotals calculados: {personTotals.Count} itens");

            var general = new GeneralTotalsDto
            {
                TotalIncome = personTotals.Sum(pt => pt.TotalIncome),
                TotalExpense = personTotals.Sum(pt => pt.TotalExpense),
                Balance = personTotals.Sum(pt => pt.Balance)
            };

            return Ok(new { items = personTotals, general = general }); // AJUSTADO: Object com 'items' e 'general'
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no GetPersonTotals: {ex.Message}");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }

    // GET: api/reports/category-totals - Totais por categoria + gerais
    [HttpGet("category-totals")]
    public async Task<ActionResult<object>> GetCategoryTotals() // Mudado para object
    {
        try
        {
            var totalTransactions = await _context.Transactions.CountAsync();
            Console.WriteLine($"Total de transações no banco: {totalTransactions}");

            var categories = await _context.Categories
                .Include(c => c.Transactions)
                .ToListAsync();

            var loadedTransactions = categories.Sum(c => c.Transactions.Count);
            Console.WriteLine($"Transações carregadas via Include: {loadedTransactions}");

            var categoryTotals = categories.Select(c => new CategoryTotalsDto
            {
                Category = new CategoryDto { Id = c.Id, Description = c.Description, Purpose = c.Purpose },
                TotalIncome = c.Transactions.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Value),
                TotalExpense = c.Transactions.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Value),
                Balance = c.Transactions.Where(t => t.Type == TransactionType.Receita).Sum(t => t.Value) -
                          c.Transactions.Where(t => t.Type == TransactionType.Despesa).Sum(t => t.Value)
            }).ToList();

            Console.WriteLine($"CategoryTotals calculados: {categoryTotals.Count} itens");

            var general = new GeneralTotalsDto
            {
                TotalIncome = categoryTotals.Sum(ct => ct.TotalIncome),
                TotalExpense = categoryTotals.Sum(ct => ct.TotalExpense),
                Balance = categoryTotals.Sum(ct => ct.Balance)
            };

            return Ok(new { items = categoryTotals, general = general }); // AJUSTADO: Object com 'items' e 'general'
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro no GetCategoryTotals: {ex.Message}");
            return StatusCode(500, "Erro interno no servidor.");
        }
    }
}