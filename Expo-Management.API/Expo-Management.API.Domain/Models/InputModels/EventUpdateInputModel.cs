namespace Expo_Management.API.Domain.Models.InputModels
{
    public class EventUpdateInputModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Details { get; set; }
        public bool AllDay { get; set; }
    }
}
