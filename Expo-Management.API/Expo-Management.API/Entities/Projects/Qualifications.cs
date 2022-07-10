namespace Expo_Management.API.Entities.Projects
{
    public class Qualifications
    {
        public int Id { get; set; }
        public int Punctuation { get; set; }
        public User Judge { get; set; }
        public ProjectModel Project { get; set; }

    }
}
