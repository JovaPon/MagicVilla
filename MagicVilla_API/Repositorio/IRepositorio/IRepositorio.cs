﻿using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task Create(T entity);

        Task<List<T>> GetAll(Expression<Func<T,bool>>? filter=null);

        Task<T> GetOne(Expression<Func<T, bool>> filter = null, bool tracked=true );

        Task Delete(T entity);

        Task Record();

    }
}
