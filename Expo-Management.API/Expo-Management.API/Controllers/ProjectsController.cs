using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadFiles.Controllers;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador para proyectos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : ControllerBase
    {

        private readonly IProjectsRepository _projectsRepository;
        private readonly ILogger<ProjectsController> _logger;


        /// <summary>
        /// Constructor del controlador de proyectos
        /// </summary>
        /// <param name="projectsRepository"></param>
        /// <param name="logger"></param>
        public ProjectsController(IProjectsRepository projectsRepository, ILogger<ProjectsController> logger)
        {
            _projectsRepository = projectsRepository;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para añadir proyectos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("projects")]
        public async Task<IActionResult> AddProjects([FromForm] NewProjectInputModel model)
        {
            try
            {
                var project = await _projectsRepository.CreateProject(model);

                if (project == null)
                {
                    return BadRequest("Por favor revisar los detalles del proyecto.");

                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para mostrar proyectos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("projects")]
        public async Task<IActionResult> ShowProjects()
        {
            try
            {
                var projects = await _projectsRepository.GetAllCurrentProjectsAsync();

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
                    }
                    return Ok(domainProjects);
                }
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para eliminar a un usuario de su grupo de proyecto de Feria
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("remove-user-project")]
        public async Task<IActionResult> removeUserFromProject(string email)
        {
            try
            {
                if (email != null)
                {
                    var removedUser = await _projectsRepository.removeUserFromProject(email);
                    if (removedUser != null)
                    {
                        return Ok(removedUser);
                    }
                    return BadRequest("usuario es Lider del proyecto o no existe");

                }
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        ///// <summary>
        ///// Endpoint para eliminar al lider de proyecto y con eso eliminar proyecto
        ///// </summary>
        ///// <returns></returns>
        //[HttpPut]
        //[Route("remove-project")]
        //public async Task<IActionResult> removeProject(string email)
        //{
        //    try
        //    {
        //        if (email != null)
        //        {
        //            var removedProject = await _projectsRepository.removeProject(email);
        //            if (removedProject != null)
        //            {
        //                return Ok(removedProject);
        //            }
        //            return BadRequest("Proyecto no existe");

        //        }
        //        return BadRequest("Hubo un error, por favor, intentelo más tarde.");
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}

        /// <summary>
        /// Endpoint para mostrar los proyectos antiguos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("old-projects")]
        public async Task<IActionResult> showOldProjects()
        {
            try
            {

                var projects = await _projectsRepository.GetOldProjectsAsync();
                if (projects != null)
                {
                    return Ok(projects);
                }
                return BadRequest("No hay proyectos antiguos");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para mostrar las menciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("mentions")]
        public async Task<IActionResult> showMentions()
        {
            try
            {

                var mentions = await _projectsRepository.GetMentionsAsync();
                if (mentions != null)
                {
                    return Ok(mentions);
                }
                return NotFound("Aún no hay menciones creadas.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener las calificaciones de los proyectos
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("project")]
        public async Task<IActionResult> getProjectQualificationAsync(int projectId)
        {
            try
            {

                var projectDetails = await _projectsRepository.GetProjectDetails(projectId);

                return Ok(projectDetails);
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para crear los reclamos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-claim")]
        public async Task<IActionResult> CreateProjectClaim(NewClaimInputModel model)
        {
            try
            {

                var claim = await _projectsRepository.CreateProjectClaim(model);

                if (claim != null)
                {
                    return Ok(claim);
                }
                return BadRequest("Id del proyecto o detalles icorrectos");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para crear las recomendaciones
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("recommendation")]
        //[Authorize(Roles = "Judge")]
        public async Task<IActionResult> postRecommendation([FromBody] NewRecommendationInputModel model)
        {
            try
            {
                var recommendation = await _projectsRepository.JudgeRecommendation(model);
                if (recommendation != null)
                {
                    return Ok(recommendation);
                }
                return BadRequest("Hubo un error interno, por favor intentelo mas tarde");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener las recomendaciones
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRecommendation")]
        public async Task<IActionResult> getRecommendation(int recomendacion)
        {
            try
            {
                var recommendation = await _projectsRepository.GetRecommendation(recomendacion);
                if (recommendation != null)
                {
                    return Ok(recommendation);
                }
                return BadRequest("Hubo un error interno, por favor intentelo mas tarde");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los miembros del proyecto
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("project-members")]
        public async Task<IActionResult> GetProjectMembers()
        {
            try
            {
                var members = await _projectsRepository.GetMembers();

                if (members != null)
                {
                    return Ok(members);
                }
                else
                {
                    return BadRequest("Algo salio mal");
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los correos de los miembros de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("members-emails")]
        public async Task<IActionResult> GetMembersEmail(int projectId)
        {
            try
            {
                var emails = await _projectsRepository.GetMembersEmail(projectId);

                if (emails != null)
                {
                    return Ok(emails);
                }
                else
                {
                    return BadRequest("Id del proyecto o detalles incorrectos");
                }

            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener la recomendacion de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("recommendation-by-project")]
        public async Task<IActionResult> getRecommendationByProjectId(int projectId)
        {
            try
            {
                var recommendations = await _projectsRepository.GetRecommendationByProjectId(projectId);

                if (recommendations != null)
                {
                    if (recommendations.Any())
                    {
                        return Ok(recommendations);
                    }
                    return BadRequest("No se encontrarion recomendaciones.");
                }
                return BadRequest("No hay recomendaciones para el proyecto.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para calificar proyectos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("qualify-project")]
        public async Task<IActionResult> qualifyProject(QualifyProjectInputModel model)
        {
            try
            {
                var result = await _projectsRepository.QualifyProject(model);

                if (result != null)
                {
                    return Ok(result);
                }
                return BadRequest("Los datos ingresados son incorrectos.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener las calificaciones del proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("project-qualifications")]
        public async Task<IActionResult> GetProjectQualifications(int projectId)
        {
            try
            {
                var qualifications = await _projectsRepository.GetProjectQualifications(projectId);

                if (qualifications != null)
                {
                    if (qualifications.Any())
                    {
                        return Ok(qualifications);
                    }
                    return BadRequest("No se encontraron calificaciones.");
                }
                return BadRequest("No hay calificaciones para el proyecto.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por año
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("project-by-year")]
        public async Task<IActionResult> GetProjectsByYear()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByYear();

                if (projects != null)
                {
                    if (projects.Any())
                    {
                        return Ok(projects);
                    }
                    return BadRequest("No se encontro el proyecto.");
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por categoria
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("project-by-category")]
        public async Task<IActionResult> GetProjectsByCategory()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByCategory();

                if (projects != null)
                {
                    if (projects.Any())
                    {
                        return Ok(projects);
                    }
                    return BadRequest("No se encontro el proyecto.");
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por las calificaciones
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("project-by-qualifications")]
        public async Task<IActionResult> GetProjectsByQualifications()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByQualifications();

                if (projects != null)
                {
                    if (projects.Any())
                    {
                        return Ok(projects);
                    }
                    return BadRequest("No se encontro del proyecto.");
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para obtener los usurios por los proyectos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("users-per-project")]
        public async Task<IActionResult> GetUsersPerProject()
        {
            try
            {
                var projects = await _projectsRepository.GetUsersByProject();

                if (projects != null)
                {
                    if (projects.Any())
                    {
                        return Ok(projects);
                    }
                    return BadRequest("No se encontro el proyecto.");
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}
