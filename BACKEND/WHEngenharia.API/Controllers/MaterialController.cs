using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoMaterial _servicoMaterial;

        public MaterialController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoMaterial = new ServicoMaterial(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<MaterialDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoMaterial.Get(apenasAtivos));
        }

        [HttpPost("ObtemMateriaisPaginados")]
        public async Task<ActionResult> ObtemMateriaisPaginados(PaginacaoDTO paginacao)
        {
            return Ok(await _servicoMaterial.ObtemMateriaisPaginados(paginacao));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<MaterialDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoMaterial.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<MaterialDTO>> Post([FromBody] MaterialDTO materialDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                materialDTO.IdUsuarioAlteracao = IdUsuario;
                materialDTO.IdUsuarioCadastro = IdUsuario;
                materialDTO.DataCadastro = DateTime.Now;
                materialDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoMaterial.Post(materialDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<MaterialDTO>> Put([FromBody] MaterialDTO materialDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                materialDTO.IdUsuarioAlteracao = IdUsuario;
                materialDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoMaterial.Put(materialDTO);

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

                var materialDTO = await _servicoMaterial.Get(id);
                materialDTO.DataUltimaAlteracao = DateTime.Now;
                materialDTO.IdUsuarioAlteracao = IdUsuario;
                materialDTO.Ativo = ativarDesativar;

                var result = await _servicoMaterial.Put(materialDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Unidades")]
        public async Task<ActionResult> Unidades()
        {
            return Ok(await _servicoMaterial.Unidades());
        }

        [HttpGet("BuscaMaterialPorTexto/{texto}")]
        public async Task<ActionResult> BuscaMaterialPorTexto(string texto)
        {
            return Ok(await _servicoMaterial.BuscaMaterialPorTexto(texto));
        }

        [HttpGet("ObtemHistoricoDeCompra/{idMaterial}")]
        public async Task<ActionResult> ObtemHistoricoDeCompra(Int64 idMaterial)
        {
            return Ok(await _servicoMaterial.ObtemHistoricoDeCompra(idMaterial));
        }
    }
}
