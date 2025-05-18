using ApiLaBodeguita.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "wwwroot",
    ApplicationName = typeof(Program).Assembly.FullName
});

// ?? PUERTO REQUERIDO PARA RENDER
builder.WebHost.UseUrls("http://+:80");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source= producto.db"));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.PropertyNamingPolicy = null;
    x.JsonSerializerOptions.WriteIndented = true;
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

var app = builder.Build();

app.Urls.Add("http://*:10000"); // Render usará este puerto

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Comenta o elimina esta línea en Rende

app.UseAuthorization();

app.MapControllers();

// Ejecutar migraciones automáticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
