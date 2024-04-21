using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.Models
{
public class Veiculo
{
    [Key]
    public int VeiculoId { get; set; }
    public string? Modelo { get; set; }
    public string? Marca { get; set; }
    public string? Placa { get; set; }
    public string? Status { get; set; }
}
}