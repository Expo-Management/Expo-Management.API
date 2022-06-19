﻿using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Mentions;

namespace Expo_Management.API.Interfaces
{
    public interface IProjectsRepository
    {

        Task<ProjectModel> CreateProject(NewProject model);

        bool ProjectExists(string lider);

        Task<List<ProjectModel>> GetAllProjectsAsync();
        Task<List<Mention>> GetMentionsAsync();
        Task<List<ProjectModel>> GetAllCurrentProjectsAsync();
    }
}
