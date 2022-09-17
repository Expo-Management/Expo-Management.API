using Expo_Management.API.Application.Contracts.Repositories;
using Expo_Management.API.Domain.Models.Entities;
using Expo_Management.API.Domain.Models.InputModels;
using Expo_Management.API.Domain.Models.ViewModels;
using Expo_Management.API.Infraestructure.Data;

namespace Expo_Management.API.Infraestructure.Utils
{
    public class CrudUtils
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ApplicationDbContext _context;
        public CrudUtils(IUsersRepository usersRepository,
            ApplicationDbContext context)
        {
            _context = context;
            _usersRepository = usersRepository;
        }

        public async Task<List<User>?> getUsersAvailableAsync(List<string> groupOfUserEmails)
        {
            List<User> groupOfUsers = new List<User>();
            var allUsersExists = true;

            foreach (string userEmail in groupOfUserEmails)
            {
                var tempUser = await _usersRepository.GetStudentAsync(userEmail);

                if (tempUser == null)
                {
                    allUsersExists = false;
                }
                else
                {
                    groupOfUsers.Add(tempUser);
                }
            }

            var areUsersAvailablesForProjects = areUsersAvailable(groupOfUsers);

            if (allUsersExists && areUsersAvailablesForProjects)
            {
                return groupOfUsers;
            } 
            return null;
        }
       
        /*consult if users are available*/
        public bool areUsersAvailable(List<User> groupOfUsers) 
        {   
            var allUsersAreAvailable = true;

            foreach (User user in groupOfUsers)
            {
                if (user.Project == null)
                {
                    allUsersAreAvailable = true;
                }
                else 
                {
                    allUsersAreAvailable = false;
                    return allUsersAreAvailable;
                }
            }

            return allUsersAreAvailable;
        }

        /*Add students users to their new project*/
        public async Task<List<User>?> addUsersToProject(List<string> groupOfStudents, Project project)
        {
            try
            {
                var students = await getUsersAvailableAsync(groupOfStudents);
                
                if(students != null)
                {
                    foreach (User user in students)
                    {
                        var isLeaderr = false;

                        if (user.Email == groupOfStudents[0])
                        {
                            isLeaderr = true;
                        }

                        user.Project = project;
                        var updatedUser = await _usersRepository.UpdateStudetProjectAsync(new UpdateUserProjectInputModel()
                        {
                            UserId = user.Id,
                            Username = user.UserName,
                            Project = project,
                            Name = user.Name,
                            Last = user.Lastname,
                            Email = user.Email,
                            Phone = user.PhoneNumber,
                            IsLead = isLeaderr
                        });

                        _context.SaveChanges();
                    }

                    return students;
                }

                return students;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
    }
}
