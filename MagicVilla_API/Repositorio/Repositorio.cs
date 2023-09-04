using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> DbSet;

        public Repositorio(ApplicationDbContext db) 
        {
            _db = db;
            this.DbSet = _db.Set<T>();
        }
        public async Task Create(T entity)
        {
           await DbSet.AddAsync(entity);
           await Record();

        }

        public async Task Delete(T entity)
        {
            DbSet.Remove(entity);
            await Record();
        }

        public async Task<List<T>> GetAll(System.Linq.Expressions.Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = DbSet;
            if (filter != null)
                query = query.Where(filter);

            return await query.ToListAsync();
        }

        public async Task<T> GetOne(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = DbSet;

            if (!tracked)
                query = query.AsNoTracking();

            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task Record()
        {
            await _db.SaveChangesAsync();
        }
    }
}
