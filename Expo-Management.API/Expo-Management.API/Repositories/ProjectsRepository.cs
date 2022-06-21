using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesUploader;
        private readonly IUsersRepository _usersRepository;


        public ProjectsRepository(ApplicationDbContext context, IFilesUploaderRepository filesUploader, IUsersRepository usersRepository)
        {
            _context = context;
            _filesUploader = filesUploader;
            _usersRepository = usersRepository;
        }


        public async Task<ProjectModel> CreateProject(NewProject model)
        {
            try
            {
                var project = ProjectExists(model.Lider);


                if (!project)
                {

                    var lider = await  _usersRepository.GetStudentAsync(model.Lider);
                    var member2 = await _usersRepository.GetStudentAsync(model.Member2);
                    var member3 = await _usersRepository.GetStudentAsync(model.Member3);
                    var upload = await _filesUploader.AddProjectsFile(model.Files);
                    //var Fair = await GetFair(model.Name); traer id por feria

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Lider =  lider,
                        Member2 = member2,
                        Member3 = member3,
                        Files =  upload
             };

                    //add newProject to database
                    await _context.Projects.AddAsync(newProject);
                    await _context.SaveChangesAsync();

                    return null;
                }

                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                throw ex;
            }

        }

        public async Task<List<ProjectModel>> GetAllProjectsAsync()
        {
            try
            {
                return await _context.Projects.Include(x => x.Files).ToListAsync();

            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public async Task<Fair> GetFair(string project)
        {
            try
            {
                var result = await (from x in _context.Projects
                                   where x.Name == project
                                   select x.Fair).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }

        public async Task<List<string>> GetOldProjectsAsync()
        {
            try
            {
                var actualTime = DateTime.Today;

                var result = await (from x in _context.Projects
                                    where x.Fair.EndDate < actualTime
                                    select x.Name).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null ;
            }

        }

        public bool ProjectExists(string name)
        {
            try
            {
                var result = (from x in _context.Projects
                              where x.Name == name
                              select x.Name).FirstOrDefaultAsync();

                if (result == null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return false;
            }

        }

    }
 }