using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class DimVentum
{
    public int IdVenta { get; set; }

    public decimal? TotalVenta { get; set; }

    public virtual ICollection<Hecho> Hechoes { get; set; } = new List<Hecho>();
}
