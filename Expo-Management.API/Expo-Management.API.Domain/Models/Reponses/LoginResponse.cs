namespace Expo_Management.API.Domain.Models.Reponses
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string Role { get; set; } = default!;
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; } = default!;
    }
}
