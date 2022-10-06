using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de protocolos de seguridad
    /// </summary>
    public class ProtocolsRepository : IProtocolsRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProtocolsRepository> _logger;

        /// <summary>
        /// Constructor del repositorio de protocolos de seguridad
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>

        public ProtocolsRepository(ApplicationDbContext context, ILogger<ProtocolsRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para crear protocolo de seguridad
        /// </summary>
        /// <param name="description"></param>
        /// <returns></returns>
        public async Task<SecurityProtocols?> CreateProtocolAsync(string description)
        {
            try
            {
                if (description != null)
                {

                    var LastId = await (from x in _context.SecurityProtocols
                                           .OrderByDescending(x => x.Id)
                                        select x.Id).FirstOrDefaultAsync();

                    var CurrentFair = await (from x in _context.Fair
                                             where x.StartDate.Year == DateTime.Now.Year
                                             select x).FirstOrDefaultAsync();

                    var newProtocol = new SecurityProtocols()
                    {
                        Name = "PrN:" + LastId,
                        Description = description,
                        Fair = CurrentFair

                    };

                    if (await _context.SecurityProtocols.AddAsync(newProtocol) != null && CurrentFair != null)
                    {
                        await _context.SaveChangesAsync();
                        return newProtocol;
                    }
                }
                _logger.LogWarning("Error al crear el protocolo de seguridad.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
        /// <summary>
        /// Metodo para eliminar protocolos de seguridad
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteProtocolAsync(int id)
        {
            try
            {
                var result = await (from x in _context.SecurityProtocols
                                    where x.Id == id
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    _context.SecurityProtocols.Remove(result);
                    _context.SaveChanges();
                    return true;
                }

                _logger.LogWarning("Error al eliminar el protocolo de seguridad.");
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

    }
}
