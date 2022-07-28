using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Projects;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expo_Management.API.Controllers
{
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
                return BadRequest("Hubo un error, por favor, intentelo más tarde");
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
    }
}
