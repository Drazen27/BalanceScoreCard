using BalanceScoreCard.Models;

namespace BalanceScoreCard.Servicio
{
    public interface IVentasService
    {
        Task<List<Hecho>> GetAllVentasAsync();
    }
}
