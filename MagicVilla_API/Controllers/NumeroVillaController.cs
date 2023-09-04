using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NumeroVillaController : ControllerBase

    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo,INumeroVillaRepositorio numeroRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                _logger.LogInformation("obtener numero villa");
                IEnumerable<NumeroVillaClass> numeroVillaList = await _numeroRepo.GetAll();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaClassDto>>(numeroVillaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);
            }
            catch (Exception ex) 
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };

            }
            
            return _response;
        }

        [HttpGet("id:int", Name = "GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("check numero villa" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);

                }

                //var Villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                var numeroVilla = await _numeroRepo.GetOne(x => x.VillaNo == id);

                if (numeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response); 
                }

                _response.Resultado = _mapper.Map<NumeroVillaClassDto>(numeroVilla);
                _response.StatusCode = HttpStatusCode.OK;

                return (_response);


            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;
           
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        public async Task<ActionResult<APIResponse>> CreateNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _numeroRepo.GetOne(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("this fild exsi numero villa", "please write again");
                    return BadRequest(ModelState);
                }

                if (await _villaRepo.GetOne(v => v.Id == createDto.VillaId) == null )
                {
                    ModelState.AddModelError("villa no existe clave foranea", "please write again");
                    return BadRequest(ModelState);
                }

                if (createDto == null)
                    return BadRequest();


                NumeroVillaClass modelo = _mapper.Map<NumeroVillaClass>(createDto);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _numeroRepo.Create(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return _response;

        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var NumeroVilla = await _numeroRepo.GetOne(v => v.VillaNo == id);
                if (NumeroVilla == null)
                {
                    _logger.LogError("Villa num not exist");
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                   

                await _numeroRepo.Delete(NumeroVilla);

                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }

            return BadRequest(_response);
         
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            if (id == 0)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }

            if (updateDto == null || updateDto.VillaNo != id)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }

            if (await _villaRepo.GetOne(v => v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("villa no existe clave foranea", "please write again");
                return BadRequest(ModelState);
            }

            //var villadt = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villadt.Nombre = villa.Nombre;
            //villadt.Ocupantes = villa.Ocupantes;
            //villadt.MetrosCuadrados = villa.MetrosCuadrados;

            NumeroVillaClass modelo = _mapper.Map<NumeroVillaClass>(updateDto);

            await _numeroRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
        }

        
    }
}
