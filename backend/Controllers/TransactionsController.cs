using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public TransactionsController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/transactions - Lista todas as transações.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions()
    {
        var transactions = await _context.Transactions.Select(t => new TransactionDto 
        { 
            Id = t.Id, 
            Description = t.Description, 
            Value = t.Value, 
            Type = t.Type, 
            CategoryId = t.CategoryId, 
            PersonId = t.PersonId 
        }).ToListAsync();
        return Ok(transactions);
    }

    // POST: api/transactions - Cria uma nova transação com validações.
    [HttpPost]
    public async Task<ActionResult<TransactionDto>> PostTransaction(TransactionDto dto)
    {
        // Valida pessoa existe e idade.
        var person = await _context.Persons.FindAsync(dto.PersonId);
        if (person == null) return BadRequest("Pessoa não encontrada.");
        if (person.Age < 18 && dto.Type == TransactionType.Income) return BadRequest("Menores de 18 anos só podem registrar despesas.");

        // Valida categoria existe e finalidade compatível.
        var category = await _context.Categories.FindAsync(dto.CategoryId);
        if (category == null) return BadRequest("Categoria não encontrada.");
        if (dto.Type == TransactionType.Expense && category.Purpose == Purpose.Income) return BadRequest("Categoria só para receitas.");
        if (dto.Type == TransactionType.Income && category.Purpose == Purpose.Expense) return BadRequest("Categoria só para despesas.");
        // 'Both' sempre permite.

        var transaction = new Transaction 
        { 
            Description = dto.Description, 
            Value = dto.Value, 
            Type = dto.Type, 
            CategoryId = dto.CategoryId, 
            PersonId = dto.PersonId 
        };
        _context.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        dto.Id = transaction.Id;
        return CreatedAtAction(nameof(GetTransactions), dto);
    }
}