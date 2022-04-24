using Microsoft.EntityFrameworkCore;

public class EFContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }

    public string connectionString;

    public EFContext(IConfiguration configuration)
    {
        connectionString = configuration.GetConnectionString("DefaultConnection");
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(500).IsRequired(false);
        modelBuilder.Entity<Product>()
            .Property(p => p.Name).HasMaxLength(120).IsRequired();
        modelBuilder.Entity<Product>()
            .Property(p => p.Code).HasMaxLength(500).IsRequired();
        modelBuilder.Entity<Category>()
            .ToTable("Categories");
    }
}