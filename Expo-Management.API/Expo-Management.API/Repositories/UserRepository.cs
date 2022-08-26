﻿using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Entities.Projects;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    /// <summary>
    /// Repositorio de usuarios
    /// </summary>
    public class UserRepository: IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IFilesUploaderRepository _filesRepository;

        /// <summary>
        /// Constructor del repositorio usuarios
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        /// <param name="filesRepository"></param>
        public UserRepository(
            UserManager<User> userManager,
            ApplicationDbContext context,
            IFilesUploaderRepository filesRepository
            ) 
        {
            _context = context;
            _userManager = userManager;
            _filesRepository = filesRepository;
        }

        /// <summary>
        /// Metodo para obtener el nombre completo del usuario
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<String> GetUserFullName(string email) 
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                return user.Name + ' ' + user.Lastname;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obetner los jueces del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetJudgesAsync()
        {
            List<User> judges = (List<User>)await _userManager.GetUsersInRoleAsync("Judge");

            if (judges.Count > 0)
            {
                return judges;
            }
            else {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obetner los jueces del sistema
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetJudgeAsync(string email)
        {
            User judge = await _userManager.FindByEmailAsync(email);

            if (judge != null)
            {
                return judge;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para actualizar jueces
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<User> UpdateJudgeAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;

            // if (model.ProfilePicture != null)
            // {
            //     var upload = await _filesRepository.AddProfilePicture(model.ProfilePicture);
            //     oldUser.ProfilePicture = upload;
            // }

            var result = await _userManager.UpdateAsync(oldUser);

            if (result.Succeeded)
            {
                return oldUser;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para eliminar jueces
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> DeleteJudgeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return true;
            } else 
            { 
                return false; 
            }
        }

        /// <summary>
        /// Metodo para los administradores del sistema
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetAdminsAsync()
        {
            List<User> admins = (List<User>)await _userManager.GetUsersInRoleAsync("Admin");

            if (admins.Count > 0)
            {
                return admins;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obtener un administrador en el sistema
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetAdminAsync(string email)
        {
            User admin = await _userManager.FindByEmailAsync(email);

            if (admin != null)
            {
                return admin;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para actualizar administradores
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<User> UpdateAdminAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;

            // if (model.ProfilePicture != null)
            // {
            //     var upload = await _filesRepository.AddProfilePicture(model.ProfilePicture);
            //     oldUser.ProfilePicture = upload;
            // }

            var result = await _userManager.UpdateAsync(oldUser);

            if (result.Succeeded)
            {
                return oldUser;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para eliminar un administrador
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> DeleteAdminAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        
        /// <summary>
        /// Metodo para obtener todos los estudiantes
        /// </summary>
        /// <returns></returns>
        public async Task<List<User>> GetStudentsAsync()
        {
            List<User> students = (List<User>)await _userManager.GetUsersInRoleAsync("User");

            if (students.Count > 0)
            {
                return students;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para obetner un estudiante
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<User> GetStudentAsync(string email)
        {
            var student = await (from u in _context.User
                                 where u.Email == email
                                 select u).Include(x => x.Project).FirstAsync();

            if (student != null)
            {
                return student;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para actualizar un estudiante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<User> UpdateStudentAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;

            // if (model.ProfilePicture != null)
            // {
            //     var upload = await _filesRepository.AddProfilePicture(model.ProfilePicture);
            //     oldUser.ProfilePicture = upload;
            // }

            var result = await _userManager.UpdateAsync(oldUser);

            if (result.Succeeded)
            {
                return oldUser;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para eliminar un estudiante
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> DeleteStudentAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Metodo para actualizar el proyecto de un estudiante
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<User> UpdateStudetProjectAsync(UpdateUserProject model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

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
                return oldUser;
            }
            else
            {
                return null;
            }
        }
    }
}
