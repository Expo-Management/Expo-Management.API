
using Expo_Management.API.Entities;
using Expo_Management.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace UploadFiles.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {

        private readonly IFilesUploaderRepository _filesUploaderRepository;


        public FilesController(IFilesUploaderRepository filesUploaderRepository)
        {
            _filesUploaderRepository = filesUploaderRepository;
        }

        [HttpPost]
        [Route("file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {

            if (file != null)
            {
                var existFile = _filesUploaderRepository.fileExist(file.FileName);
                if(existFile == false)
                {
                    await _filesUploaderRepository.Add(file);
                    return Ok($"File {file.FileName} upload succesfully!");
                }
                return BadRequest("File already exists.");
            }

            return BadRequest("There was an error.");
        }

        //[HttpPost]
        //[Route("upload-pf")]
        //public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        //{

        //    if (file != null)
        //    {
        //        var existFile = _filesUploaderRepository.fileExist(file.FileName);
        //        if (existFile == false)
        //        {
        //            if (file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/jpg")
        //            {
        //                await _filesUploaderRepository.Add(file);
        //                return Ok($"Profile picture upload succesfully!");
        //            }

        //            return BadRequest("Profile picture must be only JPEG or PNG");

        //        }
        //        return BadRequest("File already exists.");
        //    }

        //    return BadRequest("There was an error.");
        //}

        [HttpDelete]
        [Route("file")]
        public async Task<IActionResult> DeleteFile(string file)
        {
            var result = await _filesUploaderRepository.deleteFiles(file);

                if (result != null)
                {
                    return Ok($"File deleted succesfully!");
                }            

            return BadRequest("File doesn't exist.");
        }


        [HttpGet]
        [Route("files")]
        public async Task<IActionResult> ShowFiles()
        {

            var files = await _filesUploaderRepository.getAll();

            if (files != null)
            {
                var domainFiles = new List<FilesModel>();

                foreach (var items in files)
                {
                    domainFiles.Add(new FilesModel()
                    {
                        Name = items.Name,
                        Size = (items.Size*(8) / (8*1000)),
                        uploadDateTime = items.uploadDateTime

                    });
                }
                return Ok(domainFiles);
            }
            return BadRequest("There was an error.");
        }
    }
}

