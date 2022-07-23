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
            _crudUtils = new CrudUtils(_usersRepository, _context);
        }


        public async Task<ProjectModel> CreateProject(NewProject model)
        {
            try
            {
                List<string> groupOfUserEmails = new List<string>();

                groupOfUserEmails.Add(model.Lider);
                groupOfUserEmails.Add(model.Member2);
                groupOfUserEmails.Add(model.Member3);

                var membersOfTheGroup = await this._crudUtils.getUsersAvailableAsync(groupOfUserEmails);
                var project = ProjectExists(model.Name);
                
                if (membersOfTheGroup != null && !project)
                {
                    var upload = await _filesUploader.AddProjectsFile(model.Files);
                    var Fair = await GetFair(model.Fair);

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Files =  upload,
                        Fair = Fair
                    };

                    var projectCreated = await _context.Projects.AddAsync(newProject);
                    await _context.SaveChangesAsync();

                    //add newProject to database

                    var ProjectCreated = await _crudUtils.addUsersToProject(groupOfUserEmails, newProject);

                    return newProject;
                    
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

       

        public bool ProjectExists(string name)
        {
            try
            {
                var result = (from x in _context.Projects
                              where x.Name == name
                              select x.Name).FirstOrDefault();

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

        public async Task<List<ProjectModel>> GetOldProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      where p.Fair.StartDate.Year < DateTime.Now.Year
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

        public async Task<JudgeRecommendation> JudgeRecommendation(NewRecommendation model)
        {

            try
            {
                //verifica el juez
                var juez = await _usersRepository.GetJudgeAsync(model.correoJuez);
                var project = await GetProjectById(model.IdProject);

                if (juez != null && project != null)
                {
                    var newRecommendation = new JudgeRecommendation()
                    {

                        project = project,
                        user = juez,
                        Recomendacion = model.Recommendation
                    };

                    await _context.AddAsync<JudgeRecommendation>(newRecommendation);
                    _context.SaveChangesAsync();

                    return newRecommendation;

                }

                return null;     

            }
            catch (Exception ex)
            {

                throw ex;
            }                      

            //se agrega a la base de datos
           
        }

        public async Task<JudgeRecommendation> GetRecommendation(int recomendacion)
        {
            try
            {

               // var project = await GetProjectById(model.IdProject);
               // var student = await _usersRepository.GetStudentAsync(model.correoEstudiante);

               // if (student != null && project != null)
               // {
                    //    var recomendation = await (from j in _context.NewRecommendation
                    //                               where j.Recommendation == model.Recommendation
                    //                               select j);
                    var recommendation = await (from r in _context.JudgeRecommendation
                                                where r.Id == recomendacion
                                                select r).FirstOrDefaultAsync();   
                        return recommendation;
                    
             //   }
                
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProjectModel> GetProjectById(int ProjectId)
        {
            try
            {
                var project = await (from p in _context.Projects
                                     where p.Id == ProjectId
                                     select p).FirstAsync();

                if (project != null)
                {
                    return project;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
 }