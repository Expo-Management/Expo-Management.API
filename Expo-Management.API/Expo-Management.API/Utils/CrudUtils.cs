using Expo_Management.API.Auth;
using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;

namespace Expo_Management.API.Utils
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

        public async Task<List<User>> getUsersAvailableAsync(List<string> groupOfUserEmails)
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
        public async Task<List<User>> addUsersToProject(List<string> groupOfStudents, ProjectModel project)
        {
            try
            {
                var students = await getUsersAvailableAsync(groupOfStudents);

                foreach (User user in students)
                {
                    user.Project = project;
                    var updatedUser = await _usersRepository.UpdateStudentAsync(new UpdateUser() {
                        Id = user.UserId,
                        UserName = user.UserName,
                        Project = project,
                        Name = user.Name,
                        Lastname = user.Lastname,
                        Email = user.Email,
                        Phone = user.PhoneNumber
                    });

                    _context.SaveChanges();
                }

                return students;
            }
            catch (Exception ex)
            {
                return null;
                throw ex;
            }
        }
    }
}
