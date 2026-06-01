using Microsoft.EntityFrameworkCore;
using ProjetoRifa.Models;

namespace ProjetoRifa.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Raffle> Raffles => Set<Raffle>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Raffle>()
            .Property(r => r.Title)
            .IsRequired();

        modelBuilder.Entity<Ticket>()
            .Property(t => t.BuyerName)
            .IsRequired();

        modelBuilder.Entity<Ticket>()
            .HasIndex(t => new { t.RaffleId, t.Number })
            .IsUnique();
    }
}
