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

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoInternoRecorrenteController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoPedidoInternoRecorrente _servicoPedidoInternoRecorrente;

        public PedidoInternoRecorrenteController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoPedidoInternoRecorrente = new ServicoPedidoInternoRecorrente(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<PedidoInternoRecorrenteDTO>>> Get()
        {
            return Ok(await _servicoPedidoInternoRecorrente.Get());
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<PedidoInternoRecorrenteDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoPedidoInternoRecorrente.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<PedidoInternoRecorrenteDTO>> Post([FromBody] PedidoInternoRecorrenteDTO pedidoInternoRecorrenteDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                pedidoInternoRecorrenteDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoInternoRecorrenteDTO.IdUsuarioCadastro = IdUsuario;
                pedidoInternoRecorrenteDTO.DataCadastro = DateTime.Now;
                pedidoInternoRecorrenteDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoInternoRecorrente.Post(pedidoInternoRecorrenteDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<PedidoInternoRecorrenteDTO>> Put([FromBody] PedidoInternoRecorrenteDTO pedidoInternoRecorrenteDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                pedidoInternoRecorrenteDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoInternoRecorrenteDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoInternoRecorrente.Put(pedidoInternoRecorrenteDTO);

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

                var cargoDTO = await _servicoPedidoInternoRecorrente.Get(id);
                cargoDTO.DataUltimaAlteracao = DateTime.Now;
                cargoDTO.IdUsuarioAlteracao = IdUsuario;
                cargoDTO.Ativo = ativarDesativar;

                var result = await _servicoPedidoInternoRecorrente.Put(cargoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
