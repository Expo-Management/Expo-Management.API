using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.InputModels
{
    public class EventInputModel
    {
        public int? Id { get; set; }
        public string Description { get; set; } = default!;
        public string Location { get; set; } = default!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Details { get; set; }
        public int FairId { get; set; }
    }
}
