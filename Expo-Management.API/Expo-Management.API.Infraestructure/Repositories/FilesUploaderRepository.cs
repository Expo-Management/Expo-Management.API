using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Infraestructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Expo_Management.API.Infraestructure.Repositories
{
    public class FilesUploaderRepository : IFilesUploaderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<FilesUploaderRepository> _logger;

        public FilesUploaderRepository(ApplicationDbContext context,
            IHostEnvironment hostEnvironment,
            ILogger<FilesUploaderRepository> logger
            )
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task<Files?> AddProjectsFile(IFormFile file)
        {
            Files obj = new();
            string wwwPath = _hostEnvironment.ContentRootPath;

            if (file != null)
            {
                var uploads = Path.Combine(wwwPath, @"Resources\ProjectsFiles\");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }


                using (var FileStreams = new FileStream(Path.Combine(uploads, file.FileName),
                    FileMode.Create))
                {
                    file.CopyTo(FileStreams);
                    obj = new Files()
                    { 
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\ProjectsFiles\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };                    
                }
                if (file.ContentType == "application/pdf" || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {

                    return obj;
                }
            }
            _logger.LogWarning("Error al subir un archivo.");
            return null;
        }

        public async Task<Files?> AddProfilePicture(IFormFile file)
        {
            Files obj = new();
            string wwwPath = _hostEnvironment.ContentRootPath;

            if (file != null)
            {
                var uploads = Path.Combine(wwwPath, @"Resources\ProfilePictures\");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var FileStreams = new FileStream(Path.Combine(uploads, file.FileName),
                    FileMode.Create))
                {
                    file.CopyTo(FileStreams);
                    obj = new Files()
                    {
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\ProfilePictures\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };
                      await _context.Files.AddAsync(obj);
                      await _context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        public async Task<Files?> deleteFiles(int id)
        {
            try
            {
                var result = await (from X in _context.Files
                                    where X.Id == id
                                    select X).FirstOrDefaultAsync();

                if (result != null)
                {

                    string wwwPath = _hostEnvironment.ContentRootPath;
                    var uploads = Path.Combine(wwwPath, @"Resources\Files");
                    var oldImagePath = Path.Combine(uploads, result.Name.TrimStart('\\'));

                    System.IO.File.Delete(oldImagePath);


                    _context.Remove(result);
                    await _context.SaveChangesAsync();

                    return result;
                }
                _logger.LogWarning("Error al eliminar un archivo.");
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }


        public bool fileExist(string name)
        {
            try
            {
                var result = (from X in _context.Files
                              where X.Name == name
                              select X.Name).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                 throw;
            }
          
        }


        public async Task<Files?> Add(IFormFile file)
        {
            Files obj = new Files();
            string wwwPath = _hostEnvironment.ContentRootPath;

            if (file != null)
            {
                var uploads = Path.Combine(wwwPath, @"Resources\Files");
                if (!Directory.Exists(uploads))
                {
                    Directory.CreateDirectory(uploads);
                }

                using (var FileStreams = new FileStream(Path.Combine(uploads, file.FileName),
                    FileMode.Create))
                {
                    file.CopyTo(FileStreams);
                    obj = new Files()
                    {
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\Files\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };

                    await _context.Files.AddAsync(obj);
                    await _context.SaveChangesAsync();
                }

                return obj;
            }
            _logger.LogWarning("Error al subir un archivo.");
            return null;
        }

        public async Task<List<Files>?> getAll()
        {
            try
            {
                var result = await (from X in _context.Files 
                              where X.Url.Contains(@"\Files")
                              select X).ToListAsync();

                if (result != null)
                {
                    return result;
                }

                return null; ;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Files?> getProjectFile(int id)
        {
            try
            {
                var result = await (from x in _context.Files
                                    join p in _context.Projects on x.Id equals p.Files.Id
                                    where p.Id == id
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    return result;
                }

                return null; ;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async Task<Files?> getFileAsync(int id)
        {
            try
            {
                var result = await (from x in _context.Files
                                   where x.Id == id && x.Url.Contains(@"\Files")
                                    select x).FirstOrDefaultAsync();

                if (result != null)
                {
                    return result;
                }

                return null; ;
            }
            catch (Exception)
            {

                return null;
            }
        }
    }
}
