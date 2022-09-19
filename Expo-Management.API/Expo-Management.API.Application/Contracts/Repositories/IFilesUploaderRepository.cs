using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace Expo_Management.API.Application.Contracts.Repositories
{
    public interface IFilesUploaderRepository
    {
        Task<Files?> Add(IFormFile file);

        Files? AddProjectsFile(IFormFile file);

        Task<Files?> AddProfilePicture(IFormFile file);

        Task<List<Files>?> getAll();

        public bool fileExist(string name);

        Task<Files?> deleteFiles(int id);

        Task<Files?> getProjectFile(int id);

        Task<Files?> getFileAsync(int id);

     //   Task<FilesModel> getProfilePictureAsync(string userId);
    }
}
