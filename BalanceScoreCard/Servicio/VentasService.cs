using BalanceScoreCard.Data;
using BalanceScoreCard.Models;
using Microsoft.EntityFrameworkCore;

namespace BalanceScoreCard.Servicio
{
    public class VentasService: IVentasService
    {
        private readonly CerveceriaDwContext _context;

        public VentasService(CerveceriaDwContext context)
        {
            _context = context;
        }

        public async Task<List<Hecho>> GetAllVentasAsync()
        {
            var result = await _context.Hechoes
                .Include(x=>x.VentaNavigation)
                .Include(x=>x.DistribuidorNavigation)
                .Include(x => x.ProductoNavigation)
                .Include(x=>x.EmpleadoNavigation).
                Include(x=>x.TiempoPedidoNavigation)
                .Include(x=>x.TiempoEntregaNavigation).ToListAsync();
            return result;
        }

        public async Task<decimal> GetEficienciaGeneralAsync()
        {
            var ventas = await _context.Hechoes
                .Include(x => x.VentaNavigation)
                .ToListAsync();

            decimal totalVentas = ventas.Sum(v => v.SubTotal ?? 0);
            int numeroTotalVentas = ventas.Count;

            decimal eficienciaGeneral = numeroTotalVentas > 0 ? totalVentas / numeroTotalVentas : 0;
            return eficienciaGeneral;
        }
        public async Task<TimeSpan> GetTiempoEntregaPromedioAsync()
        {
            var ventas = await _context.Hechoes
                .Include(x => x.TiempoPedidoNavigation)
                .Include(x => x.TiempoEntregaNavigation)
                .ToListAsync();

            var tiemposEntrega = ventas
                .Where(v => v.TiempoPedidoNavigation != null && v.TiempoEntregaNavigation != null)
                .Select(v => v.TiempoEntregaNavigation.Fecha - v.TiempoPedidoNavigation.Fecha)
                .ToList();

            TimeSpan tiempoEntregaPromedio = tiemposEntrega.Any()
                ? new TimeSpan(Convert.ToInt64(tiemposEntrega.Average(t => t.Ticks)))
                : TimeSpan.Zero;

            return tiempoEntregaPromedio;
        }
        public async Task<decimal> GetTotalVentasAsync()
        {
            return await _context.Hechoes.SumAsync(v => v.SubTotal ?? 0);
        }

