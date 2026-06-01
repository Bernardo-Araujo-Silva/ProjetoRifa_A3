using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoRifa.Data;
using ProjetoRifa.Models;

namespace ProjetoRifa.Controllers;

[ApiController]
[Route("api/raffles")]
public class RafflesController(AppDbContext db) : ControllerBase
{
    // POST /api/raffles
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateRaffleDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title))
            return BadRequest(new { erro = "O titulo da rifa e obrigatorio." });

        var raffle = new Raffle { Title = dto.Title };
        db.Raffles.Add(raffle);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = raffle.Id }, new
        {
            mensagem = "Rifa criada com sucesso!",
            rifa = new
            {
                id = raffle.Id,
                titulo = raffle.Title,
                criadaEm = raffle.CreatedAt,
                totalBilhetes = 0
            }
        });
    }

    // GET /api/raffles/{id}
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var raffle = await db.Raffles
            .Include(r => r.Tickets)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (raffle is null)
            return NotFound(new { erro = $"Nenhuma rifa encontrada com o id {id}." });

        return Ok(new
        {
            mensagem = "Rifa encontrada.",
            rifa = new
            {
                id = raffle.Id,
                titulo = raffle.Title,
                criadaEm = raffle.CreatedAt,
                totalBilhetes = raffle.Tickets.Count,
                bilhetes = raffle.Tickets.OrderBy(t => t.Number).Select(t => new
                {
                    id = t.Id,
                    numero = t.Number,
                    comprador = t.BuyerName
                })
            }
        });
    }

    // POST /api/raffles/{id}/tickets
    [HttpPost("{id}/tickets")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddTicket(int id, [FromBody] CreateTicketDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.BuyerName))
            return BadRequest(new { erro = "O nome do comprador e obrigatorio." });

        var raffle = await db.Raffles.FindAsync(id);
        if (raffle is null)
            return NotFound(new { erro = $"Nenhuma rifa encontrada com o id {id}." });

        bool exists = await db.Tickets.AnyAsync(t => t.RaffleId == id && t.Number == dto.Number);
        if (exists)
            return Conflict(new { erro = $"O numero {dto.Number} ja esta registrado nesta rifa. Escolha outro numero." });

        var ticket = new Ticket
        {
            BuyerName = dto.BuyerName,
            Number = dto.Number,
            RaffleId = id
        };
        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id }, new
        {
            mensagem = "Bilhete registrado com sucesso!",
            bilhete = new
            {
                id = ticket.Id,
                numero = ticket.Number,
                comprador = ticket.BuyerName,
                rifaId = ticket.RaffleId
            }
        });
    }

    // POST /api/raffles/{id}/draw
    [HttpPost("{id}/draw")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Draw(int id)
    {
        var raffle = await db.Raffles
            .Include(r => r.Tickets)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (raffle is null)
            return NotFound(new { erro = $"Nenhuma rifa encontrada com o id {id}." });

        if (raffle.Tickets.Count == 0)
            return BadRequest(new { erro = "Esta rifa nao possui bilhetes cadastrados. Adicione bilhetes antes de sortear." });

        var winner = raffle.Tickets[Random.Shared.Next(raffle.Tickets.Count)];

        return Ok(new
        {
            mensagem = "Sorteio realizado com sucesso!",
            sorteio = new
            {
                rifa = raffle.Title,
                totalParticipantes = raffle.Tickets.Count,
                vencedor = new
                {
                    id = winner.Id,
                    numero = winner.Number,
                    comprador = winner.BuyerName
                }
            }
        });
    }

    // GET /api/raffles/{id}/report
    [HttpGet("{id}/report")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Report(int id)
    {
        var raffle = await db.Raffles
            .Include(r => r.Tickets)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (raffle is null)
            return NotFound(new { erro = $"Nenhuma rifa encontrada com o id {id}." });

        return Ok(new
        {
            mensagem = "Relatorio gerado com sucesso!",
            relatorio = new
            {
                rifaId = raffle.Id,
                titulo = raffle.Title,
                criadaEm = raffle.CreatedAt,
                totalBilhetes = raffle.Tickets.Count,
                bilhetes = raffle.Tickets
                    .OrderBy(t => t.Number)
                    .Select(t => new
                    {
                        id = t.Id,
                        numero = t.Number,
                        comprador = t.BuyerName
                    })
            }
        });
    }

    // DELETE /api/raffles/{id}
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        var raffle = await db.Raffles.FindAsync(id);
        if (raffle is null)
            return NotFound(new { erro = $"Nenhuma rifa encontrada com o id {id}." });

        db.Raffles.Remove(raffle);
        await db.SaveChangesAsync();

        return Ok(new
        {
            mensagem = $"A rifa '{raffle.Title}' foi encerrada e removida com sucesso.",
            rifaId = id
        });
    }
}
