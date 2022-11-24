using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Infraestructure.Data;
using Expo_Management.API.Application.Contracts.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Expo_Management.API.Domain.Models.Reponses;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Expo_Management.API.Infraestructure.Repositories
{
    /// <summary>
    /// Repositorio de usuarios
    /// </summary>
    public class UserRepository : IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesRepository;
        private readonly ILogger<UserRepository> _logger;

        /// <summary>
        /// Constructor del repositorio usuarios
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <param name="filesRepository"></param>
        public UserRepository(
            UserManager<User> userManager,
            ApplicationDbContext context,
            IFilesUploaderRepository filesRepository,
            ILogger<UserRepository> logger
            )
        {
            _context = context;
            _userManager = userManager;
            _filesRepository = filesRepository;
            _logger = logger;
        }

        /// <summary>
        /// Metodo para obtener el nombre completo del usuario
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response?> GetUserFullName(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = user.Name + ' ' + user.Lastname,
                        Message = "Usuario encontrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuario no encontrado."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obetner los jueces del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetJudgesAsync()
        {
            try
            {
                List<User> judges = (List<User>)await _userManager.GetUsersInRoleAsync("Judge");

                if (judges.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = judges,
                        Message = "Usuarios encontrados exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuarios no encontrados."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obetner los jueces del sistema
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response?> GetJudgeAsync(string email)
        {
            try
            {
                User judge = await _userManager.FindByEmailAsync(email);

                if (judge != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = judge,
                        Message = "Usuario encontrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuario no encontrado."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar jueces
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> UpdateJudgeAsync(UpdateJudgeInputModel model)
        {
            try
            {
                var oldUser = await _userManager.FindByEmailAsync(model.Email);

                if (oldUser != null)
                {
                    oldUser.UserName = model.UserName;
                    oldUser.Name = model.Name;
                    oldUser.Lastname = model.Lastname;
                    oldUser.Email = model.Email;
                    oldUser.PhoneNumber = model.Phone;
                    oldUser.Institution = model.Institution;
                    oldUser.Position = model.Position;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded)
                    {
                        return new Response()
                        {
                            Status = 200,
                            Data = oldUser,
                            Message = "Usuario actualizado exitosamente!"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Status = 400,
                            Message = "Revise los datos enviados"
                        };
                    }
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuario no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar jueces
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> DeleteJudgeAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    return new Response()
                    {
                        Status = 200,
                        Data = true,
                        Message = "Usuario borrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Data = false,
                        Message = "Usuario no encontrado."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Data = false,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para los administradores del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetAdminsAsync()
        {
            try
            {
                List<User> admins = (List<User>)await _userManager.GetUsersInRoleAsync("Admin");

                if (admins.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = admins,
                        Message = "Usuarios encontrados exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuarios no encontrados."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obtener un administrador en el sistema
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response?> GetAdminAsync(string email)
        {
            try
            {
                User admin = await _userManager.FindByEmailAsync(email);

                if (admin != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = admin,
                        Message = "Usuario encontrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuario no encontrado."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar administradores
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> UpdateAdminAsync(UpdateUserInputModel model)
        {
            try
            {
                var oldUser = await _userManager.FindByEmailAsync(model.Email);

                if (oldUser != null)
                {
                    oldUser.UserName = model.UserName;
                    oldUser.Name = model.Name;
                    oldUser.Lastname = model.Lastname;
                    oldUser.Email = model.Email;
                    oldUser.PhoneNumber = model.Phone;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded)
                    {
                        return new Response()
                        {
                            Status = 200,
                            Data = oldUser,
                            Message = "Usuario actualizado exitosamente!"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Status = 400,
                            Message = "Revise los datos enviados."
                        }; ;
                    }
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuario no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar un administrador
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> DeleteAdminAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    return new Response()
                    {
                        Status = 200,
                        Data = true,
                        Message = "Usuario borrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Data = false,
                        Message = "Usuario no encontrado."
                    }; ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Data = false,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }


        /// <summary>
        /// Metodo para obtener todos los estudiantes
        /// </summary>
        /// <returns></returns>
        public async Task<Response?> GetStudentsAsync()
        {
            try
            {
                List<User> students = (List<User>)await _userManager.GetUsersInRoleAsync("User");

                if (students.Count > 0)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = students,
                        Message = "Usuarios encontrados exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuarios no encontrados."
                    }; ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para obetner un estudiante
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response?> GetStudentAsync(string email)
        {
            try
            {
                var student = await (from u in _context.User
                                     where u.Email == email
                                     select u).Include(x => x.Project).FirstAsync();

                if (student != null)
                {
                    return new Response()
                    {
                        Status = 200,
                        Data = student,
                        Message = "Usuario encontrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Message = "Usuario no encontrado."
                    }; ;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar un estudiante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> UpdateStudentAsync(UpdateUserInputModel model)
        {
            try
            {
                var oldUser = await _userManager.FindByEmailAsync(model.Email);

                if (oldUser != null)
                {
                    oldUser.UserName = model.UserName;
                    oldUser.Name = model.Name;
                    oldUser.Lastname = model.Lastname;
                    oldUser.Email = model.Email;
                    oldUser.PhoneNumber = model.Phone;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded)
                    {
                        return new Response()
                        {
                            Status = 200,
                            Data = oldUser,
                            Message = "Usuario acutalizado exitosamente!"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Status = 204,
                            Message = "Usuario no encontrado."
                        }; ;
                    }
                }
                return new Response()
                {
                    Status = 400,
                    Message = "Revise los datos enviados"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para eliminar un estudiante
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<Response> DeleteStudentAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    return new Response()
                    {
                        Status = 200,
                        Data = true,
                        Message = "Usuario borrado exitosamente!"
                    };
                }
                else
                {
                    return new Response()
                    {
                        Status = 204,
                        Data = false,
                        Message = "Usuario no encontrado."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Data = false,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }

        /// <summary>
        /// Metodo para actualizar el proyecto de un estudiante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Response?> UpdateStudetProjectAsync(UpdateUserProjectInputModel model)
        {
            try
            {
                var oldUser = await _userManager.FindByEmailAsync(model.Email);

                if (oldUser != null)
                {
                    oldUser.UserId = model.UserId;
                    oldUser.Name = model.Name;
                    oldUser.Lastname = model.Last;
                    oldUser.Email = model.Email;
                    oldUser.UserName = model.Username;
                    oldUser.PhoneNumber = model.Phone;
                    oldUser.Project = model.Project;
                    oldUser.IsLead = model.IsLead;

                    var result = await _userManager.UpdateAsync(oldUser);

                    if (result.Succeeded)
                    {
                        return new Response()
                        {
                            Status = 200,
                            Data = oldUser,
                            Message = "Usuario actualizado exitosamente!"
                        };
                    }
                    else
                    {
                        return new Response()
                        {
                            Status = 400,
                            Message = "Revise los datos enviados"
                        };
                    }
                }
                return new Response()
                {
                    Status = 204,
                    Message = "Usuario no encontrado."
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new Response()
                {
                    Status = 500,
                    Message = "Hubo un problema procesando su solicitud, contacte administracion."
                };
            }
        }
    }
}
