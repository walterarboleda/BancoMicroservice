

# Vamos a construir este microservicio utilizando ASP.NET Core Web API, Entity Framework Core (EF Core) y MS SQL Server.

# Configuración de la Base de Datos (SQL Server)
Primero, asegúrate de que tu base de datos BANCO exista. Abre SQL Server Management Studio (SSMS) o la extensión de SQL en Visual Studio y ejecuta:

SQL
CREATE DATABASE BANCO;
GO
USE BANCO;
GO
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

# Estructura del Microservicio
A. El Modelo (Models/Cuenta.cs)
Define la clase que mapea a tu tabla:

C#
public class Cuenta {
    public int Id { get; set; }
    public string NumeroCuenta { get; set; } = string.Empty;
    public string Titular { get; set; } = string.Empty;
    public decimal Saldo { get; set; }
}


# Configurar la Conexión (appsettings.json)
Agrega tu cadena de conexión:

JSON
"ConnectionStrings": {
  "DefaultConnection": "Server=TU_SERVIDOR;Database=BANCO;Trusted_Connection=True;TrustServerCertificate=True;"
}


# Implementación del CRUD (Controlador)
Crea un archivo en la carpeta Controllers/CuentasController.cs:

C#
[ApiController]
[Route("api/[controller]")]
public class CuentasController : ControllerBase {
    private readonly BancoContext _context;

    public CuentasController(BancoContext context) => _context = context;

    // READ: Listar todas
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cuenta>>> GetCuentas() 
        => await _context.Cuentas.ToListAsync();

    // CREATE: Insertar cuenta
    [HttpPost]
    public async Task<ActionResult<Cuenta>> PostCuenta(Cuenta cuenta) {
        _context.Cuentas.Add(cuenta);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCuentas), new { id = cuenta.Id }, cuenta);
    }

    // UPDATE: Actualizar saldo o titular
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCuenta(int id, Cuenta cuenta) {
        if (id != cuenta.Id) return BadRequest();
        _context.Entry(cuenta).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: Borrar cuenta
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCuenta(int id) {
        var cuenta = await _context.Cuentas.FindAsync(id);
        if (cuenta == null) return NotFound();
        _context.Cuentas.Remove(cuenta);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

# Registro del Servicio (Program.cs)
Finalmente, debes decirle a la aplicación que use el contexto de base de datos que creaste:

C#
var builder = WebApplication.CreateCollection();

// Configurar SQL Server
builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.Run();


# ¿Cómo probarlo?

Al presionar F5 en Visual Studio, se abrirá automáticamente una página de Swagger (OpenAPI). Desde ahí podrás probar cada uno de los métodos (GET, POST, etc.) directamente en el navegador sin instalar herramientas externas.
