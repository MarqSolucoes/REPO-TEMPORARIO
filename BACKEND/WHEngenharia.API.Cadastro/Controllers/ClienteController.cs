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

namespace WHEngenharia.API.Cadastro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoCliente _servicoCliente;

        public ClienteController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoCliente = new ServicoCliente(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<ClienteDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoCliente.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ClienteDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoCliente.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<ClienteDTO>> Post([FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                clienteDTO.IdUsuarioAlteracao = IdUsuario;
                clienteDTO.IdUsuarioCadastro = IdUsuario;
                clienteDTO.DataCadastro = DateTime.Now;
                clienteDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCliente.Post(clienteDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ClienteDTO>> Put([FromBody] ClienteDTO clienteDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                clienteDTO.IdUsuarioAlteracao = IdUsuario;
                clienteDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCliente.Put(clienteDTO);

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

                var clienteDTO = await _servicoCliente.Get(id);
                clienteDTO.DataUltimaAlteracao = DateTime.Now;
                clienteDTO.IdUsuarioAlteracao = IdUsuario;
                clienteDTO.Ativo = ativarDesativar;

                var result = await _servicoCliente.Put(clienteDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
