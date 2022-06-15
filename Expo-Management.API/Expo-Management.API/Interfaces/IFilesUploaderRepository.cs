
using Expo_Management.API.Entities;

namespace Expo_Management.API.Repositories
{
    public interface IFilesUploaderRepository
    {
        Task<FilesModel> Add(IFormFile file);

        Task<FilesModel> AddProjectsFile(IFormFile file);

        Task<FilesModel> AddProfilePicture(IFormFile file);

        Task<List<FilesModel>> getAll();

        public bool fileExist(string name);

        Task<FilesModel> deleteFiles(string file);

    }
}
