namespace Expo_Management.API.Entities.Mentions
{
    public class Mention
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual ICollection<ProjectModel> Projects { get; set; }
    }
}
