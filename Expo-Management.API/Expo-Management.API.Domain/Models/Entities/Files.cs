using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class Files
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public long Size { get; set; }

        public string Url { get; set; } = default!;

        public DateTime uploadDateTime { get; set; }

    }
}
