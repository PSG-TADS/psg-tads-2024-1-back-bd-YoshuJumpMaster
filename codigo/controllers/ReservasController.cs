using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Models;

[ApiController]
[Route("[controller]")]
public class ReservasController : ControllerBase
{
    private readonly LocadoraContext _context;

    public ReservasController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Reserva>>> GetAll()
    {
        return await _context.Reservas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Reserva>> GetById(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }
        return reserva;
    }

    [HttpPost]
    public async Task<ActionResult<Reserva>> Create(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = reserva.ReservaId }, reserva);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Reserva reserva)
    {
        if (id != reserva.ReservaId)
        {
            return BadRequest();
        }

        _context.Entry(reserva).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Reservas.Any(e => e.ReservaId == id))
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
