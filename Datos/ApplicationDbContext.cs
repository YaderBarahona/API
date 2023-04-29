using API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace API.Datos
{
    //clase para la creacion de los modelos como tablas en la db

    //hereda de DbContext
    public class ApplicationDbContext : DbContext
    {
        //aplicamos inyeccion de dependencias
        //mediante base indica el padre de donde se hereda "DbContex" mandamos toda la configuracion que se tiene en el servicio mediante la inyeccion de dependencias
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        //modelo villa de tipo dbset se creara en la db como una tabla
        public DbSet<Villa> Villas { get; set; }

        //cambiar caracteristicas del metodo que ya existe de la clase dbcontext
        //agregar datos a la tabla
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //agregamos datos con hasdata 
            //al ejecutar una migracion todos los datos que se agreguen con hasdata se grabaran en la db
            modelBuilder.Entity<Villa>().HasData(
                //new villa para crear un nuevo registro
                new Villa()
                {
                //propiedades
                Id = 1,
                Nombre = "Villa Real",
                Detalle = "Detalle de la villa",
                ImageURL = "",
                Ocupantes = 4,
                MetrosCuadrados = 100,
                Tarifa = 150,
                Amenidad = "",
                FechaCreacion = DateTime.Now,
                FechaActualizacion = DateTime.Now

                },
                 new Villa()
                 {
                     //propiedades
                     Id = 2,
                     Nombre = "Villa Real 2",
                     Detalle = "Detalle de la villa 2",
                     ImageURL = "",
                     Ocupantes = 5,
                     MetrosCuadrados = 1000,
                     Tarifa = 1500,
                     Amenidad = "",
                     FechaCreacion = DateTime.Now,
                     FechaActualizacion = DateTime.Now

                 }
                );
        }
    }
}
