using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repositorio.IRepositorio
{
    public interface INumeroVillaRepositorio : IRepositorio<NumeroVillaClass> 
    {
        Task<NumeroVillaClass> Actualizar(NumeroVillaClass entidad);
    }
}
