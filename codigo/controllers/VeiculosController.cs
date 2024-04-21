using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LocadoraDeVeiculos.Models; 

[ApiController]
[Route("[controller]")]
public class VeiculosController : ControllerBase
{
    private readonly LocadoraContext _context;

    public VeiculosController(LocadoraContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Veiculo>>> GetAll()
    {
        return await _context.Veiculos.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Veiculo>> GetById(int id)
    {
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
        {
            return NotFound();
        }
        return veiculo;
    }

    [HttpPost]
    public async Task<ActionResult<Veiculo>> Create(Veiculo veiculo)
    {
        _context.Veiculos.Add(veiculo);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = veiculo.VeiculoId }, veiculo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Veiculo veiculo)
    {
        if (id != veiculo.VeiculoId)
        {
            return BadRequest();
        }
        _context.Entry(veiculo).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Veiculos.Any(e => e.VeiculoId == id))
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
        var veiculo = await _context.Veiculos.FindAsync(id);
        if (veiculo == null)
        {
            return NotFound();
        }
        _context.Veiculos.Remove(veiculo);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
