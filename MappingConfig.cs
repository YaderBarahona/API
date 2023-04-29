using API.Modelos;
using API.Modelos.Dto;
using AutoMapper;

namespace API
{
    //clase para mapear objetos

    //hereda de Profile propio del paquete automapper
    public class MappingConfig : Profile
    {

        public MappingConfig()
        {
            //crear mapeo indicando la fuente y el destino
            CreateMap<Villa, VIllaDto>();

            //hacemos lo inverso del mapeo anterior
            CreateMap<VIllaDto, Villa>();

            CreateMap<Villa, VIllaCreateDto>().ReverseMap();

            CreateMap<Villa, VIllaUpdateDto>().ReverseMap();
        }
    }
}
