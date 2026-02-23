using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public CategoriesController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/categories - Lista todas as categorias.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
    {
        var categories = await _context.Categories.Select(c => new CategoryDto { Id = c.Id, Description = c.Description, Purpose = c.Purpose }).ToListAsync();
        return Ok(categories);
    }

    // POST: api/categories - Cria uma nova categoria.
    [HttpPost]
    public async Task<ActionResult<CategoryDto>> PostCategory(CategoryDto dto)
    {
        var category = new Category { Description = dto.Description, Purpose = dto.Purpose };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        dto.Id = category.Id;
        return CreatedAtAction(nameof(GetCategories), dto); // Não precisa de GetById, pois só criação e listagem.
    }
}