namespace WeatherForecast.Services.DTOs
{
    public record PagedListDto<T>
    {
        public int TotalResults { get; init; }
        public IEnumerable<T> Data { get; init; }
    }
}
