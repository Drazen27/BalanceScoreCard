using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class DimEmpleado
{
    public int IdEmpleado { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Direccion { get; set; }

    public virtual ICollection<Hecho> Hechoes { get; set; } = new List<Hecho>();
}
