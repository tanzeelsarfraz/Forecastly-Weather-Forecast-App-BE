
using WeatherForecast.Services.DTOs;

namespace WeatherForecast.Services.Interfaces
{
    public interface IWeatherForecastService
    {
        IEnumerable<WeatherForecastDto> GetWeatherForecast();
        PagedListDto<WeatherForecastDto> GetWeatherForecastPaged(int pageSize, int pageNumber, DateTime? fromDate, DateTime? toDate);
    }
}
