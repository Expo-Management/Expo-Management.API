namespace Expo_Management.API.Entities.Projects
{
    public class UpdateUserProject
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Last { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Phone { get; set; }
        public ProjectModel Project { get; set; }
        public bool IsLead { get; set; }
    }
}
