using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.ViewModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Infraestructure.Services;
using Expo_Management.API.Infraestructure.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de proyectos
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
        private readonly ILogger<ProjectsRepository> _logger;

        /// <summary>
        /// Constructor del repositorio de proyectos
        /// </summary>
        /// <param name="context"></param>
        /// <param name="filesUploader"></param>
        /// <param name="categoryRepository"></param>
        /// <param name="usersRepository"></param>
        /// <param name="mailService"></param>
        /// <param name="userManager"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public ProjectsRepository(
            ApplicationDbContext context,
            IFilesUploaderRepository filesUploader,
            ICategoryRepository categoryRepository,
            IUsersRepository usersRepository,
            IMailService mailService,
            UserManager<User> userManager,
            IConfiguration configuration,
            ILogger<ProjectsRepository> logger
            )
        {
            _context = context;
            _filesUploader = filesUploader;
            _usersRepository = usersRepository;
            _categoryRepository = categoryRepository;
            _crudUtils = new CrudUtils(_usersRepository, _context);
            _mailService = mailService;
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para crear un proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Project?> CreateProject(NewProjectInputModel model)
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
                    var upload = _filesUploader.AddProjectsFile(model.Files);
                    var Fair = await GetFair(model.Fair);

                    var category = await (from x in _context.Categories
                                          where x.Id == model.Category
                                          select x).FirstOrDefaultAsync();

                    if (category != null && Fair != null)
                    {
                        //create new project
                        Project newProject = new Project()
                        {
                            Files = upload,
                            Name = model.Name,
                            Fair = Fair,
                            Description = model.Description,
                            category = category,
                            oldMembers = ""
                        };

                        var projectCreated = await _context.Projects.AddAsync(newProject);
                        await _context.SaveChangesAsync();

                        //add newProject to database

                        var ProjectCreated = await _crudUtils.addUsersToProject(groupOfUserEmails, newProject);

                        return newProject;
                    }
                    else
                    {
                        _logger.LogWarning("No se puede obtener la categoria o la feria.");
                        return null;
                    }
                }
                _logger.LogWarning("Error al crear un proyecto.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        /// <summary>
        /// Metodo para obtener todos los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>?> GetAllProjectsAsync()
        {
            try
            {
                return await (from p in _context.Projects
                              select p)
                              .Include(x => x.Files)
                              .Include(c => c.category)
                              .ToListAsync();

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener la feria
        /// </summary>
        /// <param name="fairId"></param>
        /// <returns></returns>
        public async Task<Fair?> GetFair(int fairId)
        {
            try
            {
                var result = await (from f in _context.Fair
                                    where f.Id == fairId
                                    select f).FirstOrDefaultAsync();

                return result;
            }
            catch (Exception)
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
            catch (Exception)
            {
                return false;
            }

        }

        /// <summary>
        /// Metodo para borrar estudiantes de un proyecto
        /// excepto el lider del proyecto
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User?> removeUserFromProject(string email)
        {
            try
            {
                var removedUser = await (from u in _context.User
                                         where u.Email == email
                                         select u)
                                         .Include(p => p.Project)
                                         .FirstOrDefaultAsync();

                if (removedUser != null)
                {
                    removedUser.Project.oldMembers = String.Concat(removedUser.Project.oldMembers, " " + removedUser.UserName);
                    removedUser.Project = null;
                    _context.SaveChanges();
                    return removedUser;
                }

                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }


        ///// <summary>
        ///// Metodo para verificar si el proyecto existe
        ///// </summary>
        ///// <param name="email"></param>
        ///// <returns></returns>
        //public async Task<Project?> removeProject(string email)
        //{
        //    try
        //    {
        //        var removedUser = await (from u in _context.User
        //                                 where u.Email == email
        //                                 select u)
        //                                 .Include(p => p.Project)
        //                                 .FirstOrDefaultAsync();

        //        if (removedUser != null)
        //        {

        //            removedUser.Project.oldMembers = String.Concat(removedUser.Project.oldMembers, ", " + removedUser.UserName);
        //            Project oldProject = removedUser.Project;
        //            removedUser.Project = null;
        //            _context.SaveChanges();
        //            return oldProject;
        //        }

        //        return null;
        //    }
        //    catch (Exception)
        //    {

        //        return null;
        //    }
        //}


        /// <summary>
        /// Metodo para obtener las menciones
        /// </summary>
        /// <returns></returns>
        public async Task<List<Mention>?> GetMentionsAsync()
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos actuales
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>?> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects =  await (from p in _context.Projects
                              join u in _context.User on p.Id equals u.Project.Id
                              where p.Fair.StartDate.Year == DateTime.Now.Year && u.Project != null
                              select p)
                              .Include(x => x.Files)
                              .Include(c => c.category)
                              .ToListAsync();

                return projects.Distinct().ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos antiguos
        /// </summary>
        /// <returns></returns>
        public async Task<List<Project>?> GetOldProjectsAsync()
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para crear los claims del proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Claim?> CreateProjectClaim(NewClaimInputModel model)
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
        public async Task<List<ProjectDetailsViewModel>?> GetProjectDetails(int projectId)
        {
            try
            {
                var category = await (from x in _context.Projects
                                      where x.Id == projectId
                                      select x.category.Description)
                                          .FirstOrDefaultAsync();

                var projects = await (from p in _context.Projects
                                      where p.Id == projectId
                                      select new ProjectDetailsViewModel()
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
                    List<string>? members = await GetProjectMembers(projectId);
                    List<ProjectQualificationsInputModel>? qualifications = await GetProjectQualifications(projectId);

                    projects[0].Members = members;
                    projects[0].ProjectQualifications = qualifications;
                    projects[0].FinalPunctuation = CalculateProjectFinalPunctuation(qualifications).Result.ToString();

                    return projects;
                }
                return null;
            }
            catch (Exception)
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

            async Task<int> CalculateProjectFinalPunctuation(List<ProjectQualificationsInputModel> qualifications)
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

                    return await Task.FromResult(FinalQualification / counter);
                }
                return 0;
            }
        }

        /// <summary>
        /// Metodo para obtener las recomendaciones del juez
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Recommendation?> JudgeRecommendation(NewRecommendationInputModel model)
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
                    var newRecommendation = new Recommendation()
                    {
                        project = project,
                        user = juez,
                        Recomendacion = model.Recommendation
                    };
                    await _context.JudgeRecommendation.AddAsync(newRecommendation);
                    await _context.SaveChangesAsync();

                    return newRecommendation;
                }
                else
                {
                    _logger.LogWarning("Error al crear una recomendación para el proyecto.");
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
            //se agrega a la base de datos

        }

        /// <summary>
        /// Metodo para obtener las recomendaciones
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <returns></returns>
        public async Task<Recommendation?> GetRecommendation(int recomendacion)
        {
            try
            {
                var recommendation = await (from r in _context.JudgeRecommendation
                                            where r.Id == recomendacion
                                            select r).FirstOrDefaultAsync();
                return recommendation;

            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Obtener las recomendaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<Recommendation>?> GetRecommendationByProjectId(int projectId)
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener un proyecto en especifico
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public async Task<Project?> GetProjectById(int ProjectId)
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para enviar la calificacion por email
        /// </summary>
        /// <param name="project"></param>
        /// <param name="judge"></param>
        public async void SendCalificationsEmails(Project project, User judge)
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
        public async Task<Qualifications?> QualifyProject(QualifyProjectInputModel model)
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
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener los miembros de un proyecto
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProjectMembersViewModels>?> GetMembers()
        {
            try
            {
                var members = await (from u in _context.User
                                     join p in _context.Projects on u.Project.Id equals p.Id
                                     select new ProjectMembersViewModels()
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
            catch (Exception)
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
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Metodo para obtener las calificaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<ProjectQualificationsInputModel>?> GetProjectQualifications(int projectId)
        {

            try
            {
                return await (from p in _context.Projects
                              join q in _context.Qualifications on p.Id equals q.Project.Id
                              join u in _context.User on q.Judge.Id equals u.Id
                              where p.Id == projectId
                              select new ProjectQualificationsInputModel()
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
        public async Task<List<ProjectQuantityInputModel>?> GetProjectsByYear()
        {

            try
            {
                return await _context.Projects
                .GroupBy(x => x.Fair.StartDate.Year)
                .Select(x => new ProjectQuantityInputModel
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
        public async Task<List<ProjectQuantityInputModel>?> GetProjectsByCategory()
        {

            try
            {
                return await _context.Projects
                .GroupBy(x => x.category.Id)
                .Select(x => new ProjectQuantityInputModel
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
        public async Task<List<ProjectQuantityInputModel>?> GetProjectsByQualifications()
        {

            try
            {
                return await _context.Qualifications
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantityInputModel
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
        public async Task<List<ProjectQuantityInputModel>?> GetUsersByProject()
        {

            try
            {
                return await _context.User
                    .Where(x => x.Project.Name != null)
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantityInputModel
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