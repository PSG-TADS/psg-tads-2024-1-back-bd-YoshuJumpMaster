using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using LocadoraDeVeiculos.Data;
using LocadoraDeVeiculos.Models;

namespace LocadoraDeVeiculos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly LocadoraContext _context;

        public AuthController(LocadoraContext context)
        {

            _context = context;
            
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _context.Clientes.SingleOrDefault(u => u.Nome == model.Username);

            if (user != null)
            {
                
                return Ok(new { Token = "Fake JWT Token" });

            }
            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var user = new Cliente
            {
                Nome = model.Username,
                CPF = model.CPF
            };

            _context.Clientes.Add(user);
            await _context.SaveChangesAsync();

            
            return Ok(new { Token = "Fake JWT Token" });
        }
    }
}
