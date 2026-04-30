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

namespace WHEngenharia.API.Compras.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MotivoDevolucaoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoMotivoDevolucao _servicoMotivoDevolucao;

        public MotivoDevolucaoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoMotivoDevolucao = new ServicoMotivoDevolucao(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<MotivoDevolucaoSaldoDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoMotivoDevolucao.Get(apenasAtivos));
        }

        [HttpPost]
        public async Task<ActionResult<MotivoDevolucaoSaldoDTO>> Post([FromBody] MotivoDevolucaoSaldoDTO motivoDevolucaoSaldoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                motivoDevolucaoSaldoDTO.IdUsuarioAlteracao = IdUsuario;
                motivoDevolucaoSaldoDTO.IdUsuarioCadastro = IdUsuario;
                motivoDevolucaoSaldoDTO.DataCadastro = DateTime.Now;
                motivoDevolucaoSaldoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoMotivoDevolucao.Post(motivoDevolucaoSaldoDTO);

                return CreatedAtAction("Post", result);
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

                var materialDTO = await _servicoMotivoDevolucao.Get(id);
                materialDTO.DataUltimaAlteracao = DateTime.Now;
                materialDTO.IdUsuarioAlteracao = IdUsuario;
                materialDTO.Ativo = ativarDesativar;

                var result = await _servicoMotivoDevolucao.Put(materialDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
