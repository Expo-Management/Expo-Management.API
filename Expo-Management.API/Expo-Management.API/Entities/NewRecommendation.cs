using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Entities
{
    public class NewRecommendation
    {
        //agregar todo lo que necesito para la recomendacion

        //[Key]
        //public int Id { get; set; }

        public int IdProject { get; set; }

        [Required(ErrorMessage = "Descripcion del proyecto es requerida")]
        public string Recommendation{ get; set; }
        public string correoJuez { get; set; }


    }
}
