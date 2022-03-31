using CyberSec.Cryptocurrency.Service.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CyberSec.Cryptocurrency.Service.Persistence;

public class CryptocurrencyContext : DbContext
{
    public DbSet<User> Users { get; set; }

    private readonly IConfiguration Configuration;

    public CryptocurrencyContext(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // in memory database used for simplicity, change to a real db for production applications
        options.UseInMemoryDatabase("TestDb");
    }
}