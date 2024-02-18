using System.Data.SQLite;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<AppDbContext>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new AppDbContext(connectionString);
});

builder.Services.AddScoped<IDriverRepository>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new DriverRepository(provider.GetRequiredService<ILogger<DriverRepository>>(), new AppDbContext(connectionString));
});
builder.Services.AddScoped<DriverService>();
builder.Services.AddLogging(builder =>
{
    builder.AddConsole();
});


var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        using (var command = dbContext.CreateCommand())
        {
            command.CommandText = "CREATE TABLE IF NOT EXISTS Driver (id INTEGER PRIMARY KEY AUTOINCREMENT, firstName varChar(50), lastName varChar(50), phone varChar(20) null);";
            command.ExecuteNonQuery();
        }
        logger.LogInformation($"Table created successfully on db: {dbContext.GetConnectionString()}");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();

