using System.ComponentModel.DataAnnotations;

namespace Expo_Management.API.Domain.Models.Entities
{
    public class SecurityProtocols
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;

        [StringLength(250, ErrorMessage = "La descripción del protocolo puede ser maximo de 250 caracteres.")]
        public string Description { get; set; } = default!;
        public Fair? Fair { get; set; } = default!;
    }
}
    