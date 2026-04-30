using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Fornecedor;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Cadastro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FornecedorController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoFornecedor _servicoFornecedor;

        public FornecedorController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoFornecedor = new ServicoFornecedor(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<FornecedorDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoFornecedor.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FornecedorDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoFornecedor.Get(Id));
        }

        [HttpPost("GetFiltrado")]
        public async Task<ActionResult> GetFiltrado([FromBody] Fornecedor_FiltroDTO fornecedor_FiltroDTO)
        {
            try
            {
                return Ok(await _servicoFornecedor.GetFiltrado(fornecedor_FiltroDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<FornecedorDTO>> Post([FromBody] FornecedorDTO fornecedorDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                fornecedorDTO.IdUsuarioAlteracao = IdUsuario;
                fornecedorDTO.IdUsuarioCadastro = IdUsuario;
                fornecedorDTO.DataCadastro = DateTime.Now;
                fornecedorDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoFornecedor.Post(fornecedorDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<FornecedorDTO>> Put([FromBody] FornecedorDTO fornecedorDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                fornecedorDTO.IdUsuarioAlteracao = IdUsuario;
                fornecedorDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoFornecedor.Put(fornecedorDTO);

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

                var fornecedorDTO = await _servicoFornecedor.Get(id);
                fornecedorDTO.DataUltimaAlteracao = DateTime.Now;
                fornecedorDTO.IdUsuarioAlteracao = IdUsuario;
                fornecedorDTO.Ativo = ativarDesativar;

                var result = await _servicoFornecedor.Put(fornecedorDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtemFiliais/{Id}")]
        public async Task<ActionResult> ObtemFiliais([FromRoute] Int64 Id)
        {
            try
            {
                var result = await _servicoFornecedor.ObtemFiliais(Id);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
