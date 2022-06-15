
using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Microsoft.EntityFrameworkCore;


namespace Expo_Management.API.Repositories
{
    public class FilesUploaderRepository : IFilesUploaderRepository
    {
        private readonly ApplicationDbContext context;
        private readonly IHostEnvironment _hostEnvironment;


        public FilesUploaderRepository(ApplicationDbContext context, IHostEnvironment hostEnvironment)
        {
            this.context = context;
            _hostEnvironment = hostEnvironment;
        }

        public async Task<FilesModel> Add(IFormFile file)
        {
            FilesModel obj = new FilesModel();
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
                    obj = new FilesModel()
                    {
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\Files\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };

                    await context.Files.AddAsync(obj);
                    await context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        public async Task<FilesModel> AddProjectsFile(IFormFile file)
        {
            FilesModel obj = new FilesModel();
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
                    obj = new FilesModel()
                    {
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\ProjectsFiles\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };

                    await context.Files.AddAsync(obj);
                    await context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        public async Task<FilesModel> AddProfilePicture(IFormFile file)
        {
            FilesModel obj = new FilesModel();
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
                    obj = new FilesModel()
                    {
                        Name = file.FileName,
                        Size = file.Length,
                        Url = @"\Resources\ProfilePictures\" + file.FileName,
                        uploadDateTime = DateTime.Now
                    };

                    await context.Files.AddAsync(obj);
                    await context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        public async Task<FilesModel> deleteFiles(string file)
        {
            try
            {
                var result = await (from X in context.Files
                                    where X.Name == file
                                    select X).FirstOrDefaultAsync();

                if (result != null)
                {

                    string wwwPath = _hostEnvironment.ContentRootPath;
                    var uploads = Path.Combine(wwwPath, @"Resources\Files");
                    var oldImagePath = Path.Combine(uploads, result.Name.TrimStart('\\'));

                    System.IO.File.Delete(oldImagePath);


                    context.Remove(result);
                    await context.SaveChangesAsync();

                    return result;
                }
                return null;
            }
            catch (Exception ex)
            {
                context.Dispose();
                return null;
            }

        }


        public bool fileExist(string name)
        {
            try
            {
                var result = (from X in context.Files
                              where X.Name == name
                              select X.Name).FirstOrDefault();

                if (result != null)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                 throw ex;
            }
          
        }


        public async Task<List<FilesModel>> getAll()
        { 
            try
            {
                return await context.Files.ToListAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
