using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class UserRepository: IUsersRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public UserRepository(
            UserManager<User> userManager,
            ApplicationDbContext context
            ) 
        {
            _context = context;
            _userManager = userManager;
        }

        //Judges

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

        public async Task<User> UpdateJudgeAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserId = model.Id;
            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;

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

        public async Task<User> UpdateAdminAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserId = model.Id;
            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;

            /*if (model.ProfilePicture != null)
            {
                var upload = await _filesUploaderRepository.AddProfilePicture(model.ProfilePicture);
                oldUser.ProfilePicture = upload;
            }*/

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

        //Students

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


        public async Task<User> UpdateStudentAsync(UpdateUser model)
        {
            var oldUser = await _userManager.FindByEmailAsync(model.Email);

            oldUser.UserId = model.Id;
            oldUser.UserName = model.UserName;
            oldUser.Name = model.Name;
            oldUser.Lastname = model.Lastname;
            oldUser.Email = model.Email;
            oldUser.PhoneNumber = model.Phone;
            oldUser.Project = model.Project;

            /*if (model.ProfilePicture != null)
            {
                var upload = await _filesUploaderRepository.AddProfilePicture(model.ProfilePicture);
                oldUser.ProfilePicture = upload;
            }*/

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

        
    }
}
