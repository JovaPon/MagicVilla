using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Modelos.Dto
{
    public class NumeroVillaClassDto
    {
        public int VillaNo { get; set; }

        public int VillaId { get; set; }

        public string DetalleEspecial { get; set; }
    }
}
