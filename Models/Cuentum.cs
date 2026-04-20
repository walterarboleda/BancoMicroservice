using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BancoService.Models;

[Index("NumeroCuenta", Name = "UQ__Cuenta__E039507B3D61896C", IsUnique = true)]
public partial class Cuenta
{
    [Key]
    public int Id { get; set; }

    [StringLength(20)]
    public string NumeroCuenta { get; set; } = null!;

    [StringLength(100)]
    public string Titular { get; set; } = null!;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Saldo { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? FechaCreacion { get; set; }
}
