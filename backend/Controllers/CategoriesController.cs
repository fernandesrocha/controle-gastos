using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public CategoriesController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/categories - Lista todas as categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories
            .Select(c => new CategoryDto 
            { 
                Id = c.Id, 
                Description = c.Description, 
                Purpose = c.Purpose 
            })
            .ToListAsync();

        return Ok(categories);
    }

    // POST: api/categories - Cria uma nova categoria
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto category)
    {
        // Validação extra (boa prática)
        if (string.IsNullOrWhiteSpace(category.Description))
        {
            return BadRequest("A descrição é obrigatória.");
        }

        var categoryEntity = new Category 
        { 
            Description = category.Description, 
            Purpose = category.Purpose 
        };

        _context.Categories.Add(categoryEntity);
        await _context.SaveChangesAsync();

        // Atualiza o DTO com o ID gerado
        category.Id = categoryEntity.Id;

        return CreatedAtAction(nameof(GetCategories), new { id = category.Id }, category);
    }
}