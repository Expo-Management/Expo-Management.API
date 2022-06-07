﻿using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Expo_Management.API.Repositories
{
    public class UserRepository: IUsersRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(
            ApplicationDbContext context,
            UserManager<User> userManager) 
        {
            _context = context;
            _userManager = userManager;
        }

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
    }
}
