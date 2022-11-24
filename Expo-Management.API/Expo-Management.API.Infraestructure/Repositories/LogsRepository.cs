using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Expo_Management.API.Domain.Models.Reponses;

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
        public async Task<Response?> GetLogsAsync()
        {
            try
            {
                List<Logs> logs = await (from l in _context.Logs
                                  select l).ToListAsync();
                if (logs.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = logs,
                        Message = "Logs encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Logs no encontrados."
                };
            }
            catch (Exception)
            {
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }
    }
}
