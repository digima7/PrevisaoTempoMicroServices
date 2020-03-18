namespace MicroService1
{
    public class WeatherDatabase
    {
        private readonly DatabaseConfig _config;

        public WeatherDatabase(DatabaseConfig config)
        {
            _config = config;
        }

        public DatabaseContext Context()
        {
            return new DatabaseContext(_config);
        }
    }
}