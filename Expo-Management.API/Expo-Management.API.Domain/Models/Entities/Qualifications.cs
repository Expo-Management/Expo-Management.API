namespace Expo_Management.API.Domain.Models.Entities
{
    public class Qualifications
    {
        public int Id { get; set; }
        public int Punctuation { get; set; }
        public string Comments { get; set; } = default!;
        public User Judge { get; set; } = default!;
        public Project Project { get; set; } = default!;
    }
}
