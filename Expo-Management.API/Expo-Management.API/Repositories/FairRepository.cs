using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de ferias
    /// </summary>
    public class FairRepository : IFairRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor del repositorio de ferias
        /// </summary>
        /// <param name="context"></param>
        public FairRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Metodo para crear ferias
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Fair> CreateFairAsync(NewFair model)
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
                return null;
            }
            catch (Exception ex)
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

                return false;
            }
            catch (Exception ex)
            {

                return false;
            }
        }

        /// <summary>
        /// Metodo para obtener todas las ferias
        /// </summary>
        /// <returns></returns>
        public async Task<List<Fair>> GetAllFairsAsync()
        {
            try
            {
                return await _context.Fair.ToListAsync();

            }
            catch (Exception ex)
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

                if(currentFair != null)
                {
                    return currentFair;
                }
                return 0;

            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
