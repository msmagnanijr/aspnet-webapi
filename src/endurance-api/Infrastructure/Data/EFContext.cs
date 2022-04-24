namespace Endurance.Infrastructure.Data;
public class EFContext : IdentityDbContext<IdentityUser>
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

        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<Notification>();

        modelBuilder.Entity<Product>()
            .Property(p => p.Name).IsRequired();
        modelBuilder.Entity<Product>()
            .Property(p => p.Description).HasMaxLength(500);

        modelBuilder.Entity<Category>()
            .Property(p => p.Name).IsRequired();
    }
    protected override void ConfigureConventions(ModelConfigurationBuilder modelBuilder)
    {
        modelBuilder.Properties<string>()
            .HaveMaxLength(100);
    }
}