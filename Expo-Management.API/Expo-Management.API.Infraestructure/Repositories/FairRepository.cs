using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
        public async Task<Fair?> CreateFairAsync()
        {
            try
            {
                var model = DateTime.Now;
                var newFair = new Fair()
                {
                    StartDate = model,
                    EndDate = model.AddMonths(5),
                    Description = "Expo Ingeniería " + model.Year,
                };

                if(GetCurrentFairId() == 0 && newFair.EndDate.Year == DateTime.Now.Year)
                {

                    if (await _context.Fair.AddAsync(newFair) != null)
                    {
                        await _context.SaveChangesAsync();
                        return newFair;
                    }

                    _logger.LogWarning("Error al crear una feria.");
                    return null;
                }

                _logger.LogWarning("Ya existe una feria actual.");
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

                if (result != null && result.EndDate <= DateTime.Now)
                {
                    _context.Fair.Remove(result);
                    _context.SaveChanges();
                    return true;
                }

                _logger.LogWarning("Feria todavia no esta por acabar.");
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
        public int GetCurrentFairId()
        {
            try
            {
                var currentFair =  (from x in _context.Fair
                                         where x.StartDate.Year == DateTime.Now.Year
                                         select x.Id).FirstOrDefault();

                if (currentFair != 0)
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

        /// <summary>
        /// Metodo para obtener los dias restantes de la feria actual
        /// </summary>
        /// <returns></returns>
        public async Task<int> GetLeftDaysAsync()
        {
            try
            {
                var currentFair = await (from x in _context.Fair
                                         where x.StartDate.Year == DateTime.Now.Year
                                         select x).FirstOrDefaultAsync();

                if (currentFair != null)
                {
                    var Alldays = 0;
                    var useDays = 0;

                    for (int i = currentFair.StartDate.Month; i <= currentFair.EndDate.Month; i++)
                    {
                        Alldays += DateTime.DaysInMonth(currentFair.StartDate.Year, i);

                    }

                    for (int i = currentFair.StartDate.Month; i <= DateTime.Now.Month; i++)
                    {
                        useDays += DateTime.DaysInMonth(currentFair.StartDate.Year, i);
                    }

                    Alldays = Alldays - (DateTime.DaysInMonth(DateTime.Now.Year, currentFair.EndDate.Month) - currentFair.EndDate.Day);

                    useDays = useDays - (DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) - DateTime.Now.Day);

                    return Alldays - useDays;
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
