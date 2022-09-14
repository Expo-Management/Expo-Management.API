using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de logs
    /// </summary>
    public class LogsRepository : ILogsRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de logs
        /// </summary>
        /// <param name="context"></param>
        public LogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo para obtener los logs del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<List<Logs>?> GetLogsAsync()
        {
            try
            {
                return await (from l in _context.Logs
                              select l).ToListAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
