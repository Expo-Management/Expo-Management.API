namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class ProjectMembersViewModels
    {
        public string Name { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = default!;
    }
}