        public async Task<int> GetNumeroTotalVentasAsync()
        {
            return await _context.Hechoes.CountAsync();
        }
        public async Task<decimal> GetIndiceEficienciaAsync(TimeSpan tiempoEntregaPromedioIdeal)
        {
            decimal totalVentas = await GetTotalVentasAsync();
            int numeroTotalVentas = await GetNumeroTotalVentasAsync();
            TimeSpan tiempoEntregaPromedioReal = await GetTiempoEntregaPromedioAsync();

            if (numeroTotalVentas == 0 || tiempoEntregaPromedioReal == TimeSpan.Zero)
            {
                return 0;
            }

            decimal promedioVentasPorTransaccion = totalVentas / numeroTotalVentas;
            decimal factorTiempoEntrega = tiempoEntregaPromedioReal.Ticks > 0
                ? (decimal)tiempoEntregaPromedioIdeal.Ticks / tiempoEntregaPromedioReal.Ticks
                : 1;

            decimal indiceEficiencia = promedioVentasPorTransaccion * factorTiempoEntrega;
            return indiceEficiencia;
        }
        public async Task<decimal> GetIndiceSatisfaccionAsync()
        {
            
            var ventas = await _context.Hechoes
                .Include(x => x.TiempoPedidoNavigation)
                .Include(x => x.TiempoEntregaNavigation)
                .ToListAsync();

           
            var ventasConFechasValidas = ventas
                .Where(v => v.TiempoPedidoNavigation != null && v.TiempoEntregaNavigation != null)
                .ToList();

            int numeroTotalRespondientes = ventasConFechasValidas.Count;

            int numeroClientesSatisfechos = ventasConFechasValidas
                .Count(v => (v.TiempoEntregaNavigation.Fecha - v.TiempoPedidoNavigation.Fecha).TotalDays <= 5);

            decimal indiceSatisfaccion = numeroTotalRespondientes > 0
                ? ((decimal)numeroClientesSatisfechos / numeroTotalRespondientes) * 100
                : 0;

            return indiceSatisfaccion;
        }
        public async Task<List<(int? IdDistribuidor, DateTime FechaPedido)>?> GetDistribuidoresConFechasAsync()
        {
            var list = await _context.Hechoes
                .Include(h => h.TiempoPedidoNavigation)
                .Where(h => h.TiempoPedidoNavigation != null)
                .Select(h => new { h.IdDistribuidor, h.TiempoPedidoNavigation.Fecha })
                .ToListAsync()
                .ContinueWith(task => task.Result
                    .Select(h => (h.IdDistribuidor, h.Fecha))
                    .ToList());
            return list;
        }
        public async Task<decimal> GetTasaRetencionAsync()
        {
            var distribuidoresConFechas = await GetDistribuidoresConFechasAsync();

            var distribuidoresPorAnio = distribuidoresConFechas
                .GroupBy(df => df.FechaPedido.Year)
                .ToDictionary(g => g.Key, g => g.Select(df => df.IdDistribuidor).Distinct().ToList());

            if (distribuidoresPorAnio.Count < 2)
            {
                return 0; // No hay suficientes datos para calcular la retención
            }

            var anios = distribuidoresPorAnio.Keys.OrderBy(anio => anio).ToList();
            var distribuidoresRetenidos = new HashSet<int?>(distribuidoresPorAnio[anios[0]]);

            for (int i = 1; i < anios.Count; i++)
            {
                distribuidoresRetenidos.IntersectWith(distribuidoresPorAnio[anios[i]]);
            }

            int numeroTotalClientesIniciales = distribuidoresPorAnio[anios[0]].Count;
            int numeroClientesRetenidos = distribuidoresRetenidos.Count;

            decimal tasaRetencion = numeroTotalClientesIniciales > 0
                ? ((decimal)numeroClientesRetenidos / numeroTotalClientesIniciales) * 100
                : 0;

            return tasaRetencion;
        }
        public async Task<Dictionary<int, decimal>> GetIngresosPorMesAsync()
        {
            var ingresosPorAnio = await _context.Hechoes
                .Include(h => h.TiempoPedidoNavigation)
                .Where(h => h.TiempoPedidoNavigation != null)
                .GroupBy(h => h.TiempoPedidoNavigation.Fecha.Month)
                .Select(g => new
                {
                    Mes = g.Key,
                    Ingresos = g.Sum(h => h.SubTotal ?? 0)
                })
                .ToDictionaryAsync(g => g.Mes, g => g.Ingresos);

            return ingresosPorAnio;
        }
        public async Task<decimal> GetCrecimientoPorcentualIngresosAsync()
        {
            var ingresosPorAnio = await GetIngresosPorMesAsync();

            if (ingresosPorAnio.Count < 2)
            {
                return 0; // No hay suficientes datos para calcular el crecimiento
            }

            var aniosOrdenados = ingresosPorAnio.Keys.OrderBy(anio => anio).ToList();
            int anioActual = aniosOrdenados.Last();
            int anioAnterior = aniosOrdenados[aniosOrdenados.Count - 2];

            decimal ingresosActuales = ingresosPorAnio[anioActual];
            decimal ingresosAnteriores = ingresosPorAnio[anioAnterior];

            decimal crecimientoPorcentual = ingresosAnteriores > 0
                ? ((ingresosActuales - ingresosAnteriores) / ingresosAnteriores) * 100
                : 0;

            return crecimientoPorcentual * -1;
        }
        public partial class DatosFinancieros
        {
            public decimal CostosOperativosTotales { get; set; }
            public decimal IngresosTotales { get; set; }
        }
        public async Task<DatosFinancieros> GetDatosFinancierosAsync()
        {
            // Este método debe implementarse para recuperar los datos financieros necesarios.
            // Aquí, se puede obtener desde una base de datos, un servicio externo, etc.
            return new DatosFinancieros
            {
                CostosOperativosTotales = 70000, // Ejemplo de datos
                IngresosTotales = 200000 // Ejemplo de datos
            };
        }
        public async Task<decimal> GetCostoOperacionPorcentajeAsync()
        {
            var datosFinancieros = await GetDatosFinancierosAsync();

            decimal costoOperacionPorcentaje = datosFinancieros.IngresosTotales > 0
                ? (datosFinancieros.CostosOperativosTotales / datosFinancieros.IngresosTotales) * 100
                : 0;

            return costoOperacionPorcentaje;
        }
    }
}
