
using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Microsoft.EntityFrameworkCore;


namespace Expo_Management.API.Repositories
{
    public class FilesUploaderRepository : IFilesUploaderRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostEnvironment _hostEnvironment;


        public FilesUploaderRepository(ApplicationDbContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
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
                }
                if (file.ContentType == "application/pdf" || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {

                    return obj;
                }
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
                      await _context.Files.AddAsync(obj);
                      await _context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        // get profile picture
        // public async Task<FilesModel> getProfilePictureAsync(string userId)
        // {
        //     try
        //     {
        //         var result = await (from u in _context.User
        //                             where u.Id == userId
        //                             select u.ProfilePicture).FirstOrDefaultAsync();

        //         if (result != null)
        //         {
        //             return result;
        //         }

        //         return null;
        //     }
        //     catch (Exception)
        //     {

        //         return null;
        //     }
        // }


        public async Task<FilesModel> deleteFiles(int id)
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
                return null;
            }
            catch (Exception ex)
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
            catch (Exception ex)
            {
                 throw ex;
            }
          
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

                    await _context.Files.AddAsync(obj);
                    await _context.SaveChangesAsync();
                }

                return obj;
            }
            return null;
        }

        public async Task<List<FilesModel>> getAll()
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
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<FilesModel> getProjectFile(int id)
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
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<FilesModel> getFileAsync(int id)
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
            catch (Exception ex)
            {

                return null;
            }
        }
    }
}
