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
                Include(x=>x.TiempoNavigation).ToListAsync();
            return result;
        }

        
    }
}
