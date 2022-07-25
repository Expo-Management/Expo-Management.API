namespace Expo_Management.API.Entities.Projects
{
    public class Claim
    {
        public int Id { get; set; }
        public string ClaimDescription { get; set; }
        public ProjectModel Project { get; set; }
    }
}
