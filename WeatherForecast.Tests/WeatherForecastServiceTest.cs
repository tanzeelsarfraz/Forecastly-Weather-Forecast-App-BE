using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using WeatherForecast.Services;
using WeatherForecast.Services.Interfaces;

namespace WeatherForecast.Tests
{
    public class WeatherForecastServiceTest
    {
        private readonly IWeatherForecastService _weatherForecastService;
        private readonly Mock<ILogger<WeatherForecastService>> _loggerMock;

        public WeatherForecastServiceTest()
        {
            _loggerMock = new Mock<ILogger<WeatherForecastService>>();
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddScoped<IWeatherForecastService, WeatherForecastService>();
            serviceCollection.AddSingleton(_loggerMock.Object);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            _weatherForecastService = serviceProvider.GetRequiredService<IWeatherForecastService>();
        }
        [Fact]
        public void GetWeatherForecastTest()
        {
            var response = _weatherForecastService.GetWeatherForecast();
            Assert.NotNull(response);
            Assert.True(response.Any());
        }
        [Theory]
        [InlineData(5, 1)]
        [InlineData(10, 2)]
        [InlineData(0, 1)]
        public void GetWeatherForecastPagedVariousPageSizesTest(int pageSize, int pageNumber)
        {
            var response = _weatherForecastService.GetWeatherForecastPaged(pageSize, pageNumber, null, null);
            Assert.NotNull(response);
            Assert.True(response.Data.Count() <= pageSize);
        }
        [Theory]
        [InlineData(70, 1, 50)]
        public void GetWeatherForecastPagedSizeGreaterThan50Test(int pageSize, int pageNumber, int expectedCount)
        {
            var response = _weatherForecastService.GetWeatherForecastPaged(pageSize, pageNumber, null, null);
            Assert.NotNull(response);
            Assert.True(response.Data.Count() == expectedCount);
        }
        [Theory]
        [InlineData(10, 1, "2024-11-07", "2024-11-10")]
        [InlineData(10, 1, "2024-11-05", "2024-11-05")]
        public void GetWeatherForecastPagedDatesTest(int pageSize, int pageNumber, string fromDateString, string toDateString)
        {
            DateTime fromDate = Convert.ToDateTime(fromDateString);
            DateTime toDate = Convert.ToDateTime(toDateString);
            var response = _weatherForecastService.GetWeatherForecastPaged(pageSize, pageNumber, fromDate, toDate);
            Assert.True(response.Data.Count() <= ((toDate - fromDate).TotalDays + 1));
        }
    }
}