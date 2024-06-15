public class Reserva
{
    public int ReservaId { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public decimal ValorTotal { get; set; }
    
    public int ClienteId { get; set; }
    public Cliente Cliente { get; set; } = null!;

    public int VeiculoId { get; set; }
    public Veiculo Veiculo { get; set; } = null!;
}
