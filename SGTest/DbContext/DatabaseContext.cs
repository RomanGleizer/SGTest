using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DbSet<Department> Department { get; set; }

    public DbSet<Employee> Employees { get; set; }

    public DbSet<JobTitle> JobTitles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(ConfigManager.ReadDbConfig("config.json").ConnectionString);
    }
}