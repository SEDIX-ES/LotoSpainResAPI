using LotoSpain_API.Datos;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder =>
        {
            builder.AllowAnyOrigin(); // Permite cualquier origen
            builder.AllowAnyMethod(); // Permite cualquier método HTTP
            builder.AllowAnyHeader(); // Permite cualquier header
        });
});

builder.Services.AddSingleton(new MySqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<MySqlConnection, MySqlConnection>();
builder.Services.AddDbContext<ApplicationDBContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), options => options.CommandTimeout(180));
});

// Se configura Kestrel para escuchar en el puerto especificado por la variable de entorno PORT, o 8080 por defecto
string port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(int.Parse(port));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
