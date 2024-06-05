using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class DimDistribuidor
{
    public int IdDistribuidor { get; set; }

    public string? Nombre { get; set; }

    public string Ciudad { get; set; } = null!;

    public string Direccion { get; set; } = null!;

    public virtual ICollection<Hecho> Hechoes { get; set; } = new List<Hecho>();
}
