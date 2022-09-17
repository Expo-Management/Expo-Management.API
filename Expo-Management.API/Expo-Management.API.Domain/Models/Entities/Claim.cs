namespace Expo_Management.API.Domain.Models.Entities
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimDescription { get; set; } = default!;
        public Project Project { get; set; } = default!;
    }
}
