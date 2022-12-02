using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.Reponses;
using Expo_Management.API.Domain.Models.ViewModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IProjectsRepository
    {

        Task<Response?> CreateProject(NewProjectInputModel model);
        bool ProjectExists(string lider);
        Task<Response?> GetAllProjectsAsync();
        Task<Response?> GetFair(int project);
        Task<Response?> removeUserFromProject(string email);
        Task<Response?> GetAllCurrentProjectsAsync();
        Task<Response?> GetOldProjectsAsync();
        Task<Response?> GetProjectDetails(int projectId);
        Task<Response> CreateProjectClaim(NewClaimInputModel model);
        void SendCalificationsEmails(Project project, User judge);
        Task<Response?> JudgeRecommendation(NewRecommendationInputModel model);
        Task<Response?> GetProjectById(int ProjectId);
        Task<Response?> GetRecommendation(int recomendacion);
        Task<Response?> GetMembers();
        Task<Response> GetMembersEmail(int projectId);
        Task<Response?> GetRecommendationByProjectId(int recomendacion);
        Task<Response?> QualifyProject(QualifyProjectInputModel model);
        Task<Boolean> CanJudgeQualifyTheProject(int ProjectId, string JudgeEmail);
        Task<Response?> GetProjectQualifications(int projectId);
        Task<Response?> GetProjectsByYear();
        Task<Response?> GetProjectsByCategory();
        Task<Response?> GetUsersByProject();
        Task<Response?> GetProjectsByQualifications();
    }
}
