using BancoService.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();

// Configurar SQL Server
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();