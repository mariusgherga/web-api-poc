using Microsoft.AspNetCore.Mvc;

namespace Dummy.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("encrypt")]
        public IActionResult EncryptString([FromBody] EncryptRequest request)
        {
            if (string.IsNullOrEmpty(request.PlainText) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Plain text and password are required.");
            }

            string encryptedString = EncryptionHelper.EncryptString(request.PlainText, request.Password);
            return Ok(new { EncryptedText = encryptedString });
        }

        [HttpPost("decrypt")]
        public IActionResult DecryptString([FromBody] EncryptRequest request)
        {
            if (string.IsNullOrEmpty(request.EncryptedText) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest("Encrypted text and password are required.");
            }

            string decryptedString = EncryptionHelper.DecryptString(request.EncryptedText, request.Password);
            return Ok(new { DecryptedText = decryptedString });
        }
    }

    public class EncryptRequest
    {
        public string PlainText { get; set; }
        public string EncryptedText { get; set; }
        public string Password { get; set; }
    }
}
