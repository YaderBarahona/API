using System.ComponentModel.DataAnnotations;

namespace API.Modelos.Dto
{

    //clase updateDto para actualizar el registro
    public class VIllaUpdateDto
    {
        [Required]
        public int Id { get; set; }

        //dataanotation para validar el campo
        [Required]  
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        [Required]
        public int Ocupantes { get; set; }

        [Required]
        public int MetrosCuadrados { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public string Amenidad { get; set; }

    }
}
