using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.PedidoInterno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DefController:ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoDef _servicoDef;

        public DefController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoDef = new ServicoDef(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<DEFDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoDef.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<DEFDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoDef.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<DEFDTO>> Post([FromBody] DEFDTO defDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                defDTO.IdUsuarioAlteracao = IdUsuario;
                defDTO.IdUsuarioCadastro = IdUsuario;
                defDTO.DataCadastro = DateTime.Now;
                defDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoDef.Post(defDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<DEFDTO>> Put([FromBody] DEFDTO defDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                defDTO.IdUsuarioAlteracao = IdUsuario;
                defDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoDef.Put(defDTO);

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

                var defDTO = await _servicoDef.Get(id);
                defDTO.DataUltimaAlteracao = DateTime.Now;
                defDTO.IdUsuarioAlteracao = IdUsuario;
                defDTO.Ativo = ativarDesativar;

                var result = await _servicoDef.Put(defDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
