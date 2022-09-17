namespace Expo_Management.API.Domain.Models.InputModels
{
    public class QualifyProjectInputModel
    {
        public int Punctuation { get; set; }
        public string Comments { get; set; } = default!;
        public int ProjectId { get; set; }
        public string JudgeEmail { get; set; } = default!;

    }
}
