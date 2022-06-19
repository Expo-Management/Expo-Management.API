using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class Files
    {

        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string Url { get; set; }

        public DateTime uploadDateTime { get; set; }

    }
}
