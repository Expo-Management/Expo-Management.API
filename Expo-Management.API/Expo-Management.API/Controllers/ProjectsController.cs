using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Projects;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        public ProjectsController(IProjectsRepository projectsRepository)
        {
            _projectsRepository = projectsRepository;
        }

        [HttpPost]
        [Route("projects")]
        public async Task<IActionResult> AddProjects([FromForm] NewProject model)
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
                throw ex;
            }
        }

        [HttpGet]
        [Route("projects")]
        public async Task<IActionResult> ShowProjects()
        {
            try
            {
                var projects = await _projectsRepository.GetAllProjectsAsync();

                if (projects != null)
                {
                    var domainProjects = new List<ProjectModel>();

                    foreach (var items in projects)
                    {
                        domainProjects.Add(new ProjectModel()
                        {
                            Id = items.Id,
                            Name = items.Name,
                            Description = items.Description,
                            Fair = items.Fair,
                            Files = new FilesModel()
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
            catch (Exception ex)
            {

                throw ex;
            }
        }

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
            catch (Exception ex)
            {

                throw ex;
            }
        }

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
                return NotFound("No hay menciones creadas aun.");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpGet]
        [Route("project")]
        public async Task<IActionResult> getProjectQualificationAsync(int projectId)
        {
            try
            {

                var projectDetails = await _projectsRepository.GetProjectDetails(projectId);

                return Ok(projectDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("create-claim")]
        public async Task<IActionResult> CreateProjectClaim(NewClaim model)
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
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("recommendation")]
        //[Authorize(Roles = "Judge")]
        public async Task<IActionResult> postRecommendation([FromBody] NewRecommendation model)
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
            catch (Exception ex)
            {
                throw ex;            }
        }

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
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

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
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("recommendation-by-project")]
        public async Task<IActionResult> getRecommendationByProjectId(int projectId)
        {
            try
            {
                var recommendations = await _projectsRepository.GetRecommendationByProjectId(projectId);

                if (recommendations.Any())
                {
                    return Ok(recommendations);
                }
                return BadRequest("No hay recomendaciones para el proyecto.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpPost]
        [Route("qualify-project")]
        public async Task<IActionResult> qualifyProject(QualifyProject model)
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
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("project-qualifications")]
        public async Task<IActionResult> GetProjectQualifications(int projectId)
        {
            try
            {
                var qualifications = await _projectsRepository.GetProjectQualifications(projectId);

                if (qualifications.Any())
                {


                    return Ok(qualifications);
                }
                return BadRequest("No hay calificaciones para el proyecto.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("project-by-year")]
        public async Task<IActionResult> GetProjectsByYear()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByYear();

                if (projects.Any())
                {


                    return Ok(projects);
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("project-by-category")]
        public async Task<IActionResult> GetProjectsByCategory()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByCategory();

                if (projects.Any())
                {


                    return Ok(projects);
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("project-by-qualifications")]
        public async Task<IActionResult> GetProjectsByQualifications()
        {
            try
            {
                var projects = await _projectsRepository.GetProjectsByQualifications();

                if (projects.Any())
                {


                    return Ok(projects);
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("users-per-project")]
        public async Task<IActionResult> GetUsersPerProject()
        {
            try
            {
                var projects = await _projectsRepository.GetUsersByProject();

                if (projects.Any())
                {


                    return Ok(projects);
                }
                return BadRequest("No hay proyectos registrados.");
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }
        }
    }
}
