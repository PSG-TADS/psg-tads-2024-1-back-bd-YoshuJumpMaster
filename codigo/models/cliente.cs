public class Cliente
{
    public int ClienteId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
