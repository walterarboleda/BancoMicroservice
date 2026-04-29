

# Vamos a construir este microservicio utilizando ASP.NET Core Web API, Entity Framework Core (EF Core) y MS SQL Server.

# Configuración de la Base de Datos (SQL Server)
Primero, asegúrate de que tu base de datos BANCO exista. Abre SQL Server Management Studio (SSMS) o la extensión de SQL en Visual Studio y ejecuta:


CREATE DATABASE BANCO;

USE BANCO;



CREATE TABLE Cuenta (
    Id INT PRIMARY KEY IDENTITY(1,1),
    NumeroCuenta NVARCHAR(20) NOT NULL UNIQUE,
    Titular NVARCHAR(100) NOT NULL,
    Saldo DECIMAL(18, 2) NOT NULL DEFAULT 0,
    FechaCreacion DATETIME DEFAULT GETDATE()
);

# Creación del Proyecto en Visual Studio 2026
Abre Visual Studio y selecciona "Crear un nuevo proyecto".

Busca la plantilla ASP.NET Core Web API.

Ponle nombre al proyecto (ej. BancoService).

# Instalación de Dependencias (NuGet)
Necesitamos los paquetes para comunicarnos con SQL Server. Abre la consola del administrador de paquetes o usa el gestor NuGet:

Microsoft.EntityFrameworkCore.SqlServer

Microsoft.EntityFrameworkCore.Design

Microsoft.EntityFrameworkCore.Tools

# Creación Estructura del Microservicio

usando el PM (Packet Manager) digitar las siguientes instrucciones para generar automaticamente la carpeta Data y la carpeta Model:

Scaffold-DbContext 'Server=B8-407-37235;Database=BANCO;Trusted_Connection=True;TrustServerCertificate=True;' Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -ContextDir Data -DataAnnotations -Force


# Configurar la Conexión (del archivo appsettings.json)
Agregando al inicio la cadena de conexión a la base de datos:

"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=BANCO;Trusted_Connection=True;TrustServerCertificate=True;"
}


# Implementación de los Endpoints del CRUD  de la API(Controlador)
Crea un archivo llamado CuentasController.cs en la carpeta Controllers/ mediante:
1. Agregar
2. Controlador
3. API/ Controlador API en blanco y colocar el siguiente codigo fuente:
   
using BancoService.Data;
using BancoService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BancoService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly BancoContext _context;

        public CuentasController(BancoContext context) => _context = context;

        // READ: Listar todas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas()
            => await _context.Cuenta.ToListAsync();

        // CREATE: Insertar cuenta
        [HttpPost]
        public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta)
        {
            _context.Cuenta.Add(cuenta);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCuentas), new { id = cuenta.Id }, cuenta);
        }

        // UPDATE: Actualizar saldo o titular
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, Cuenta cuenta)
        {
            if (id != cuenta.Id) return BadRequest();
            _context.Entry(cuenta).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: Borrar cuenta
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = await _context.Cuenta.FindAsync(id);
            if (cuenta == null) return NotFound();
            _context.Cuenta.Remove(cuenta);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}


# Registro del Servicio en el archivo Program.cs
Finalmente, debes decirle a la aplicación que use el contexto de base de datos que creaste usando el siguiente codigo:

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

# ¿Cómo probarlo?

Al presionar F5 en Visual Studio, se abrirá automáticamente una página de Swagger (OpenAPI). Desde ahí podrás probar cada uno de los métodos (GET, POST, etc.) directamente en el navegador sin instalar herramientas externas.


# Referencias

https://javiergarzas.com/2015/06/microservicios.html

https://jugnicaragua.org/comunicacion-entre-microservicios-con-spring/

https://www.mytaskpanel.com/arquitectura-de-microservicios-escalabilidad-y-flexibilidad/

https://www.oscarblancarteblog.com/2018/05/22/que-son-los-microservicios/


