using System.Text.Json.Serialization;

namespace ProjetoRifa.Models;

public class CreateRaffleDto
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;
}

public class CreateTicketDto
{
    [JsonPropertyName("buyerName")]
    public string BuyerName { get; set; } = string.Empty;

    [JsonPropertyName("number")]
    public int Number { get; set; }
}

public class TicketDto
{
    public int Id { get; set; }
    public string BuyerName { get; set; } = string.Empty;
    public int Number { get; set; }
}

public class RaffleReportDto
{
    public int RaffleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int TotalTickets { get; set; }
    public List<TicketDto> Tickets { get; set; } = [];
}

public class DrawResultDto
{
    public int RaffleId { get; set; }
    public string Title { get; set; } = string.Empty;
    public TicketDto Winner { get; set; } = new();
}
