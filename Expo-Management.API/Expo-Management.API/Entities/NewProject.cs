namespace Expo_Management.API.Entities
{
    public class NewProject
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Lider { get; set; }

        public string Member2 { get; set; }

        public string Member3 { get; set; }

        public IFormFile Files { get; set; }


    }
}
