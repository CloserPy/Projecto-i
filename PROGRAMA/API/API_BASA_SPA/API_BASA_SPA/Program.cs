using API_BASA_SPA.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ArriendoMaquinariasContext>(options =>
    options.UseSqlServer("Server=.\\SQLEXPRESS;Database=ARRIENDO_MAQUINARIAS;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Agregar la configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("DisableCors",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Usar la configuración de CORS
app.UseCors("DisableCors");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
