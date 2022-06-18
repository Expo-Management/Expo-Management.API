using Expo_Management.API.Entities;
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
        [Route("add")]
        public async Task<IActionResult> AddProjects([FromForm] NewProject model)
        {
            try
            {
                var project = await _projectsRepository.CreateProject(model);

                if (model == null)
                {
                    return BadRequest("Project or file are null");

                }
                return Ok(project);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        [Route("showAll")]
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
                            Lider = items.Lider,
                            Member2 = items.Member2,
                            Member3 = items.Member3,
                            Files = new FilesModel()
                            {
                                Id = items.Files.Id,
                                Name = items.Files.Name,
                                Size = items.Files.Size,
                                Url = items.Files.Url,
                                uploadDateTime = items.Files.uploadDateTime
                            }
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
    }
}
