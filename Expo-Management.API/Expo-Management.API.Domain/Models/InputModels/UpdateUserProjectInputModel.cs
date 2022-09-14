using Expo_Management.API.Domain.Models.Entities;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class UpdateUserProjectInputModel
    {
        public string UserId { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string Last { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Username { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public Project Project { get; set; } = default!;
        public bool IsLead { get; set; }
    }
}
