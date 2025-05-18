using ApiLaBodeguita.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ?? PUERTO REQUERIDO PARA RENDER
builder.WebHost.UseUrls("http://+:80");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source= producto.db"));

builder.Services.AddControllers().AddJsonOptions(x =>
{
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    x.JsonSerializerOptions.WriteIndented = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.Urls.Add("http://*:10000"); // Render usar� este puerto

app.UseSwagger();
app.UseSwaggerUI();

// app.UseHttpsRedirection(); // Comenta o elimina esta l�nea en Rende

app.UseAuthorization();

app.MapControllers();

// Ejecutar migraciones autom�ticamente
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.Run();
