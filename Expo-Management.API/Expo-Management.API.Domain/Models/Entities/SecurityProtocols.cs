namespace Expo_Management.API.Domain.Models.Entities
{
    public class SecurityProtocols
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Description { get; set; } = default!;
        public Fair Fair { get; set; } = default!;
    }
}
    