using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de ferias
    /// </summary>
    public class FairRepository : IFairRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FairRepository> _logger;

        /// <summary>
        /// Constructor del repositorio de ferias
        /// </summary>
        /// <param name="context"></param>
        public FairRepository(ApplicationDbContext context, ILogger<FairRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para crear ferias
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Fair?> CreateFairAsync(NewFairInputModel model)
        {
            try
            {
                if(model != null)
                {
                    var newFair = new Fair()
                    {
                        StartDate = model.StartDate,
                        EndDate = model.EndDate,
                        Description = "Expo Ingeniería " + model.StartDate.Year,
                    };
                    if (await _context.Fair.AddAsync(newFair) != null)
                    {
                        await _context.SaveChangesAsync();
                        return newFair;
                    }
                }
                _logger.LogWarning("Error al crear una feria.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para eliminar ferias
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteFairAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Fair
                                    where x.Id == id
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    _context.Fair.Remove(result);
                    _context.SaveChanges();
                    return true;
                }

                _logger.LogWarning("Error al eliminar una feria.");
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener todas las ferias
        /// </summary>
        /// <returns></returns>
        public async Task<List<Fair>?> GetAllFairsAsync()
        {
            try
            {
                return await _context.Fair.ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener las ferias actuales
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetCurrentFairIdAsync()
        {
            try
            {
                var currentFair = await (from x in _context.Fair
                                         where x.StartDate.Year == DateTime.Now.Year
                                         select x.Id).FirstOrDefaultAsync();

                if(currentFair != 0)
                {
                    return currentFair;
                }
                return 0;

            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}
