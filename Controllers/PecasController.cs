using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsinaOS.Domain.Entities;

[Route("api/[controller]")]
[ApiController]
public class PecasController : ControllerBase
{
    private readonly UsinaOSContext _context;
    public PecasController(UsinaOSContext context)
    {
        _context = context;
    }

    // GET: api/Peca
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Peca>>> GetPeca()
    {
        return await _context.Pecas.ToListAsync();
    }

    // GET: api/Peca/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Peca>> GetPeca(System.Guid id)
    {
        var peca = await _context.Pecas.FindAsync(id);

        if (peca == null)
        {
            return NotFound();
        }

        return peca;
    }

    // PUT: api/Peca/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPeca(System.Guid? id, Peca peca)
    {
        if (id != peca.Id)
        {
            return BadRequest();
        }

        _context.Entry(peca).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PecaExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Peca
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Peca>> PostPeca(Peca peca)
    {
        _context.Pecas.Add(peca);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPeca", new { id = peca.Id }, peca);
    }

    // DELETE: api/Peca/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePeca(System.Guid? id)
    {
        var peca = await _context.Pecas.FindAsync(id);
        if (peca == null)
        {
            return NotFound();
        }

        _context.Pecas.Remove(peca);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PecaExists(System.Guid? id)
    {
        return _context.Pecas.Any(e => e.Id == id);
    }
}
