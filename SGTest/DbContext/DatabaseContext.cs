using Microsoft.EntityFrameworkCore;

public class DatabaseContext(DatabaseSettings databaseSettings) : DbContext
{
    public DbSet<Department> Department { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<JobTitle> JobTitles { get; set; }

    private readonly DatabaseSettings _databaseSettings = databaseSettings;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_databaseSettings.ConnectionString);
    }
}