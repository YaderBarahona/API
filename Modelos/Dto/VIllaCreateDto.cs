using System.ComponentModel.DataAnnotations;

namespace API.Modelos.Dto
{

    //clase createDto para crear el registro
    public class VIllaCreateDto
    { 
        
        //dataanotation para validar el campo
        [Required]  
        [MaxLength(30)]
        public string Nombre { get; set; }

        public string Detalle { get; set; }

        [Required]
        public double Tarifa { get; set; }

        public int Ocupantes { get; set; }

        public int MetrosCuadrados { get; set; }

        public string ImageURL { get; set; }

        public string Amenidad { get; set; }

    }
}
