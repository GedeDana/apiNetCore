using DelegasiAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DelegasiAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // TODO: tambahkan pencarian WeatherForecast berdasarkan summary. Misal ketik "free" maka akan muncul data yang mengandung summary dg kata "free"
        [HttpGet]
        public ActionResult<List<WeatherForecast>> Get(string? summary = null, int? pageIndex = 0, int? pageSize = 2)
        {
            var data = WeatherForecastData.Data;

            if (!string.IsNullOrEmpty(summary))
            {
                data = data.Where(x => x.Summary != null && x.Summary.Contains(summary, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            var count = data.Count;

            data = data.Skip(pageIndex * pageSize ?? 0).Take(pageSize ?? 2).ToList();


            var result = new PageResult<WeatherForecast>(data, pageIndex.GetValueOrDefault(), pageSize.GetValueOrDefault(), count);

            return Ok(result);
        }

        [HttpGet("{id}", Name = "GetWeatherForecastById")]
        public ActionResult<WeatherForecast> Get(int id)
        {
            var result = WeatherForecastData.Data.FirstOrDefault(x => x.Id == id);

            if (result == null) return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<WeatherForecast> Post([FromBody] WeatherForecast model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Model tidak ada" });
            }

            if (WeatherForecastData.Data.Any(x => x.Id == model.Id))
            {
                return BadRequest(new { message = "Data duplikat" });
            }

            WeatherForecastData.Data.Add(model);

            return Created(Url.Action("Get", new { model.Id }), model);
        }

        [HttpPut("{id}")]
        public ActionResult<WeatherForecast> Put(int id, [FromBody] WeatherForecast model)
        {
            if (model == null)
            {
                return BadRequest(new { message = "Model tidak ada" });
            }

            var existingModel = WeatherForecastData.Data.FirstOrDefault(x => x.Id == id);

            if (existingModel == null) return NotFound();

            existingModel.Date = model.Date;
            existingModel.TemperatureC = model.TemperatureC;
            existingModel.Summary = model.Summary;

            return Ok(existingModel);
        }

        [HttpDelete("{id}")]
        public ActionResult<WeatherForecast> Put(int id)
        {
            var existingModel = WeatherForecastData.Data.FirstOrDefault(x => x.Id == id);

            if (existingModel == null) return NotFound();

            WeatherForecastData.Data.Remove(existingModel);

            return Ok(existingModel);
        }

  
    }
}