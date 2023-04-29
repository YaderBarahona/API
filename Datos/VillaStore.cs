using API.Modelos.Dto;

namespace API.Datos
{
    //clase "store" para dactos fictisios y acceder a ellos
    public static class VillaStore
    {
        public static List<VIllaDto> villaList = new List<VIllaDto>() 
        { 
            new VIllaDto{Id=1, Nombre="Vista piscina", Ocupantes=3, MetrosCuadrados=100},
            new VIllaDto{Id=2, Nombre="Vista Playa", Ocupantes=4, MetrosCuadrados=50}
        };
    }
}
