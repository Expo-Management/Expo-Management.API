using Expo_Management.API.Auth;
using Expo_Management.API.Entities.Logs;
using Expo_Management.API.Interfaces;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de logs
    /// </summary>
    public class LogsRepository: ILogsRepository
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
        async Task<List<Logs>> ILogsRepository.GetLogsAsync()
        {
            var results = (from l in _context.Logs
                           select l).ToList();

            if(results != null)
            {
                return results;
            }
            return null;
        }
    }
}
