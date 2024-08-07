using DataAccess.Entities;
using DataAccess.Helper;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Contexts;

public class AppDb : DbContext
{

    string connectionString;//= $"Data Source=192.168.1.5,1433\\MSSQLSERVER01; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
    public AppDb()
    {
        //connectionString = $"Data Source=192.168.1.103,1433\\MSSQLSERVER01; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
        connectionString = $"Data Source={Constants.IP},{Constants.Port}\\MSSQLSERVER01; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    }
    public DbSet<Product_Amount> Product_Amount { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Vendor> Vendor { get; set; }
    public DbSet<Companys> Companys { get; set; }
    public DbSet<Product_units> Product_units { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Stores> Stores { get; set; }
    public DbSet<InventoryHistory> InventoryHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
             .Entity<InventoryHistory>(entity =>
             {
                 entity.ToTable("store_stock_update_time");
                 entity.HasKey(p => p.Ssut_id);
                 entity.Property(x => x.Ssut_id).ValueGeneratedOnAdd();
             });

        modelBuilder
            .Entity<Stores>()
            .HasKey(x => x.Store_id);

        modelBuilder
            .Entity<Employee>()
            .HasKey(x => x.emp_id);

        modelBuilder
            .Entity<Product_Amount>(entity =>
            {
                entity.HasKey(p => new { p.product_id, p.counter_id, p.store_id });
                entity.Property(x => x.pa_id).ValueGeneratedOnAdd();
                entity.Property(a => a.update_date).HasDefaultValueSql("(getutcdate())");
            });

        modelBuilder
            .Entity<Product>()
            .HasNoKey();

        modelBuilder
            .Entity<Vendor>()
            .HasNoKey();

        modelBuilder
            .Entity<Companys>()
            .HasNoKey();

        modelBuilder
            .Entity<Product_units>()
            .HasNoKey();
    }

}