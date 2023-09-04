using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<NumeroVillaClass> NumeroVillas {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Nombre = "jose Manuel",
                    Detalle = "detalle del producto",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 5,
                    Tarifa = 5,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa()
                {
                    Id = 2,
                    Nombre = "Adrian Perez",
                    Detalle = "detalle dffffel producto",
                    ImagenUrl = "",
                    Ocupantes = 6,
                    MetrosCuadrados = 90,
                    Tarifa = 90,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                }

                );
                

        }

    }
}
