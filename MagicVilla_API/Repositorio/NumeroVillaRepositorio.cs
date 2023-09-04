using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repositorio.IRepositorio;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVillaClass>, INumeroVillaRepositorio
    {
        private readonly ApplicationDbContext _db;
        public NumeroVillaRepositorio(ApplicationDbContext db) :base(db)
        {
            _db = db;
            
        }
        public async Task<NumeroVillaClass> Actualizar(NumeroVillaClass entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _db.NumeroVillas.Update(entidad);
            await _db.SaveChangesAsync();
            return entidad;
        }
    }
}
