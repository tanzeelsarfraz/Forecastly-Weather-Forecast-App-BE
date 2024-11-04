using Microsoft.Extensions.Logging;
using WeatherForecast.Services.DTOs;
using WeatherForecast.Services.Interfaces;

namespace WeatherForecast.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly ILogger<WeatherForecastService> _logger;
        private static readonly int totalDays = 365;
        public WeatherForecastService(ILogger<WeatherForecastService> logger)
        {
            _logger = logger;
        }
        private static readonly string[] summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private static readonly IEnumerable<WeatherForecastDto> YearlyForecastData = GenerateYearlyForecastData();
        private static IEnumerable<WeatherForecastDto> GenerateYearlyForecastData()
        {
            return Enumerable.Range(1, totalDays).Select(index => new WeatherForecastDto()
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            })
            .ToArray();
        }
        /// <summary>
        /// This method returns weather forecast for next 5 days 
        /// </summary>
        /// <returns>List of WeatherForecastDto object</returns>
        public IEnumerable<WeatherForecastDto> GetWeatherForecast()
        {
            try
            {
                return Enumerable.Range(1, 5).Select(index => new WeatherForecastDto()
                {
                    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = summaries[Random.Shared.Next(summaries.Length)]
                })
                .ToArray();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
        /// <summary>
        /// This method returns paged list of weather forecast for defined date ranges. 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns>PagedListDto of WeatherForecastDto object</returns>
        public PagedListDto<WeatherForecastDto> GetWeatherForecastPaged(int pageSize, int pageNumber, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                if (pageSize > 50) pageSize = 50;
                var responseData = fromDate.HasValue && toDate.HasValue ? YearlyForecastData
                    .Where(x => x.Date >= DateOnly.FromDateTime(fromDate.Value) && x.Date <= DateOnly.FromDateTime(toDate.Value))
                    : YearlyForecastData;
                return new PagedListDto<WeatherForecastDto>()
                {
                    Data = responseData.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToArray(),
                    TotalResults = responseData.Count(),
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
