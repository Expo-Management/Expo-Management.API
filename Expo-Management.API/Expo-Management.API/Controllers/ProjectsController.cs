using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadFiles.Controllers;
using Expo_Management.API.Infraestructure.Repositories;

namespace Expo_Management.API.Controllers
{
    /// <summary>
    /// Controlador para proyectos
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : Controller
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
        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("projects")]
        public async Task<IActionResult> AddProjects([FromForm] NewProjectInputModel model)
        {
            var response = await _projectsRepository.CreateProject(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para mostrar proyectos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("projects")]
        public async Task<IActionResult> ShowProjects()
        {
            var response = await _projectsRepository.GetAllCurrentProjectsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para eliminar a un usuario de su grupo de proyecto de Feria
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("remove-user-project")]
        public async Task<IActionResult> removeUserFromProject(string email)
        {

            var response = await _projectsRepository.removeUserFromProject(email);
            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para mostrar los proyectos antiguos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("old-projects")]
        public async Task<IActionResult> showOldProjects()
        {
            var response = await _projectsRepository.GetOldProjectsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para mostrar las menciones
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("mentions")]
        public async Task<IActionResult> showMentions()
        {

            var response = await _projectsRepository.GetMentionsAsync();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener las calificaciones de los proyectos
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("project")]
        public async Task<IActionResult> getProjectQualificationAsync(int projectId)
        {

            var response = await _projectsRepository.GetProjectDetails(projectId);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });

        }

        /// <summary>
        /// Endpoint para crear los reclamos
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPost]
        [Route("create-claim")]
        public async Task<IActionResult> CreateProjectClaim(NewClaimInputModel model)
        {

            var response = await _projectsRepository.CreateProjectClaim(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para crear las recomendaciones
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge")]
        [HttpPost]
        [Route("recommendation")]
        //[Authorize(Roles = "Judge")]
        public async Task<IActionResult> postRecommendation([FromBody] NewRecommendationInputModel model)
        {

            var response = await _projectsRepository.JudgeRecommendation(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener las recomendaciones
        /// </summary>
        /// <param name="recomendacion"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("getRecommendation")]
        public async Task<IActionResult> getRecommendation(int recomendacion)
        {

            var response = await _projectsRepository.GetRecommendation(recomendacion);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los miembros del proyecto
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("project-members")]
        public async Task<IActionResult> GetProjectMembers()
        {

            var response = await _projectsRepository.GetMembers();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los correos de los miembros de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge")]
        [HttpGet]
        [Route("members-emails")]
        public async Task<IActionResult> GetMembersEmail(int projectId)
        {

            var response = await _projectsRepository.GetMembersEmail(projectId);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener la recomendacion de un proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("recommendation-by-project")]
        public async Task<IActionResult> getRecommendationByProjectId(int projectId)
        {

            var response = await _projectsRepository.GetRecommendationByProjectId(projectId);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para validar la calificacion del proyecto
        /// </summary>
        /// <param name="ProjectId"></param>
        /// <param name="JudgeEmail"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge")]
        [HttpGet]
        [Route("can-judge-qualify-project")]
        public async Task<IActionResult> qualifyProject(int ProjectId, string JudgeEmail)
        {
            try
            {
                var result = await _projectsRepository.CanJudgeQualifyTheProject(ProjectId, JudgeEmail);

                if (result)
                {
                    return Ok(result);
                }
                return BadRequest("El proyecto ya fue calificado con esta cuenta");
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
        [Authorize(Roles = "Judge")]
        [HttpPost]
        [Route("qualify-project")]
        public async Task<IActionResult> qualifyProject(QualifyProjectInputModel model)
        {

            var response = await _projectsRepository.QualifyProject(model);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener las calificaciones del proyecto
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("project-qualifications")]
        public async Task<IActionResult> GetProjectQualifications(int projectId)
        {

            var response = await _projectsRepository.GetProjectQualifications(projectId);

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por año
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("project-by-year")]
        public async Task<IActionResult> GetProjectsByYear()
        {

            var response = await _projectsRepository.GetProjectsByYear();
            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por categoria
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("project-by-category")]
        public async Task<IActionResult> GetProjectsByCategory()
        {

            var response = await _projectsRepository.GetProjectsByCategory();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los proyectos por las calificaciones
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("project-by-qualifications")]
        public async Task<IActionResult> GetProjectsByQualifications()
        {

            var response = await _projectsRepository.GetProjectsByQualifications();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }

        /// <summary>
        /// Endpoint para obtener los usurios por los proyectos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("users-per-project")]
        public async Task<IActionResult> GetUsersPerProject()
        {

            var response = await _projectsRepository.GetUsersByProject();

            return Json(new { status = response.Status, message = response.Message, data = response.Data, error = response.Error });
        }
    }
}
