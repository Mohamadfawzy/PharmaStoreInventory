using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
namespace DataAccess.Contexts;

public class AppDb : DbContext
{

    // private readonly string connectionString;
    //= $"Data Source=192.168.1.5,1433\\MSSQLSERVER01; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    //connectionString = "Server=192.168.1.103,1433;Database=stock;User Id=sa;Password=Ph@store;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True";
    //connectionString = $"Data Source=108.166.183.115,1433;Initial Catalog=modernsoft;Persist Security Info=False;User ID=maua;Password=ma321982; MultipleActiveResultSets=False;Encrypt=True;Trusted_Connection=false; TrustServerCertificate=true; ";

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //{
    //    optionsBuilder.UseSqlServer(connectionString, options =>
    //    {
    //        options.CommandTimeout(180);
    //    });
    //}

    //public AppDb()
    //{
    //    //if (!string.IsNullOrEmpty(Strings.IP))
    //    //{
    //    //    connectionString = $"Data Source={Strings.IP},{Strings.Port}; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    //    //}
    //    //else

    //    connectionString = $"Server=192.168.1.25,1433; Database=stock;User Id=sa;Password=Ph@store;Persist Security Info=True;Encrypt=True;TrustServerCertificate=True;";
    //    //connectionString = $"Data Source=192.168.1.103,1433; Initial Catalog=stock;User ID=admin;Password=admin; Trusted_Connection=false; TrustServerCertificate=true;";
    //}

    //public AppDb(string db)
    //{
    //    connectionString = db;
    //}

    public AppDb(DbContextOptions<AppDb> options) : base(options)
    {
    }

    public DbSet<Product_Amount> Product_Amount { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Vendor> Vendor { get; set; }
    public DbSet<Companys> Companys { get; set; }
    public DbSet<Product_units> Product_units { get; set; }
    public DbSet<Employee> Employee { get; set; }
    public DbSet<Stores> Stores { get; set; }
    public DbSet<InventoryHistory> InventoryHistory { get; set; }
    public DbSet<ProductAmountUpdate> ProductAmountUpdates { get; set; }
    public DbSet<ProductAmountChange> ProductAmountChanges { get; set; }

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
             .Entity<ProductAmountUpdate>(entity =>
             {
                 entity.ToTable("Product_amount_update");
                 entity.HasKey(p => p.Id);
                 entity.Property(x => x.Id).ValueGeneratedOnAdd();
                 entity.Property(a => a.Insert_date).HasDefaultValueSql("(getdate())");
             });
        
        modelBuilder
             .Entity<ProductAmountChange>(entity =>
             {
                 entity.ToTable("Product_amount_change");
                 entity.HasKey(p => p.Id);
                 entity.Property(x => x.Id).ValueGeneratedOnAdd();
                 entity.Property(a => a.Insert_date).HasDefaultValueSql("(getdate())");
             });

        modelBuilder
            .Entity<Stores>()
            .HasKey(x => x.Store_id);

        modelBuilder
            .Entity<Employee>()
            .HasKey(x => x.Emp_id);

        modelBuilder
            .Entity<Product_Amount>(entity =>
            {
                entity.HasKey(p => new { p.Product_id, p.Counter_id, p.Store_id });
                entity.Property(x => x.Pa_id).ValueGeneratedOnAdd();
                entity.Property(a => a.Update_date).HasDefaultValueSql("(getutcdate())");
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