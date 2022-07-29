using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class FairRepository : IFairRepository
    {
        private readonly ApplicationDbContext _context;

        public FairRepository(ApplicationDbContext context)
        {
            _context = context;
        }

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
                    if (_context.Fair.Add(newFair) != null)
                    {
                        _context.SaveChanges();
                        return newFair;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public Task<Fair> DeleteFairAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Fair>> GetAllFairsAsync()
        {
            try
            {
                return await _context.Fair.ToListAsync();

            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

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
                _context.Dispose();
                return 0;
            }
        }
    }
}
