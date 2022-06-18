using Expo_Management.API.Entities;

namespace Expo_Management.API.Interfaces
{
    public interface IProjectsRepository
    {

        Task<ProjectModel> CreateProject(NewProject model);

        bool ProjectExists(string lider);

        Task<List<ProjectModel>> GetAllProjectsAsync();

    }
}
