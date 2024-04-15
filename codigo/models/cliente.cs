using System.ComponentModel.DataAnnotations;
namespace LocadoraDeVeiculos.Models
{

public class Cliente
{
    [Key]
    public int ClienteId { get; set; }
    public string Nome { get; set; }
    public string CPF { get; set; }
}
}
