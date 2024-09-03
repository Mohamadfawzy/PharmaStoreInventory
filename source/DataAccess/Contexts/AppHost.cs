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
        //connectionString = $"Data Source=localhost; Initial Catalog=mosoftphram;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
        connectionString = $"workstation id=msoftStock.mssql.somee.com;packet size=4096;user id=mfawzyH_SQLLogin_1;pwd=kd35ziwgqs;data source=msoftStock.mssql.somee.com;persist security info=False;initial catalog=msoftStock;TrustServerCertificate=True";
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
