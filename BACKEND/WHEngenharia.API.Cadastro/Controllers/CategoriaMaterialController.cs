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
    public class CategoriaMaterialController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoCategoriaMaterial _servicoCategoriaMaterial;

        public CategoriaMaterialController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoCategoriaMaterial = new ServicoCategoriaMaterial(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaMaterialDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoCategoriaMaterial.Get(apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<CategoriaMaterialDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoCategoriaMaterial.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<CategoriaMaterialDTO>> Post([FromBody] CategoriaMaterialDTO categoriaMaterialDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                categoriaMaterialDTO.IdUsuarioAlteracao = IdUsuario;
                categoriaMaterialDTO.IdUsuarioCadastro = IdUsuario;
                categoriaMaterialDTO.DataCadastro = DateTime.Now;
                categoriaMaterialDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCategoriaMaterial.Post(categoriaMaterialDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CategoriaMaterialDTO>> Put([FromBody] CategoriaMaterialDTO categoriaMaterialDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                categoriaMaterialDTO.IdUsuarioAlteracao = IdUsuario;
                categoriaMaterialDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoCategoriaMaterial.Put(categoriaMaterialDTO);

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

                var categoriaMaterialDTO = await _servicoCategoriaMaterial.Get(id);
                categoriaMaterialDTO.DataUltimaAlteracao = DateTime.Now;
                categoriaMaterialDTO.IdUsuarioAlteracao = IdUsuario;
                categoriaMaterialDTO.Ativo = ativarDesativar;

                var result = await _servicoCategoriaMaterial.Put(categoriaMaterialDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
