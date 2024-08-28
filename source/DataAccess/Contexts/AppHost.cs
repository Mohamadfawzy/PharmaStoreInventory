using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Contexts;

public class AppHost: DbContext
{
    private readonly string connectionString;
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }

    public AppHost()
    {
        connectionString = $"Data Source=192.168.1.103,1433; Initial Catalog=mosoftphram;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    }

    public DbSet<UserAccount> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.CreateOn).HasDefaultValueSql("(getutcdate())");
        });
    }
}
