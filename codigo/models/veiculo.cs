public class Veiculo
{
    public int VeiculoId { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public bool Status { get; set; }
    
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
