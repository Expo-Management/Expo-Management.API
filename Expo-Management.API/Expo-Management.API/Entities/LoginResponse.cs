namespace Expo_Management.API.Entities
{
    public class LoginResponse
    {
        public string? Token { get; set; }
        public DateTime? Expiration { get; set; }
        public string Role { get; set; }
    }
}
