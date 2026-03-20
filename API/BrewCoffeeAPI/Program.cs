using BrewCoffeeAPI.Interfaces;
using BrewCoffeeAPI.Services;
using DotNetEnv;

Env.Load();
var baseUrl = Environment.GetEnvironmentVariable("OPENWEATHER_BASEURL");
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddSingleton<ICounterService, CounterService>();
builder.Services.AddSingleton<IWeatherService, WeatherService>();
builder.Services.AddHttpClient("OpenWeather", client =>
{
    client.BaseAddress = new Uri(baseUrl);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
