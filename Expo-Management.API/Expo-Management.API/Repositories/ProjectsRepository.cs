using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Entities.Projects;
using Expo_Management.API.Interfaces;
using Expo_Management.API.Services;
using Expo_Management.API.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{

    /// <summary>
    /// Repositorio de projectos
    /// </summary>
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesUploader;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUsersRepository _usersRepository;
        private CrudUtils _crudUtils;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor del repositorio de proyetos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filesUploader"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="usersRepository"></param>
        /// <param name="mailService"></param>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        public ProjectsRepository(
            ApplicationDbContext context,
            IFilesUploaderRepository filesUploader,
            ICategoryRepository categoryRepository,
            IUsersRepository usersRepository,
            IMailService mailService,
            UserManager<User> userManager,
        IConfiguration configuration)
        {
            _context = context;
            _filesUploader = filesUploader;
            _usersRepository = usersRepository;
            _categoryRepository = categoryRepository;
            _crudUtils = new CrudUtils(_usersRepository, _context);
            _mailService = mailService;
            _configuration = configuration;
            _userManager = userManager;
        }

        /// <summary>
        /// Metodo para crear un proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

                    var category = await (from x in _context.Categories
                                          where x.Id == model.Category
                                          select x).FirstOrDefaultAsync();

                    //create new project
                    ProjectModel newProject = new ProjectModel()
                    {
                        Files = upload,
                        Name = model.Name,
                        Fair = Fair,
                        Description = model.Description,
                        category = category
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
                return null;
            }

        }

        /// <summary>
        /// Metodo para obtener todos los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectModel>?> GetAllProjectsAsync()
        {
            try
            {
                return await (from p in _context.Projects
                              select p)
                              .Include(x => x.Files)
                              .Include(c => c.category)
                              .ToListAsync();

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener la feria
        /// </summary>
        /// <param name="fairId"></param>
        /// <returns></returns>
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
                return null;
            }
        }

        /// <summary>
        /// Metodo para verificar si el proyecto existe
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
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
                return false;
            }

        }

        /// <summary>
        /// Metodo para obtener las menciones
        /// </summary>
        /// <returns></returns>
        public async Task<List<Mention>> GetMentionsAsync()
        {
            try
            {
                var mentions = await (from m in _context.Mention
                                      select m).ToListAsync();

                if (mentions != null && mentions.Any())
                {
                    return mentions;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos actuales
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectModel>> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      where p.Fair.StartDate.Year == DateTime.Now.Year
                                      select p).ToListAsync();

                if (projects != null && projects.Any())
                {
                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos antiguos
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectModel>> GetOldProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      .Include(x => x.Fair)
                                      .Include(c => c.category)
                                      where p.Fair.StartDate.Year < DateTime.Now.Year
                                      select p).ToListAsync();

                if (projects != null && projects.Any())
                {
                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para crear los claims del proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Claim> CreateProjectClaim(NewClaim model)
        {
            var project = await (from p in _context.Projects
                                 where p.Id == model.ProjectId
                                 select p).FirstOrDefaultAsync();

            if (project == null)
            {
                return null;
            }

            var claim = new Claim()
            {
                ClaimDescription = model.ClaimDescription,
                Project = project
            };

            await _context.Claim.AddAsync(claim);
            await _context.SaveChangesAsync();
            return claim;
        }

        /// <summary>
        /// Metodo para obtener los detalles del proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<ProjectDetails>> GetProjectDetails(int projectId)
        {
            try
            {
                var category = await (from x in _context.Projects
                                      where x.Id == projectId
                                      select x.category.Description)
                                          .FirstOrDefaultAsync();

                var projects = await (from p in _context.Projects
                                      where p.Id == projectId
                                      select new ProjectDetails()
                                      {
                                          ProjectId = p.Id,
                                          ProjectName = p.Name,
                                          ProjectDescription = p.Description,
                                          Members = null,
                                          Category = category,
                                          ProjectQualifications = null,
                                          FinalPunctuation = null,
                                      }).ToListAsync();

                if (projects != null)
                {
                    List<string> members = await GetProjectMembers(projectId);
                    List<ProjectQualifications> qualifications = await GetProjectQualifications(projectId);

                    projects[0].Members = members;
                    projects[0].ProjectQualifications = qualifications;
                    projects[0].FinalPunctuation = CalculateProjectFinalPunctuation(qualifications).Result.ToString();

                    return projects;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }

            async Task<List<string>> GetProjectMembers(int projectId)
            {
                return await (from u in _context.User
                              join p in _context.Projects on u.Project.Id equals p.Id
                              where p.Id == projectId
                              select u.Name + " " + u.Lastname).ToListAsync();
            }

            async Task<int> CalculateProjectFinalPunctuation(List<ProjectQualifications> qualifications)
            {
                if (qualifications != null && qualifications.Count() > 0)
                {
                    var FinalQualification = 0;
                    var counter = 0;

                    foreach (var judgeQualification in qualifications)
                    {
                        FinalQualification = FinalQualification + judgeQualification.Punctuation;
                        counter++;
                    }

                    return FinalQualification / counter;
                }
                return 0;

            }
        }

        /// <summary>
        /// Metodo para obtener las recomendaciones del juez
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<JudgeRecommendation> JudgeRecommendation(NewRecommendation model)
        {
            try
            {
                //verifica el juez
                var juez = (from j in _context.User
                            where j.Email == model.correoJuez
                            select j).FirstOrDefault();

                var project = (from p in _context.Projects
                               where p.Id == model.IdProject
                               select p).FirstOrDefault();

                if (juez != null && project != null)
                {
                    var newRecommendation = new JudgeRecommendation()
                    {
                        project = project,
                        user = juez,
                        Recomendacion = model.Recommendation
                    };
                    await _context.JudgeRecommendation.AddAsync(newRecommendation);
                    await _context.SaveChangesAsync();

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

        /// <summary>
        /// Metodo para obtener las recomendaciones
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <returns></returns>
        public async Task<JudgeRecommendation> GetRecommendation(int recomendacion)
        {
            try
            {
                var recommendation = await (from r in _context.JudgeRecommendation
                                            where r.Id == recomendacion
                                            select r).FirstOrDefaultAsync();
                return recommendation;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener las recomendaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<JudgeRecommendation>> GetRecommendationByProjectId(int projectId)
        {
            try
            {
                var recommendations = await (from r in _context.JudgeRecommendation
                                             where r.project.Id == projectId
                                             select r)
                                            .Include(p => p.user)
                                            .ToListAsync();
                return recommendations;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener un proyecto en especifico
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo para enviar la calificacion por email
        /// </summary>
        /// <param name="project"></param>
        /// <param name="judge"></param>
        public async void SendCalificationsEmails(ProjectModel project, User judge)
        {

            var Leader = await (from l in _context.User
                                where l.Project.Id == project.Id && l.IsLead == true
                                select l).FirstOrDefaultAsync();


            //Email to student
            dynamic ToStudentEmailTemplate = new DynamicTemplate();

            await _mailService.SendEmailAsync(Leader.Email, "d-dac12791e045497b9d5a84dfa260f244", ToStudentEmailTemplate = new
            {
                student_username = Leader.UserName,
                project_name = project.Name,
                judge_name = judge.UserName,
                url = $"{_configuration["WebUrl"]}/student/project/{project.Id}"
            });

            //Email to judge
            dynamic ToJudgeEmailTemplate = new DynamicTemplate();

            await _mailService.SendEmailAsync(judge.Email, "d-3d3a74d6d191448e82fbb6d59de3a683", ToJudgeEmailTemplate = new
            {
                judge_name = judge.UserName,
                project_name = project.Name,
                url = $"{_configuration["WebUrl"]}/judges/project-qualify/{project.Id}"
            });
        }

        /// <summary>
        /// Metodo para calificar proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Qualifications> QualifyProject(QualifyProject model)
        {
            try
            {
                var judge = await (from u in _context.User
                                   where u.Email == model.JudgeEmail
                                   select u).FirstAsync();

                var project = await (from p in _context.Projects
                                     where p.Id == model.ProjectId
                                     select p).FirstAsync();

                if (project == null || judge == null)
                {
                    return null;
                }

                var qualification = new Qualifications()
                {
                    Punctuation = model.Punctuation,
                    Comments = model.Comments,
                    Judge = (User)judge,
                    Project = project
                };

                //send emails 
                SendCalificationsEmails(project, judge);

                await _context.Qualifications.AddAsync(qualification);
                await _context.SaveChangesAsync();

                return qualification;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los miembros de un proyecto
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectMembers>> GetMembers()
        {
            try
            {
                var members = await (from u in _context.User
                                     join p in _context.Projects on u.Project.Id equals p.Id
                                     select new ProjectMembers()
                                     {
                                         Name = u.Name,
                                         LastName = u.Lastname,
                                         Email = u.Email,
                                         PhoneNumber = u.PhoneNumber,
                                         ProjectId = p.Id,
                                         ProjectName = p.Name
                                     }).ToListAsync();

                if (members.Any())
                {
                    return members;
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los correos de los miembros de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<User>> GetMembersEmail(int projectId)
        {
            try
            {
                var emails = await (from u in _context.User
                                    where u.Project.Id == projectId
                                    select u)
                                    .Include(p => p.Project)
                                    .ToListAsync();

                return emails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Metodo para obtener las calificaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<ProjectQualifications>> GetProjectQualifications(int projectId)
        {

            try
            {
                return await (from p in _context.Projects
                              join q in _context.Qualifications on p.Id equals q.Project.Id
                              join u in _context.User on q.Judge.Id equals u.Id
                              where p.Id == projectId
                              select new ProjectQualifications()
                              {
                                  Punctuation = q.Punctuation,
                                  JudgeName = u.Name + " " + u.Lastname
                              }).ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos de cada año
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectQuantity>> GetProjectsByYear()
        {

            try
            {
                return await _context.Projects
                .GroupBy(x => x.Fair.StartDate.Year)
                .Select(x => new ProjectQuantity
                {
                    name = x.Select(x => x.Fair.Description).First(),
                    value = x.Count()
                }).ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener las categorias por los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectQuantity>> GetProjectsByCategory()
        {

            try
            {
                return await _context.Projects
                .GroupBy(x => x.category.Id)
                .Select(x => new ProjectQuantity
                {
                    name = x.Select(x => x.category.Description).First(),
                    value = x.Count()
                }).ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener las calificaciones por proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectQuantity>> GetProjectsByQualifications()
        {

            try
            {
                return await _context.Qualifications
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantity
                    {
                        name = x.Select(x => x.Project.Name).First(),
                        value = x.Count()
                    }).ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los los usuarios por los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectQuantity>> GetUsersByProject()
        {

            try
            {
                return await _context.User
                    .Where(x => x.Project.Name != null)
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantity
                    {
                        name = x.Select(x => x.Project.Name).First(),
                        value = x.Count()
                    }).ToListAsync();
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}