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

builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithJwt();

builder.Services.AddDbContext<ApplicationDatabaseContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(options =>
{
    options.SingleLine = true;
    options.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
    options.IncludeScopes = false;
});
builder.Services.AddTransient<InitialDataSeeder>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    //app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Vehicle Rental Platform API";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vehicle Rental Platform");
        options.DefaultModelsExpandDepth(-1);
    });
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDatabaseContext>();
    dbContext.Database.Migrate();

    var seeder = scope.ServiceProvider.GetRequiredService<InitialDataSeeder>();

    string csvPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "vehicles.csv");
    seeder.SeedVehicles(csvPath);

    var telemetryPath = Path.Combine(Directory.GetCurrentDirectory(), "Files", "telemetry.csv");
    seeder.SeedTelemetry(telemetryPath);

    seeder.SeedUsers();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Urls.Add("http://0.0.0.0:80");
app.Run();
