namespace DelegasiAPI
{
    public static class WeatherForecastData
    {
        public static List<WeatherForecast> Data = new List<WeatherForecast>()
        {
            new WeatherForecast()
            {
                Id = 1,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Freezing",
            },
            new WeatherForecast()
            {
                Id = 2,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Bracing",
            },
            new WeatherForecast()
            {
                Id = 3,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Chilly",
            },
            new WeatherForecast()
            {
                Id = 4,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Cool",
            },
            new WeatherForecast()
            {
                Id = 5,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Mild",
            },
            new WeatherForecast()
            {
                Id = 6,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Warm",
            },
            new WeatherForecast()
            {
                Id = 7,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Balmy",
            },
            new WeatherForecast()
            {
                Id = 8,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Hot",
            },
            new WeatherForecast()
            {
                Id = 9,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Sweltering",
            },
            new WeatherForecast()
            {
                Id = 10,
                Date = DateTime.Now,
                TemperatureC = Random.Shared.Next(-11, 32),
                Summary = "Scorching"
            },
        };
    }

    public class WeatherForecast
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}