using FinanceUi.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace FinanceUi.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) 
    {
    }
    
    public DbSet<Board> Boards { get; set; }
    public DbSet<Goal> Goals { get; set; }
    public DbSet<Income> Incomes { get; set; }
    public DbSet<Payment> Payments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Goal>()
            .HasOne(g => g.Board)
            .WithMany(b => b.Goals)
            .HasForeignKey(g => g.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Income>()
            .HasOne(i => i.Board)
            .WithMany(b => b.Incomes)
            .HasForeignKey(i => i.BoardId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Board)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BoardId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Goal)
            .WithMany(g => g.Payments)
            .HasForeignKey(p => p.GoalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}