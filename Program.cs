using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using sudoku_solver_api.Helpers;
using sudoku_solver_api.Services;

var builder = WebApplication.CreateBuilder(args); // This initiates the application setup using a `WebApplicationBuilder`. It leverages modern .NET Core approaches to set up services and middleware.

// Add services to the container - Service Registration (DI).
{
  var services = builder.Services;
  // Cors policy... 
  services.AddCors();
  // JSON serialization
  services.AddControllers().AddJsonOptions(options =>
  {
    // serialize enums as strings in api responses (e.g. Role)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); // Converts enums into strings (e.g., Difficulty.Easy => "Easy") in JSON responses.

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
  });

  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
  services.AddOpenApi();
  // Custom Service Registration
  services.AddScoped<ISudokuService, SudokuService>();
  services.AddScoped<ISudokuSolver, SudokuSolver>();
}

// App Pipeline Configuration - the pipeline specifies how the application will process incoming HTTP requests.
// a. Build the App
var app = builder.Build();

// b. Environment-Specific Behavior
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

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