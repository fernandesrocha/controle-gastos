using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class PersonsController : ControllerBase
{
    private readonly ExpensesDbContext _context;

    public PersonsController(ExpensesDbContext context)
    {
        _context = context;
    }

    // GET: api/persons - Lista todas as pessoas.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPersons()
    {
        var persons = await _context.Persons.Select(p => new PersonDto { Id = p.Id, Name = p.Name, Age = p.Age }).ToListAsync();
        return Ok(persons);
    }

    // GET: api/persons/5 - Obtém uma pessoa por ID.
    [HttpGet("{id}")]
    public async Task<ActionResult<PersonDto>> GetPerson(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();
        return new PersonDto { Id = person.Id, Name = person.Name, Age = person.Age };
    }

    // POST: api/persons - Cria uma nova pessoa.
    [HttpPost]
    public async Task<ActionResult<PersonDto>> PostPerson(PersonDto dto)
    {
        var person = new Person { Name = dto.Name, Age = dto.Age };
        _context.Persons.Add(person);
        await _context.SaveChangesAsync();
        dto.Id = person.Id;
        return CreatedAtAction(nameof(GetPerson), new { id = person.Id }, dto);
    }

    // PUT: api/persons/5 - Edita uma pessoa.
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPerson(int id, PersonDto dto)
    {
        if (id != dto.Id) return BadRequest();
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();
        person.Name = dto.Name;
        person.Age = dto.Age;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: api/persons/5 - Deleta uma pessoa (cascata para transações).
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePerson(int id)
    {
        var person = await _context.Persons.FindAsync(id);
        if (person == null) return NotFound();
        _context.Persons.Remove(person);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}