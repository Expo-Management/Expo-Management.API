using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Controllers;
using Expo_Management.API.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UploadFiles.Controllers
{
    /// <summary>
    /// Controlador de archivos
    /// </summary>

    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : Controller
    {

        private readonly IFilesUploaderRepository _filesUploaderRepository;
        private readonly ILogger<FilesController> _logger;

        /// <summary>
        /// Constructor del controlador de archivos
        /// </summary>
        /// <param name="filesUploaderRepository"></param>
        /// <param name="logger"></param>

        public FilesController(IFilesUploaderRepository filesUploaderRepository, ILogger<FilesController> logger)
        {
            _filesUploaderRepository = filesUploaderRepository;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint para subir un archivo
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        [Route("file")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var existFile = _filesUploaderRepository.fileExist(file.FileName);
                    if (existFile == false)
                    {
                        if (file.ContentType == "application/pdf" || file.ContentType == "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                        {
                            await _filesUploaderRepository.Add(file);
                            return Ok($"Documento {file.FileName} subido exitosamente!");
                        }
                        return BadRequest("Documento solo puede ser PDF o Word.");
                    }
                    return BadRequest("Documento ya existe.");
                }

                return BadRequest("Hubo un error por favor intentelo más tarde.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para descargar un archivo del proyecto
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Judge,User")]
        [HttpGet]
        [Route("download-project-file")]
        public async Task<IActionResult> DownladProjectFile(int id)
        {
            try
            {
                var file = await _filesUploaderRepository.getProjectFile(id);

                if (file != null)
                {
                    string startupPath = System.IO.Directory.GetCurrentDirectory();
                    startupPath = Environment.CurrentDirectory + file.Url;

                    var bytes = System.IO.File.ReadAllBytes(startupPath);

                    if (file.Name.EndsWith(".pdf"))
                    {
                        return File(bytes, "application/pdf", file.Name);

                    }
                    return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", file.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return StatusCode(500);
        }

        /// <summary>
        /// Endpoint para descargar un archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("download-file")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var file = await _filesUploaderRepository.getFileAsync(id);

                if (file != null)
                {
                    string startupPath = System.IO.Directory.GetCurrentDirectory();
                    startupPath = Environment.CurrentDirectory + file.Url;

                    var bytes = System.IO.File.ReadAllBytes(startupPath);

                    if (file.Name.EndsWith(".pdf"))
                    {
                        return File(bytes, "application/pdf", file.Name);

                    }
                    return File(bytes, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", file.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return StatusCode(500);
        }

        // [HttpGet]
        // [Route("profile-picture")]
        // public async Task<IActionResult> getProfilePicture(string userId)
        // {
        //     var profilePicture = await _filesUploaderRepository.getProfilePictureAsync(userId);

        //     if (profilePicture != null)
        //     {
        //             return Ok(profilePicture);
        //     }

        //     return BadRequest("Usuario no posee foto de perfil");
        // }

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

        /// <summary>
        /// Endpoint para eliminar un archivo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("file")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                var result = await _filesUploaderRepository.deleteFiles(id);

                if (result != null)
                {
                    return Ok("Documento borrado exitosamente!");
                }

                return BadRequest("Documento no existe.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Endpoint para ver los archivos
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        [Route("files")]
        public async Task<IActionResult> ShowFiles()
        {

            try
            {
                var files = await _filesUploaderRepository.getAll();

                if (files != null)
                {
                    var domainFiles = new List<Files>();

                    foreach (var items in files)
                    {
                        domainFiles.Add(new Files()
                        {
                            Id = items.Id,
                            Name = items.Name,
                            Url = items.Url,
                            Size = (items.Size * (8) / (8 * 1000)),
                            uploadDateTime = items.uploadDateTime

                        });
                    }
                    return Ok(domainFiles);
                }
                return BadRequest("Hubo un error, por favor, intentelo más tarde.");
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }
}

