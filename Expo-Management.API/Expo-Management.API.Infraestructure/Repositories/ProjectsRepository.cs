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
using Expo_Management.API.Domain.Models.Reponses;

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
        public async Task<Response?> CreateProject(NewProjectInputModel model)
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
                            Fair = (Fair)Fair.Data,
                            Description = model.Description,
                            category = category,
                            oldMembers = ""
                        };

                        var projectCreated = await _context.Projects.AddAsync(newProject);
                        await _context.SaveChangesAsync();

                        //add newProject to database

                        var ProjectCreated = await _crudUtils.addUsersToProject(groupOfUserEmails, newProject);

                        return new Response()
                        {
                            Status = 200,
                            Data = newProject,
                            Message = "Proyectos creado exitosamente!"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Status = 204,
                            Message = "Categoria y/o feria no encontrados."
                        };
                    }
                }
                return new Response()
                {
                    Status = 400,
                    Message = "Revise los datos enviados"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener todos los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetAllProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      select p)
                              .Include(x => x.Files)
                              .Include(c => c.category)
                              .ToListAsync();

                if (projects != null)
                {

                    var domainProjects = new List<Project>();

                    foreach (var items in projects)
                    {
                        domainProjects.Add(new Project()
                        {
                            Id = items.Id,
                            Name = items.Name,
                            Description = items.Description,
                            Fair = items.Fair,
                            oldMembers = items.oldMembers,
                            Files = new Files()
                            {
                                Id = items.Files.Id,
                                Name = items.Files.Name,
                                Size = items.Files.Size,
                                Url = items.Files.Url,
                                uploadDateTime = items.Files.uploadDateTime
                            },
                            category = new Category()
                            {
                                Id = items.category.Id,
                                Description = items.category.Description
                            }
                        });

                        return new Response()
                        {
                            Status = 200,
                            Data = domainProjects,
                            Message = "Proyectos encontrados exitosamente!"
                        };
                    }
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener la feria
        /// </summary>
        /// <param name="fairId"></param>
        /// <returns></returns>
        public async Task<Response?> GetFair(int fairId)
        {
            try
            {
                var result = await (from f in _context.Fair
                                    where f.Id == fairId
                                    select f).FirstOrDefaultAsync();

                if (result != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = result,
                        Message = "Feria encontrada exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Feria no encontrada."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
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
        public async Task<Response?> removeUserFromProject(string email)
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
                    return new Response()
                    {
                        Status = 200,
                        Data = removedUser,
                        Message = "Usuario removido del proyecto exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuario no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos actuales
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetAllCurrentProjectsAsync()
        {
            try
            {
                var projects = await (from p in _context.Projects
                                      where p.Fair.StartDate.Year == DateTime.Now.Year
                                      select p)
                              .Include(x => x.Files)
                              .Include(c => c.category)
                              .Distinct()
                              .ToListAsync();


                var domainProjects = new List<Project>();


                foreach (var items in projects)
                {
                    domainProjects.Add(new Project()
                    {
                        Id = items.Id,
                        Name = items.Name,
                        Description = items.Description,
                        Fair = items.Fair,
                        oldMembers = items.oldMembers,
                        Files = new Files()
                        {
                            Id = items.Files.Id,
                            Name = items.Files.Name,
                            Size = items.Files.Size,
                            Url = items.Files.Url,
                            uploadDateTime = items.Files.uploadDateTime
                        },
                        category = new Category()
                        {
                            Id = items.category.Id,
                            Description = items.category.Description
                        }
                    });
                }
                if (domainProjects.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = domainProjects,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos antiguos
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetOldProjectsAsync()
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
                    return new Response()
                    {
                        Status = 200,
                        Data = projects,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para crear los claims del proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response> CreateProjectClaim(NewClaimInputModel model)
        {
            try
            {
                var project = await (from p in _context.Projects
                                     where p.Id == model.ProjectId
                                     select p).FirstOrDefaultAsync();

                if (project == null)
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Proyecto no encontrado."
                    };
                }

                var claim = new Claim()
                {
                    ClaimDescription = model.ClaimDescription,
                    Project = project
                };

                await _context.Claim.AddAsync(claim);
                await _context.SaveChangesAsync();
                return new Response()
                {
                    Status = 200,
                    Data = claim,
                    Message = "Reclamo creado exitosamente!"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los detalles del proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Response?> GetProjectDetails(int projectId)
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
                    var qualifications = await GetProjectQualifications(projectId);

                    projects[0].Members = members;
                    projects[0].ProjectQualifications = (List<ProjectQualificationsViewModel>?)qualifications.Data;
                    projects[0].FinalPunctuation = CalculateProjectFinalPunctuation((List<ProjectQualificationsViewModel>)qualifications.Data).Result.ToString();

                    return new Response()
                    {
                        Status = 200,
                        Data = projects,
                        Message = "Detalles del proyecto encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyecto no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }

            async Task<List<string>> GetProjectMembers(int projectId)
            {
                return await (from u in _context.User
                              join p in _context.Projects on u.Project.Id equals p.Id
                              where p.Id == projectId
                              select u.Name + " " + u.Lastname).ToListAsync();
            }

            async Task<int> CalculateProjectFinalPunctuation(List<ProjectQualificationsViewModel> qualifications)
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
        public async Task<Response?> JudgeRecommendation(NewRecommendationInputModel model)
        {
            try
            {
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

                    return new Response()
                    {
                        Status = 200,
                        Data = newRecommendation,
                        Message = "Recomendación creada exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 400,
                        Message = "Revise los datos enviados"
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener las recomendaciones
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <returns></returns>
        public async Task<Response?> GetRecommendation(int recomendacion)
        {
            try
            {
                var recommendation = await (from r in _context.JudgeRecommendation
                                            where r.Id == recomendacion
                                            select r).FirstOrDefaultAsync();
                if (recommendation != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = recommendation,
                        Message = "Recomendación encontrada exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Recomendación no encontrada."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Obtener las recomendaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Response?> GetRecommendationByProjectId(int projectId)
        {
            try
            {
                var recommendations = await (from r in _context.JudgeRecommendation
                                             where r.project.Id == projectId
                                             select r)
                                            .Include(p => p.user)
                                            .ToListAsync();
                if (recommendations.Count() > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = recommendations,
                        Message = "Recomendaciones encontradas exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Recomendaciones no encontradas."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener un proyecto en especifico
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <returns></returns>
        public async Task<Response?> GetProjectById(int ProjectId)
        {
            try
            {
                var project = await (from p in _context.Projects
                                     where p.Id == ProjectId
                                     select p).FirstAsync();
                if (project != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = project,
                        Message = "Proyecto encontrado exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyecto no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para enviar la calificacion por email
        /// </summary>
        /// <param name="project"></param>
        /// <param name="judge"></param>
        public async void SendCalificationsEmails(Project project, User judge)
        {
            try
            {
                var Leader = await (from l in _context.User
                                    where l.Project.Id == project.Id && l.IsLead == true
                                    select l).FirstOrDefaultAsync();

                var userTwo = await (from l in _context.User
                                     where l.Project.Id == project.Id && l.IsLead == false
                                     select l).FirstOrDefaultAsync();


                if (Leader != null)
                {
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
                else
                {
                    //Email to students
                    dynamic ToStudentEmailTemplate = new DynamicTemplate();

                    await _mailService.SendEmailAsync(userTwo.Email, "d-dac12791e045497b9d5a84dfa260f244", ToStudentEmailTemplate = new
                    {
                        studentTwo_username = userTwo.UserName,
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        /// <summary>
        /// Metodo para saber si un juez puede calificar o no un proyecto
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="JudgeEmail"></param>
        public async Task<Boolean> CanJudgeQualifyTheProject(int ProjectId, string JudgeEmail)
        {
            try
            {
                var judge = await _usersRepository.GetJudgeAsync(JudgeEmail);
                var project = await GetProjectById(ProjectId);

                var haveJudgeQualifiedTheProject = await (from q in _context.Qualifications
                                                          where q.Project.Id == ProjectId && q.Judge.Email == JudgeEmail
                                                          select q).FirstOrDefaultAsync();

                if (project == null || judge == null)
                {
                    _logger.LogWarning("Hubo un error obteniendo el juez o el proyecto.");
                    return false;
                }

                if (haveJudgeQualifiedTheProject != null)
                {
                    _logger.LogWarning("El proyecto ya fue calificado por el juez con email: " + JudgeEmail);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                _logger.LogWarning("Hubo un error validando si el juez puede calificar un proyecto.");
                return false;
            }
        }

        /// <summary>
        /// Metodo para calificar proyecto
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> QualifyProject(QualifyProjectInputModel model)
        {
            try
            {
                var judge = (from u in _context.User
                             where u.Email == model.JudgeEmail
                             select u).FirstOrDefault();

                var project = (from p in _context.Projects
                               where p.Id == model.ProjectId
                               select p).FirstOrDefault();

                if (project != null && judge != null)
                {
                    var qualification = new Qualifications()
                    {
                        Punctuation = model.Punctuation,
                        Judge = judge,
                        Project = project
                    };

                    SendCalificationsEmails(project, judge);

                    await _context.Qualifications.AddAsync(qualification);
                    await _context.SaveChangesAsync();

                    return new Response()
                    {
                        Status = 200,
                        Data = qualification,
                        Message = "Calificación realizada exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Proyecto y/o usuario juez no encontrados."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los miembros de un proyecto
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetMembers()
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
                    return new Response()
                    {
                        Status = 200,
                        Data = members,
                        Message = "Miembros del proyecto encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuarios no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los correos de los miembros de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Response> GetMembersEmail(int projectId)
        {
            try
            {
                var emails = await (from u in _context.User
                                    where u.Project.Id == projectId
                                    select u)
                                    .Include(p => p.Project)
                                    .ToListAsync();

                if (emails != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = emails,
                        Message = "Correos de los usuarios encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuarios no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener las calificaciones de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<Response?> GetProjectQualifications(int projectId)
        {

            try
            {
                var projectsByQualifications = await (from p in _context.Projects
                                                      join q in _context.Qualifications on p.Id equals q.Project.Id
                                                      join u in _context.User on q.Judge.Id equals u.Id
                                                      where p.Id == projectId
                                                      select new ProjectQualificationsViewModel()
                                                      {
                                                          Punctuation = q.Punctuation,
                                                          JudgeName = u.Name + " " + u.Lastname
                                                      }).ToListAsync();

                if (projectsByQualifications.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = projectsByQualifications,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los proyectos de cada año
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetProjectsByYear()
        {

            try
            {
                var projectsByYear = await _context.Projects
                .GroupBy(x => x.Fair.StartDate.Year)
                .Select(x => new ProjectQuantityViewModel
                {
                    name = x.Select(x => x.Fair.Description).First(),
                    value = x.Count()
                }).ToListAsync();

                if (projectsByYear != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = projectsByYear,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener las categorias por los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetProjectsByCategory()
        {

            try
            {
                var projectsByCategory = await _context.Projects
                .GroupBy(x => x.category.Id)
                .Select(x => new ProjectQuantityViewModel
                {
                    name = x.Select(x => x.category.Description).First(),
                    value = x.Count()
                }).ToListAsync();

                if (projectsByCategory != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = projectsByCategory,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener las calificaciones por proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetProjectsByQualifications()
        {

            try
            {
                var projects = await _context.Qualifications
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantityViewModel
                    {
                        name = x.Select(x => x.Project.Name).First(),
                        value = x.Count()
                    }).ToListAsync();

                if (projects != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = projects,
                        Message = "Proyectos encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Proyectos no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener los los usuarios por los proyectos
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetUsersByProject()
        {
            try
            {
                var users = await _context.User
                    .Where(x => x.Project.Name != null)
                    .GroupBy(x => x.Project.Id)
                    .Select(x => new ProjectQuantityViewModel
                    {
                        name = x.Select(x => x.Project.Name).First(),
                        value = x.Count()
                    }).ToListAsync();

                if (users != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = users,
                        Message = "Usuarios encontrados exitosamente!"
                    };
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuarios no encontrados."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }
    }
}