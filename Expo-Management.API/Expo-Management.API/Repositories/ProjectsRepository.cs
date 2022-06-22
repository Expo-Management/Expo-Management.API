using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Utils;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class ProjectsRepository : IProjectsRepository
    {

        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesUploader;
        private readonly IUsersRepository _usersRepository;
        private CrudUtils _crudUtils;


        public ProjectsRepository(ApplicationDbContext context, IFilesUploaderRepository filesUploader, IUsersRepository usersRepository)
        {
            _context = context;
            _filesUploader = filesUploader;
            _usersRepository = usersRepository;
            _crudUtils = new CrudUtils(_usersRepository);
        }


        public async Task<ProjectModel> CreateProject(NewProject model)
        {
            try
            {
                List<string> groupOfUserEmails = new List<string>();

                groupOfUserEmails.Add(model.Lider);
                groupOfUserEmails.Add(model.Member2);
                groupOfUserEmails.Add(model.Member3);

                var membersOfTheGroup = this._crudUtils.getUsersAvailable(groupOfUserEmails);
                var project = ProjectExists(model.Name);
                
                if (membersOfTheGroup != null && !project)
                {
                    var upload = await _filesUploader.AddProjectsFile(model.Files);
                    var Fair = await GetFair(model.Fair); //traer id por feria

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Files =  upload,
                        Fair = Fair
                    };

                    //add newProject to database
                    if (await _context.Projects.AddAsync(newProject) != null)
                    {
                        await _context.SaveChangesAsync();

                        /*Notas: 
                            1. Obtener proyecto de la base de datos recien creado
                            2. Crear funcion en Crud utils o otro modulo que vea necesario para asignar ese proyecto al usuario.
                            3. Guardar los cambios
                        */ 
                        return newProject;
                    }
                    else 
                    {
                        return null;
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

        public async Task<Fair> GetFair(int fairId)
        {
            try
            {
                var result = await (from f in _context.Fair
                                   where f.Id == fairId
                                   select f).FirstOrDefaultAsync();

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

            /*try
            {
                var actualTime = DateTime.Today;

                var result = await (from x in _context.Projects
                                    where x.Fair.EndDate < actualTime
                                    select x.Name).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                _context.Dispose();*/
                return null ;
            //}

        }

        public bool ProjectExists(string name)
        {
            try
            {
                var result = (from x in _context.Projects
                              where x.Name == name
                              select x.Name).FirstOrDefaultAsync();

                if (result != null)
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

        public async Task<List<Mention>> GetMentionsAsync() 
        {
            try
            {
                var mentions = await (from m in _context.Mention
                                      select m).ToListAsync();

                if (mentions != null && mentions.Count > 0)
                {
                    return mentions;
                }
                return null;
            }
            catch (Exception ex)
            {
               _context.Dispose();
                return null;
            }
        }

        public async Task<List<ProjectModel>> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      where p.Fair.StartDate.Year == DateTime.Now.Year
                                      select p).ToListAsync();

                if (projects != null && projects.Count > 0)
                {
                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                _context.Dispose();
                return null;
            }
        }
    }
 }