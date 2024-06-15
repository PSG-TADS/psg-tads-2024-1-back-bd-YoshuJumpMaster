public class Pagamento
{
    public int PagamentoId { get; set; }
    public decimal Valor { get; set; }
    public DateTime Data { get; set; }

    
    public int ReservaId { get; set; }
    public Reserva Reserva { get; set; } = null!;
}
