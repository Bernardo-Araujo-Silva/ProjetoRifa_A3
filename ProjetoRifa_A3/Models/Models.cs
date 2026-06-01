namespace ProjetoRifa.Models;

public class Raffle
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public List<Ticket> Tickets { get; set; } = [];
}

public class Ticket
{
    public int Id { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public int Number { get; set; }
    public int RaffleId { get; set; }
    public Raffle? Raffle { get; set; }
}
