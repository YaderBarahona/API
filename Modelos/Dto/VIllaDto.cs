using System.ComponentModel.DataAnnotations;

namespace API.Modelos.Dto
{

    //clase DTO para incluir las propiedades que se quieren mostrar cuando se expongan los datos
    //no sera una tabla en la db solo sera usado para trabajar con este modelo en el controlador para temas de presentacion y asi evitar trabajar directamente con el modelo de la db
    //contiene los mismos datos que el modelo
    public class VIllaDto
    { 
        
        public int Id { get; set; }

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
