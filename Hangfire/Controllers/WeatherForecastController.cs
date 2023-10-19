using Microsoft.AspNetCore.Mvc;

namespace Hangfire.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name ="Get")]
        public IEnumerable<WeatherForecast> Get()
        {
            // Enqueue is Fire-and-forget method. 
            // It make the background job scenario but it executed without delay. 
            BackgroundJob.Enqueue(() => Greeting("Enqueue"));


            // Schedule is Scheduled method run after a determine time interval. 
            // It make the background job scenario but it executed with delay. 
            BackgroundJob.Schedule(() => Greeting("Schedule"),TimeSpan.FromMinutes(1));


            // The recurring job run every time unit you pass it. 
            // Take a Cron expression that will work with respect to it.
            RecurringJob.AddOrUpdate("GreetingEveryDay12AM",() => Greeting("Schedule"), "0  0 * * *");

            RecurringJob.AddOrUpdate("GreetingEveryMinute",() => Greeting("Recurring"), Cron.Minutely);

            RecurringJob.RemoveIfExists("GreetingEveryDay12AM");

            RecurringJob.Trigger("GreetingEveryMinute");




            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void Greeting(string where)
        {
            // Run and Forget => take the same process of serializing and deserializing and then run.
           Console.WriteLine($"Hello form {where}.");
        }


    }
}
