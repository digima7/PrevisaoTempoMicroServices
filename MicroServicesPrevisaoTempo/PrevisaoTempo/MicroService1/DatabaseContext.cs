using Microsoft.EntityFrameworkCore;

namespace MicroService1
{
    public class DatabaseContext : DbContext
    {
        private readonly DatabaseConfig _config;
        
        public DbSet<CityWeather> CityWeathers { get; set; }

        public DatabaseContext(DatabaseConfig config)
        {
            _config = config;
            Database.EnsureCreated();
        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_config.Type == "postgresql")
                optionsBuilder.UseNpgsql(_config.ConnectionString);
            else
                optionsBuilder.UseSqlite(_config.ConnectionString);
        }
    }
}