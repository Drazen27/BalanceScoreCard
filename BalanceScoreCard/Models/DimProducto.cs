using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class DimProducto
{
    public int IdProducto { get; set; }

    public string? Nombre { get; set; }

    public decimal? PrecioUnit { get; set; }

    public virtual ICollection<Hecho> Hechoes { get; set; } = new List<Hecho>();
}
