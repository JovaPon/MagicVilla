﻿using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.Dto;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase

    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
        {
            _logger = logger;
            _villaRepo = villaRepo;
            _mapper = mapper;
            _response = new();
        }


        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVilla()
        {
            try
            {
                _logger.LogInformation("loadinformation");
                IEnumerable<Villa> villaList = await _villaRepo.GetAll();

                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
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

        [HttpGet("id:int", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("check" + id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);

                }

                //var Villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                var Villa = await _villaRepo.GetOne(x => x.Id == id);

                if (Villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response); 
                }

                _response.Resultado = _mapper.Map<VillaDto>(Villa);
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

        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody] VillaCreateDto villa)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                if (await _villaRepo.GetOne(v => v.Nombre.ToLower() == villa.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("this fild exsi", "please write again");
                    return BadRequest(ModelState);
                }

                if (villa == null)
                    return BadRequest();


                Villa modelo = _mapper.Map<Villa>(villa);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await _villaRepo.Create(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;

                return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
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
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villaRepo.GetOne(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                   

                await _villaRepo.Delete(villa);

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
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDto villa)
        {
            if (id == 0)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }

            if (villa == null || villa.Id == 0)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest();
            }
                

            //var villadt = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villadt.Nombre = villa.Nombre;
            //villadt.Ocupantes = villa.Ocupantes;
            //villadt.MetrosCuadrados = villa.MetrosCuadrados;

            Villa modelo = _mapper.Map<Villa>(villa);

            await _villaRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);
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

            var villa = await _villaRepo.GetOne(v => v.Id == id, tracked:false );

            VillaUpdateDto villadto = _mapper.Map<VillaUpdateDto>(villa);

            if (villa == null)
                return BadRequest();


            patchVilla.ApplyTo(villadto, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            Villa modelo = _mapper.Map<Villa>(villadto);

            await _villaRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;

            return Ok(_response);

        }
    }
}
