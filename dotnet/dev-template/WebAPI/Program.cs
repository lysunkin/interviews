using Payload;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{
    var greeter = new Hello();
    return greeter.Greet();
})
.WithName("Index");

app.MapGet("/weatherforecast", () =>
{
    var w = new WeatherProvider();
    return w.GetForecasts();
})
.WithName("GetWeatherForecast");

app.Run();
