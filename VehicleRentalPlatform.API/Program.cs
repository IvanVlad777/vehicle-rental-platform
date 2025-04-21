using Microsoft.EntityFrameworkCore;
using VehicleRentalPlatform.Infrastructure.Data;
using System;
using VehicleRentalPlatform.Infrastructure.Data.Seeders;
using VehicleRentalPlatform.API.Extensions;
using VehicleRentalPlatform.Application.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.IncludeScopes = false;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
    var seeder = scope.ServiceProvider.GetRequiredService<InitialDataSeeder>();

    string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "vehicles.csv");
    seeder.SeedVehicles(csvPath);

    var telemetryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "telemetry.csv");
    seeder.SeedTelemetry(telemetryPath);
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
