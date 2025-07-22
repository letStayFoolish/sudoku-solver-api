using System.Text.Json.Serialization;
using sudoku_solver_api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
{
  var services = builder.Services;
  // Cors policy... 
  services.AddCors();
  services.AddControllers().AddJsonOptions(options =>
  {
    // serialize enums as strings in api responses (e.g. Role)
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());

    // ignore omitted parameters on models to enable optional params (e.g. User update)
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
  });

  // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
  services.AddOpenApi();

  services.AddScoped<ISudokuService, SudokuService>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
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

  app.MapControllers();
}

app.Run();