using API.Datos;
using API.Modelos;

namespace API.Repositorio.IRepositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly ApplicationDbContext _context;

        //pasamos el dbcontext de hijo a padre con base
        public VillaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        //cada metodo actualizar funciona diferente segun la entidad
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;

            _context.Update(entidad);

            await _context.SaveChangesAsync();

            return entidad;
        }
    }
}
