using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Models
{

public class LocadoraContext : DbContext
{
    public LocadoraContext(DbContextOptions<LocadoraContext> options) : base(options) { }

    public DbSet<Veiculo> Veiculos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Reserva> Reservas { get; set; }
    public DbSet<Pagamento> Pagamentos { get; set; }
}
}
