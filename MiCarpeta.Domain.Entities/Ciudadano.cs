using System.ComponentModel.DataAnnotations;

namespace MiCarpeta.Domain.Entities
{
    public class Ciudadano
    {
        public long Id { get; set; }
        [Required]
        public string Nombres { get; set; }

        [Required]
        public string Apellidos { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        public int TipoDocumento { get; set; }

        [Required]
        public string Correo { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public int IdOperador { get; set; }
    }
}
