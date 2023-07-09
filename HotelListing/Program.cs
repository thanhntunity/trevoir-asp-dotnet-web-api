using HotelListing.Configurations;
using HotelListing.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.AspNetCore;
using Serilog.Events;

Log.Logger = new LoggerConfiguration().WriteTo.File(
        path: "C:\\HotelListings\\logs\\log-.txt", outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
        rollingInterval: RollingInterval.Day,
        restrictedToMinimumLevel: LogEventLevel.Information
        ).CreateLogger();

try
{
    Log.Information("Application is starting");

    var builder = WebApplication.CreateBuilder(args);

    // Set Serilog as the default logging tool
    // builder.Host.UseSerilog();

    // CORS configuration
    builder.Services.AddCors(o => {
        o.AddPolicy("AllowAll", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    });

    // Attach the AutoMapper configuration
    builder.Services.AddAutoMapper(typeof(MapperInitializer));

    // Add services to the container.
    builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    

    builder.Services.AddSwaggerGen();

    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseCors("AllowAll");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();

    
}
catch (Exception ex)
{
    Log.Fatal(ex, "Application Failed to start");
}
finally
{
    Log.CloseAndFlush();
}


