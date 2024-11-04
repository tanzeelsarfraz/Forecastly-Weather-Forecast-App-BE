using WeatherForecast.Services;
using WeatherForecast.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(configuration.GetValue<string>("FrontEndURL")).AllowAnyMethod().AllowAnyHeader();
    });
});
var app = builder.Build();
app.UseCors();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
/**
 * @summary Get Weather Forecast for next 5 days.
 * @returns List of WeatherForecastDto object.
*/
app.MapGet("api/weatherforecast", (IWeatherForecastService weatherForecastService) =>
{
    return weatherForecastService.GetWeatherForecast();
})
.WithName("GetWeatherForecast")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError)
.WithOpenApi();
/**
 * @summary Get Paged list of Weather Forecase based on defined date range.
 * @param name pageSize.
 * @param name pageNumber.
 * @param name fromDate.
 * @param name toDate.
 * @returns PagedList of WeatherForecastDto object.
 */
app.MapGet("api/weatherforecast/paged", (IWeatherForecastService weatherForecastService, int pageSize, int pageNumber, DateTime? fromDate, DateTime? toDate) =>
{
    return weatherForecastService.GetWeatherForecastPaged(pageSize, pageNumber, fromDate, toDate);
})
.WithName("GetWeatherForecastPaged")
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound)
.Produces(StatusCodes.Status500InternalServerError)
.WithOpenApi();

app.Run();
