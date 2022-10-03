﻿using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.ViewModels;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IProjectsRepository
    {

        Task<Project?> CreateProject(NewProjectInputModel model);
        bool ProjectExists(string lider);
        Task<List<Project>?> GetAllProjectsAsync();
        Task<Fair?> GetFair(int project);
        Task<User?> removeUserFromProject(string email);
        //Task<Project?> removeProject(string email);
        Task<List<Mention>?> GetMentionsAsync();
        Task<List<Project>?> GetAllCurrentProjectsAsync();
        Task<List<Project>?> GetOldProjectsAsync();
        Task<List<ProjectDetailsViewModel>?> GetProjectDetails(int projectId);
        Task<Claim?> CreateProjectClaim(NewClaimInputModel model);
        void SendCalificationsEmails(Project project, User judge);
        Task<Recommendation?> JudgeRecommendation(NewRecommendationInputModel model);
        Task<Project?> GetProjectById(int ProjectId);
        Task<Recommendation?> GetRecommendation(int recomendacion);
        Task<List<ProjectMembersViewModels>?> GetMembers();
        Task<List<User>> GetMembersEmail(int projectId);
        Task<List<Recommendation>?> GetRecommendationByProjectId(int recomendacion);
        Task<Qualifications?> QualifyProject(QualifyProjectInputModel model);
        Task<Boolean> CanJudgeQualifyTheProject(int ProjectId, string JudgeEmail);
        Task<List<ProjectQualificationsViewModel>?> GetProjectQualifications(int projectId);
        Task<List<ProjectQuantityViewModel>?> GetProjectsByYear();
        Task<List<ProjectQuantityViewModel>?> GetProjectsByCategory();
        Task<List<ProjectQuantityViewModel>?> GetUsersByProject();
        Task<List<ProjectQuantityViewModel>?> GetProjectsByQualifications();
    }
}
