using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace LocadoraDeVeiculos.Models
{
public class Pagamento
{
    [Key]
    public int PagamentoId { get; set; }

    [ForeignKey("Reserva")]
    public int ReservaId { get; set; }
    public Reserva Reserva { get; set; }
    
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }
}
}