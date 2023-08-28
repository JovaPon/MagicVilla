using MagicVilla_API.Modelos.Dto;

namespace MagicVilla_API.Datos
{
    public class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        { 
            new VillaDto {Id=1, Nombre="hello", Ocupantes=2, MetrosCuadrados = 5 },
            new VillaDto {Id=2,Nombre="new new" , Ocupantes=2, MetrosCuadrados = 4  }
        };
    }
}
