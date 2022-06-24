using Expo_Management.API.Entities;
using Expo_Management.API.Interfaces;

namespace Expo_Management.API.Utils
{
    public class CrudUtils
    {
        private readonly IUsersRepository _usersRepository;

        public CrudUtils(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<List<User>> getUsersAvailable(List<string> groupOfUserEmails)
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

            if (!allUsersExists && !this.areUsersAvailable(groupOfUsers)) return null;
            else return groupOfUsers;
        }
       
        /*consult if users are available*/
        public bool areUsersAvailable(List<User> groupOfUsers) 
        {   
            var allUsersAreAvailable = true;

            foreach (User user in groupOfUsers)
            {
                if (user.Project == null)
                {
                    allUsersAreAvailable = false;
                }
            }

            return allUsersAreAvailable;
        }

        /*Add students users to their new project*/
        public async Task<List<User>> addUsersToProject(List<string> groupOfStudents, ProjectModel project)
        {
            try
            {
                var students = await getUsersAvailable(groupOfStudents);

                foreach (User user in students)
                {
                    user.Project = project;
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
