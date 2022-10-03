namespace Expo_Management.API.Domain.Models.ViewModels
{
    public class ProjectDetailsViewModel
    {
        public int ProjectId { get; set; }   
        public string ProjectName { get; set; } = default!;
        public string ProjectDescription { get; set; } = default!;
        public List<string>? Members { get; set; }
        public string? Category { get; set; }
        public List<ProjectQualificationsViewModel>?  ProjectQualifications { get; set; } 
        public string? FinalPunctuation { get; set; }
    } 
}
