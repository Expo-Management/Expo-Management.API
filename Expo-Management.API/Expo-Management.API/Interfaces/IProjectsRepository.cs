using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;
using Expo_Management.API.Entities.Projects;

namespace Expo_Management.API.Interfaces
{
    public interface IProjectsRepository
    {

        Task<ProjectModel> CreateProject(NewProject model);
        bool ProjectExists(string lider);
        Task<List<ProjectModel>?> GetAllProjectsAsync();
        Task<Fair> GetFair(int project);
        Task<List<Mention>> GetMentionsAsync();
        Task<List<ProjectModel>> GetAllCurrentProjectsAsync();
        Task<List<ProjectModel>> GetOldProjectsAsync();
        Task<List<ProjectDetails>> GetProjectDetails(int projectId);
        Task<Claim> CreateProjectClaim(NewClaim model);
        void SendCalificationsEmails(ProjectModel project, User judge);
        Task<JudgeRecommendation> JudgeRecommendation(NewRecommendation model);
        Task<ProjectModel> GetProjectById(int ProjectId);
        Task<JudgeRecommendation> GetRecommendation(int recomendacion);
        Task<List<ProjectMembers>> GetMembers();
        Task<List<User>> GetMembersEmail(int projectId);
        Task<List<JudgeRecommendation>> GetRecommendationByProjectId(int recomendacion);
        Task<Qualifications> QualifyProject(QualifyProject model);
        Task<List<ProjectQualifications>> GetProjectQualifications(int projectId);
        Task<List<ProjectQuantity>> GetProjectsByYear();
        Task<List<ProjectQuantity>> GetProjectsByCategory();
        Task<List<ProjectQuantity>> GetUsersByProject();
        Task<List<ProjectQuantity>> GetProjectsByQualifications();
    }
}
