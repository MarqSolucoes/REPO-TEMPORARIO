using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CargoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoCargo _servicoCargo;

        public CargoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoCargo = new ServicoCargo(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<CargoDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoCargo.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CargoDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoCargo.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CargoDTO>> Post([FromBody] CargoDTO cargoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                cargoDTO.IdUsuarioAlteracao = IdUsuario;
                cargoDTO.IdUsuarioCadastro = IdUsuario;
                cargoDTO.DataCadastro = DateTime.Now;
                cargoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCargo.Post(cargoDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CargoDTO>> Put([FromBody] CargoDTO cargoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                cargoDTO.IdUsuarioAlteracao = IdUsuario;
                cargoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCargo.Put(cargoDTO);

                return CreatedAtAction("Put", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AtivarDesativar")]
        public async Task<ActionResult> AtivarDesativar(Int64 id, bool ativarDesativar)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var cargoDTO = await _servicoCargo.Get(id);
                cargoDTO.DataUltimaAlteracao = DateTime.Now;
                cargoDTO.IdUsuarioAlteracao = IdUsuario;
                cargoDTO.Ativo = ativarDesativar;

                var result = await _servicoCargo.Put(cargoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
