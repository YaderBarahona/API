using API.Datos;
using API.Modelos;
using API.Modelos.Dto;
using API.Repositorio.IRepositorio;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers 
{
    //ruta
    [Route("api/[controller]")]
    //data anotation para indicar que es una clase controlador de tipo api 
    [ApiController]
    public class VillaController : ControllerBase
    {
        //variable logger
        private readonly ILogger<VillaController> _logger;

        //inyeccion de dbcontext
        //private readonly ApplicationDbContext _context;

        private readonly IVillaRepositorio _villaRepo;


        //inyeccion de automapper para el mapeo de objetos 
        private readonly IMapper _mapper;

        //inyeccion de logger
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            //inicializamos la variable que hace referencia al servicio de logger
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;

        }

        //tipo de verbo o metodo http 
        //httpGet ya que vamos a retornar una lista 
        //endpoint que retornara una lista completa "GetAll"
        //interfaz Actionresult para poder utilizar cualquier tipo de retorno que queramos
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //metodo asincrono seguido de Task 
        //colocar await antes del async dentro del metodo en las funciones reconocidas por entityframeworkcore
        public async Task<ActionResult<IEnumerable<VIllaDto>>> GetVillas()
        {

            //mensaje de informacion en la consola
            _logger.LogInformation("Obtener las villas");

            //lista para utilizar el mapper
            //IEnumerable<Villa> villaList = await _context.Villas.ToListAsync();

            IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();


            //aplicamos el mapeo con la variable _mapper retornando un IEnumerable de tipo villaDto con el source de villaList
            return Ok(_mapper.Map<IEnumerable<VIllaDto>>(villaList));

            //traemos en forma de lista los registros de la tabla
            //return Ok(await _context.Villas.ToListAsync());

            //retornamos el store simulado
            //Ok para indicar el codigo 200
            //return Ok(VillaStore.villaList);   

            //return new List<VIllaDto>
            //{
            //    new VIllaDto {Id=1, Nombre="Vista piscina"},
            //    new VIllaDto {Id=2, Nombre="Vista Playa"}

            //};
        }


        //httpGet por id ya que vamos a retornar una villa especifica de la lista enviando el parametro id
        //Name para podernos dirigirnos a esta ruta
        [HttpGet("id", Name="GetVilla")]
        //documentamos el tipo de codigo de respuesta mediante ProducesResponseType
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //endpoint que retornara una villa de la lista "GetById"
        public async Task<ActionResult<VIllaDto>> GetVilla(int id)
        {
            //validacion para retornar codigo 400 badrequest
            if (id == 0)
            {
                _logger.LogError("Error con la villa con id: " + id);
                return BadRequest();
            }

            //obtenemos de store el parametro id
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //traemos un solo registro de las villas almacenadas en la db en base al id
            //var villa = await _context.Villas.FirstOrDefaultAsync(x => x.Id == id);

            var villa = await _villaRepo.Obtener(x => x.Id == id);


            //validacion para retornar codigo 404 notfound
            if (villa==null)
            {
                return NotFound();
            }

            
            return Ok(_mapper.Map<VIllaDto>(villa));

            //accedemos a la lista y al metodo "FirstOrDefault" para traer del listado el la villa por id por medio de una expresion lambda comparando los id's
            //return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //atributo FromBody indica que vamos a recibir datos, el tipo de modelo
        //villaCreateDto para crear el registro
        public async Task<ActionResult<VIllaDto>> CrearVilla([FromBody] VIllaCreateDto createDto)
        {
            //validar si el modelo no es valido 
            //el modelo esta conectado a lo que se declaro en el Actionresult
            if(!ModelState.IsValid)
            {
                //retornamos badrequest del modelstate para que no se grabe el registro
                return BadRequest(ModelState);
            }

            //validacion personalizada
            //primer registro que encuentre donde el nombre estando en minuscula o mayuscula es igual a lo que se esta recibiendo como parametro en villaDto.nombre es diferente de null entonces encontro un registro que coincide con el registro que se esta intentando ingresar
            if(await _villaRepo.Obtener(v=>v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                //ModelState personalizado con AddModelError con dos parametros "nombre de la validacion", "mensaje que se quiere mostrar"
                ModelState.AddModelError("NombreExiste","La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            if (createDto == null)
            {
                return BadRequest(createDto);        
            }

            var modelo = _mapper.Map<Villa>(createDto);

            //if (villaDto.Id > 0)
            //{
            //    return StatusCode(StatusCodes.Status500InternalServerError); 
            //}

            //generamos el id con el primer registro que encuentre de la lista del store ordenado descendentemente en este caso solo el id y no todo el modelo e incrementamos en 1 
            //villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;    
            //agregamos el nuevo registro a la lista
            //VillaStore.villaList.Add(villaDto);

            //modelo en base a lo que se recibe en el dto
            //Villa modelo = new()
            //{
            //    //llenamos las propiedades del modelo
            //    Nombre = createDto.Nombre,
            //    Detalle = createDto.Detalle,
            //    ImageURL = createDto.ImageURL,
            //    Ocupantes = createDto.Ocupantes,
            //    Tarifa = createDto.Tarifa,
            //    MetrosCuadrados = createDto.MetrosCuadrados,
            //    Amenidad = createDto.Amenidad
            //};

            //agregamos el registro a la DB
            await _villaRepo.Crear(modelo);

            //guardamos los cambios
            //await _context.SaveChangesAsync();

            //retornamos el registro nuevo mediante la ruta
            //indicamos la ruta de creacion "getbyid" mediante el nombre de la ruta, el id y el modelo completo
            return CreatedAtRoute("GetVIlla", new {id=modelo.Id}, createDto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //IActionResult ya que no se va a necesitar el modelo
        public async Task<IActionResult> Delete(int id)
        {
            if (id==0)
            {
                return BadRequest();
            }

            var villa = await _villaRepo.Obtener(v => v.Id == id);

            if(villa==null)
            {
                //no encontrada
                return NotFound();
            }

            //VillaStore.villaList.Remove(villa);

            //eliminamos de la db por medio del _context y el metodo remove
            _villaRepo.Remover(villa);

            //guardamos cambios
            //await _context.SaveChangesAsync();

            //retornamos nocontent al ser delete
            return NoContent();
        }

        //httpPut   
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult>UpdateVilla(int id, [FromBody] VIllaUpdateDto updateDto)
        {
            if (updateDto == null || id != updateDto.Id)
            {
                return BadRequest();
            }

            var modelo = _mapper .Map<Villa>(updateDto); 

            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //villa.Nombre = villaDto.Nombre;
            //villa.Ocupantes = villaDto.Ocupantes;
            //villa.MetrosCuadrados = villaDto.MetrosCuadrados;

            //modelo en base a lo que se recibe en el dto
            //Villa modelo = new()
            //{
            //    //llenamos las propiedades del modelo
            //    Id = updateDto.Id,
            //    Nombre = updateDto.Nombre,
            //    Detalle = updateDto.Detalle,
            //    ImageURL = updateDto.ImageURL,
            //    Ocupantes = updateDto.Ocupantes,
            //    Tarifa = updateDto.Tarifa,
            //    MetrosCuadrados = updateDto.MetrosCuadrados,
            //    Amenidad = updateDto.Amenidad
            //};

            //actualizamos el modelo
            _villaRepo.Actualizar(modelo);

            //guardamos cambios
            //await _context.SaveChangesAsync();

            return NoContent();

        }

        //httpPatch   
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateParcialVilla(int id, JsonPatchDocument<VIllaUpdateDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            //capturamos el registro anterior
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            //variable villa para capturar el registro que se va a modificar en base al id que se esta enviando
            //AsNoTracking para consultar un registro del dbcontext y no se trackee 
            //var villa = await _context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            var villa = await _villaRepo.Obtener(v => v.Id == id, tracked:false);


            VIllaUpdateDto villaDto = _mapper.Map<VIllaUpdateDto>(villa);

            //modelo de tipo villaDto para llenar cada una de las propiedades en base a la variable villa, antes de que se actualice el registro ponemos temporalmente en el modelo villaDto y cada unas de las propiedades se llenan en base al registro actual
            //VIllaUpdateDto vIllaDto = new()
            //{
            //    Id = villa.Id,
            //    Nombre = villa.Nombre,
            //    Detalle = villa.Detalle,
            //    ImageURL = villa.ImageURL,
            //    Ocupantes = villa.Ocupantes,
            //    Tarifa = villa.Tarifa,
            //    MetrosCuadrados = villa.MetrosCuadrados,
            //    Amenidad = villa.Amenidad
            //};

            //validacion en caso de que la villa no exista
            if (villa == null)
            {
                return BadRequest();
            }

            //patchDto.ApplyTo(villa, ModelState);

            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(villaDto);

            ////modelo que contendra las mismas propiedades del villaDto luego de pasar el applyto que ya contiene lo unico que se va a modificar y se lo devuelve al modelo de tipo villa
            ////es el que se enviara al metodo update dentro del dbcontex
            //Villa modelo = new()
            //{
            //    Id = vIllaDto.Id,
            //    Nombre = vIllaDto.Nombre,
            //    Detalle = vIllaDto.Detalle,
            //    ImageURL = vIllaDto.ImageURL,
            //    Ocupantes = vIllaDto.Ocupantes,
            //    Tarifa = vIllaDto.Tarifa,
            //    MetrosCuadrados = vIllaDto.MetrosCuadrados,
            //    Amenidad = vIllaDto.Amenidad
            //};

            //actualizamos la propiedad enviando al modelo de tipo villa
            _villaRepo.Actualizar(modelo);

            //guardamos cambios
            //await _villaRepo.SaveChangesAsync();

            return NoContent();

        }
    }
}
