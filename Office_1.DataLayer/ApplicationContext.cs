using Microsoft.EntityFrameworkCore;
using Office_1.DataLayer.Models;

namespace Office_1.DataLayer;

public sealed class ApplicationContext : DbContext
{

    public DbSet<Request> Requests { get; set; }
    public DbSet<Client> Clients { get; set; }
    
    public DbSet<Settings> Settings { get; set; }

    public ApplicationContext()
    {
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("FileName=Application.db");
        optionsBuilder.UseSqlite("DataSource=D:/Work-3/Other/Practice/Office_1/Application.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Request>()
            .HasOne(request => request.Client)
            .WithMany(client => client.Requests);

        modelBuilder.Entity<Client>()
            .HasIndex(p => new { p.Name, p.Address }).IsUnique();
    }

}