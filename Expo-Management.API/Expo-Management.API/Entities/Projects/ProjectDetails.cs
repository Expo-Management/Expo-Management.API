namespace Expo_Management.API.Entities.Projects
{
    public class ProjectDetails
    {
        public int ProjectId { get; set; }   
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public List<string> Members { get; set; }
        public string Category { get; set; }
        public List<ProjectQualifications>  ProjectQualifications { get; set; }
        public string FinalPunctuation { get; set; }
    }
}
