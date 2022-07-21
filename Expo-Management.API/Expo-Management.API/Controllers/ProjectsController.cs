using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
                    return BadRequest("El proyecto ya existe o alguno de los usuarios ya se encuentra asignado a otro proyecto.");

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
                return BadRequest("There was an error.");
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
                return BadRequest("There was an error");
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        [HttpPost]
        [Route("recommendation")]
        [Authorize(Roles = "Judge")]
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
    }
}
