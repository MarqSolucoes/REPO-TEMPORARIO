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

namespace WHEngenharia.API.Auth.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoUsuario _servicoUsuario;

        public UsuarioController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoUsuario = new ServicoUsuario(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoUsuario.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<UsuarioDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoUsuario.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<UsuarioDTO>> Post([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                usuarioDTO.IdUsuarioAlteracao = IdUsuario;
                usuarioDTO.IdUsuarioCadastro = IdUsuario;
                usuarioDTO.DataCadastro = DateTime.Now;
                usuarioDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoUsuario.Post(usuarioDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<UsuarioDTO>> Put([FromBody] UsuarioDTO usuarioDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                usuarioDTO.IdUsuarioAlteracao = IdUsuario;
                usuarioDTO.DataUltimaAlteracao = DateTime.Now;
                usuarioDTO.Senha = null;

                var result = await _servicoUsuario.Put(usuarioDTO);

                return CreatedAtAction("Put", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("HabilitaDesabilitaLogin")]
        public async Task<ActionResult> HabilitaDesabilitaLogin(Int64 id, bool habilitaDesabilitaLogin)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var usuarioDTO = await _servicoUsuario.Get(id);
                usuarioDTO.DataUltimaAlteracao = DateTime.Now;
                usuarioDTO.IdUsuarioAlteracao = IdUsuario;
                usuarioDTO.HabilitaLogin = habilitaDesabilitaLogin;

                var result = await _servicoUsuario.Put(usuarioDTO);

                return Ok(result);
            }
            catch(Exception ex)
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

                var usuarioDTO = await _servicoUsuario.Get(id);
                usuarioDTO.DataUltimaAlteracao = DateTime.Now;
                usuarioDTO.IdUsuarioAlteracao = IdUsuario;
                usuarioDTO.Ativo = ativarDesativar;

                var result = await _servicoUsuario.Put(usuarioDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AlterarSenha")]
        public async Task<ActionResult> AlterarSenha(Int64 id, string senha)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var usuarioDTO = await _servicoUsuario.Get(id);
                usuarioDTO.DataUltimaAlteracao = DateTime.Now;
                usuarioDTO.IdUsuarioAlteracao = IdUsuario;
                usuarioDTO.Senha = senha;

                var result = await _servicoUsuario.Put(usuarioDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Engenheiros")]
        public async Task<ActionResult> Engenheiros()
        {
            try
            {
                return Ok(await _servicoUsuario.Engenheiros());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Diretores")]
        public async Task<ActionResult<List<UsuarioDTO>>> Diretores()
        {
            try
            {
                return Ok(await _servicoUsuario.Diretores());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
    }
}
