using System;
using System.Collections.Generic;

namespace BalanceScoreCard.Models;

public partial class Hecho
{
    public int IdHecho { get; set; }

    public int? IdVenta { get; set; }

    public int? IdProducto { get; set; }

    public DateTime? IdTiempoPedido { get; set; }

    public DateTime? IdTiempoEntrega { get; set; }

    public int? IdDistribuidor { get; set; }

    public int? IdEmpleado { get; set; }

    public int? Cantidad { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual DimDistribuidor? DistribuidorNavigation { get; set; }

    public virtual DimEmpleado? EmpleadoNavigation { get; set; }

    public virtual DimProducto? ProductoNavigation { get; set; }

    public virtual DimTiempoEntrega? TiempoEntregaNavigation { get; set; }

    public virtual DimTiempoPedido? TiempoPedidoNavigation { get; set; }

    public virtual DimVenta? VentaNavigation { get; set; }
}
