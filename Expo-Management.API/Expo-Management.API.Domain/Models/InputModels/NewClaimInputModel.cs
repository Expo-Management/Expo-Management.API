namespace Expo_Management.API.Domain.Models.InputModels
{
    public class NewClaimInputModel
    {
        public int ProjectId { get; set; }
        public string ClaimDescription { get; set; } = default!;
    }
}
