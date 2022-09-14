using Expo_Management.API.Domain.Models.Entities;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Mention
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;

        public virtual ICollection<Project>? Projects { get; set; }
    }
}
