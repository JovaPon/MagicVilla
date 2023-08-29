using AutoMapper;
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
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDto>>> GetVilla()
        {
            _logger.LogInformation("loadinformation");

            IEnumerable<Villa> villaList = await _db.Villas.ToListAsync();

            return Ok(_mapper.Map<VillaDto>(villaList));
        }

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("check");
                return BadRequest();
            }

            //var Villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            var Villa = await _db.Villas.FirstOrDefaultAsync (x => x.Id == id);

            if (Villa == null) { return NotFound(); }

            return Ok(_mapper.Map<VillaDto>(Villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<VillaDto>> CreateVilla([FromBody] VillaCreateDto villa)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _db.Villas.FirstOrDefaultAsync(v => v.Nombre.ToLower() == villa.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("this fild exsi", "please write again");
                return BadRequest(ModelState);
            }

            if (villa == null)
                return BadRequest();


            Villa modelo = _mapper.Map<Villa>(villa);


            await _db.Villas.AddAsync(modelo);
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, modelo);

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa = await _db.Villas.FirstOrDefaultAsync(v => v.Id == id);
            if (villa == null)
                return NotFound();

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villa)
        {
            if (id == 0)
                return BadRequest();

            if (villa == null || villa.Id == 0)
                return BadRequest();

            //var villadt = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villadt.Nombre = villa.Nombre;
            //villadt.Ocupantes = villa.Ocupantes;
            //villadt.MetrosCuadrados = villa.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(villa);

            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPatch("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchVilla) 
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (patchVilla == null || id == 0)
                return BadRequest();

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDto villadto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null)
                return BadRequest();


            patchVilla.ApplyTo(villadto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Villa modelo = _mapper.Map<Villa>(villadto);


            _db.Villas.Update(modelo);
            await _db.SaveChangesAsync();


            return NoContent();

        }

       

    }
}
