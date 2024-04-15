using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LocadoraDeVeiculos.Models
{
public class Reserva
{
    [Key]
    public int ReservaId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }

    [ForeignKey("Cliente")]
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; }

    [ForeignKey("Veiculo")]
    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; }

    public decimal ValorTotal { get; set; }
}
}
