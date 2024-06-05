using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class DimTiempo
{
    public DateOnly Fecha { get; set; }

    public int? Dia { get; set; }

    public int? Mes { get; set; }

    public int? Trimestre { get; set; }

    public int? Anio { get; set; }

    public virtual ICollection<Hecho> Hechoes { get; set; } = new List<Hecho>();
}
