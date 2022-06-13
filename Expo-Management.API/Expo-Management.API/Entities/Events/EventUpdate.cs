namespace Expo_Management.API.Entities.Events
{
    public class EventUpdate
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string? Details { get; set; }
    }
}
