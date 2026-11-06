using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsinaOS.Domain.Entities;

[Route("api/[controller]")]
[ApiController]
public class AuthsController : ControllerBase
{
    private readonly UsinaOSContext _context;
    public AuthsController(UsinaOSContext context)
    {
        _context = context;
    }

    
    // PUT: api/Auth/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAuth(System.Guid? id, AuthEntitie auth)
    {
        if (id != auth.Id)
        {
            return BadRequest();
        }

        _context.Entry(auth).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AuthExists(id))
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

    // POST: api/Auth
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<AuthEntitie>> PostAuth(AuthEntitie auth)
    {
        _context.Auth.Add(auth);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAuth", new { id = auth.Id }, auth);
    }


 

    private bool AuthExists(System.Guid? id)
    {
        return _context.Auth.Any(e => e.Id == id);
    }
}
