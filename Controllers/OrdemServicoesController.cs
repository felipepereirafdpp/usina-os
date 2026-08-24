using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsinaOS.Domain.Entities;

[Route("api/[controller]")]
[ApiController]
public class OrdemServicoesController : ControllerBase
{
    private readonly UsinaOSContext _context;
    public OrdemServicoesController(UsinaOSContext context)
    {
        _context = context;
    }

    // GET: api/OrdemServico
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OrdemServico>>> GetOrdemServico()
    {
        return await _context.OrdemServicos.ToListAsync();
    }

    // GET: api/OrdemServico/5
    [HttpGet("{id}")]
    public async Task<ActionResult<OrdemServico>> GetOrdemServico(System.Guid id)
    {
        var ordemservico = await _context.OrdemServicos.FindAsync(id);

        if (ordemservico == null)
        {
            return NotFound();
        }

        return ordemservico;
    }

    // PUT: api/OrdemServico/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOrdemServico(System.Guid? id, OrdemServico ordemservico)
    {
        if (id != ordemservico.Id)
        {
            return BadRequest();
        }

        _context.Entry(ordemservico).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OrdemServicoExists(id))
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

    // POST: api/OrdemServico
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<OrdemServico>> PostOrdemServico(OrdemServico ordemservico)
    {
        _context.OrdemServicos.Add(ordemservico);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetOrdemServico", new { id = ordemservico.Id }, ordemservico);
    }

    // DELETE: api/OrdemServico/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrdemServico(System.Guid? id)
    {
        var ordemservico = await _context.OrdemServicos.FindAsync(id);
        if (ordemservico == null)
        {
            return NotFound();
        }

        _context.OrdemServicos.Remove(ordemservico);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool OrdemServicoExists(System.Guid? id)
    {
        return _context.OrdemServicos.Any(e => e.Id == id);
    }
}
