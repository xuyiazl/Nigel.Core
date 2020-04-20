using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Nigel.Core;
using Nigel.Core.HttpFactory;
using Nigel.Webs;

namespace Nigel.MessageApiTest.Controllers
{

    [ApiController]
    [Route("[Controller]/[Action]")]
    public class WeatherForecastController : ApiControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IHttpService _httpService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpService httpService) : base(logger)
        {
            _logger = logger;
            _httpService = httpService;
        }

        [HttpGet]
        public async Task<Result<List<WeatherForecast>>> GetDemo()
        {
            var url = UrlArguments.Create("msgpack", "WeatherForecast/GetWeather");

            var res = await _httpService.GetAsync<Result<List<WeatherForecast>>>(url);

            return Success("0000001", "查询成功", res.data);
        }

        [HttpGet]
        public Result<List<WeatherForecast>> GetWeather()
        {
            var rng = new Random();
            var res = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.UtcNow.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToList();

            return Success("0000001", "查询成功", res);
        }

        [HttpGet]
        public async Task<Result<WeatherForecast>> PostDemo()
        {
            var url = UrlArguments.Create("msgpack", "WeatherForecast/PostWeather");

            WeatherForecast weather = new WeatherForecast
            {
                Date = DateTime.UtcNow,
                Summary = "测试POST",
                TemperatureC = 33
            };

            var res = await _httpService.PostAsync<Result<WeatherForecast>, WeatherForecast>(url, weather);

            return Success("0000001", "POST成功", res.data);
        }

        [HttpPost]
        public Result<WeatherForecast> PostWeather([FromBody] WeatherForecast weatherForecast)
        {
            weatherForecast.Summary = "POST成功，接收后修改的Summary";

            return Success("0000001", "POST成功", weatherForecast);
        }
    }
}
