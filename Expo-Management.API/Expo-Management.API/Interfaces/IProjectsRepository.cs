using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Entities.Projects;

namespace Expo_Management.API.Interfaces
{
    public interface IProjectsRepository
    {

        Task<ProjectModel> CreateProject(NewProject model);
        bool ProjectExists(string lider);
        Task<List<ProjectModel>> GetAllProjectsAsync();
        Task<Fair> GetFair(int project);
        Task<List<Mention>> GetMentionsAsync();
        Task<List<ProjectModel>> GetAllCurrentProjectsAsync();
        Task<List<ProjectModel>> GetOldProjectsAsync();
        Task<List<ProjectQualifications>> GetProjectWithQualificationsAsync(int projectId);
        Task<List<ProjectDetails>> GetProjectDetails(int projectId);

    }
}
