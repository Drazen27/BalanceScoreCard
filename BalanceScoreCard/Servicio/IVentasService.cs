using BalanceScoreCard.Models;

namespace BalanceScoreCard.Servicio
{
    public interface IVentasService
    {
        Task<List<Hecho>> GetAllVentasAsync();
        Task<decimal> GetEficienciaGeneralAsync();
        Task<TimeSpan> GetTiempoEntregaPromedioAsync();
        Task<decimal> GetTotalVentasAsync();
        Task<int> GetNumeroTotalVentasAsync();
        Task<decimal> GetIndiceEficienciaAsync(TimeSpan tiempoEntregaPromedioIdeal);
        Task<decimal> GetIndiceSatisfaccionAsync();
        Task<decimal> GetTasaRetencionAsync();
        Task<decimal> GetCrecimientoPorcentualIngresosAsync();
        Task<decimal> GetCostoOperacionPorcentajeAsync();
    }
}
