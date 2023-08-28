using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase

    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }


        [HttpGet]
        public ActionResult<IEnumerable<VillaDto>> GetVilla()
        {
            _logger.LogInformation("loadinformation");
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("check");
                return BadRequest();
            }

            //var Villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            var Villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (Villa == null) { return NotFound(); }

            return Ok(Villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public ActionResult<VillaDto> CreateVilla([FromBody] VillaDto villa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (_db.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villa.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("this fild exsi", "please write again");
                return BadRequest(ModelState);
            }

            if (villa == null)
                return BadRequest();

            if (villa.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);


            Villa modelo = new()
            {
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };

            _db.Villas.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (villa == null)
                return NotFound();

            _db.Villas.Remove(villa);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villa)
        {
            if (id == 0)
                return BadRequest();

            if (villa == null || villa.Id == 0)
                return BadRequest();

            //var villadt = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villadt.Nombre = villa.Nombre;
            //villadt.Ocupantes = villa.Ocupantes;
            //villadt.MetrosCuadrados = villa.MetrosCuadrados;


            Villa modelo = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();


            return NoContent();


        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchVilla) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (patchVilla == null || id == 0)
                return BadRequest();

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(v => v.Id == id);

            VillaDto villadto = new()
            {
                Id = villa.Id,
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };

            if (villa == null)
                return BadRequest();


            patchVilla.ApplyTo(villadto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Villa modelo = new()
            {
                Id = villadto.Id,
                Nombre = villadto.Nombre,
                Detalle = villadto.Detalle,
                ImagenUrl = villadto.ImagenUrl,
                Ocupantes = villadto.Ocupantes,
                Tarifa = villadto.Tarifa,
                MetrosCuadrados = villadto.MetrosCuadrados,
                Amenidad = villadto.Amenidad
            };

            _db.Villas.Update(modelo);
            _db.SaveChanges();


            return NoContent();

        }

       

    }
}
