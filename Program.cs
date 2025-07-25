using System.Text.Json.Serialization;
using sudoku_solver_api.Filters;
using sudoku_solver_api.Helpers;
using sudoku_solver_api.Interfaces;
using sudoku_solver_api.Services;
using Serilog;

Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
  .CreateLogger();

var
  builder = WebApplication
    .CreateBuilder(args); // This initiates the application setup using a `WebApplicationBuilder`. It leverages modern .NET Core approaches to set up services and middleware.

// Add services to the container - Service Registration (DI).
{
  builder.Host.UseSerilog();
  var services = builder.Services;
  // Cors policy... 
  services.AddCors();
  // Add memory caching
  services.AddMemoryCache();
  // JSON serialization
  services.AddControllers(config =>
  {
    config.Filters.Add<ValidationFilter>(); // Adds ValidationFilter globally
  }).AddJsonOptions(options =>
  {
    // serialize enums as strings in api responses (e.g. Role)
    options.JsonSerializerOptions.Converters.Add(
      new JsonStringEnumConverter()); // Converts enums into strings (e.g., Difficulty.Easy => "Easy") in JSON responses.

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
  });

  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
  services.AddOpenApi();
  // Custom Service Registration
  services.AddScoped<IGridGenerator, GridGenerator>();
  services.AddScoped<ISudokuService, SudokuService>();
  services.AddScoped<ISudokuSolver, SudokuSolver>();
  services.AddSingleton<ICustomConverter, CustomConverter>();
}

// App Pipeline Configuration - the pipeline specifies how the application will process incoming HTTP requests.
// a. Build the App
var app = builder.Build();

// b. Environment-Specific Behavior
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

// Add exception middleware
app.UseMiddleware<ExceptionMiddleware>();

{
// global cors policy 
  app.UseCors(x =>
  {
    x.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
  });

  // Map Controllers - Connects the application's controller routes to the request pipeline, establishing the routes defined in controllers.
  app.MapControllers();
}

app.Run();