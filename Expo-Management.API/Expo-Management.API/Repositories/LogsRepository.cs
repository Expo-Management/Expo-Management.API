using Expo_Management.API.Auth;
using Expo_Management.API.Entities.Logs;
using Expo_Management.API.Interfaces;

namespace Expo_Management.API.Repositories
{
    public class LogsRepository: ILogsRepository
    {
        private readonly ApplicationDbContext _context;

        public LogsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
