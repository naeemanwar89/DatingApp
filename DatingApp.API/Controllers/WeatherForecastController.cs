using System;
using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DatingApp.API.Controllers
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
        private readonly DataContext _dataContext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, DataContext dataContext)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var values = _dataContext.Values.ToList();
            return Ok(values);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var value = _dataContext.Values.FirstOrDefault(x => x.Id == id);
            return Ok(value);
        }
    }
}
