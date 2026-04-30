using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Financeiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FinanceiroController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoFinanceiro _servicoFinanceiro;

        public FinanceiroController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoFinanceiro = new ServicoFinanceiro(_context, _mapper);
        }

        [HttpGet("ObtemEntradas")]
        public async Task<ActionResult> ObtemEntradas()
        {
            try
            {
                return Ok(await _servicoFinanceiro.ObtemEntradas());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObtemEntradasRecusadas")]
        public async Task<ActionResult> ObtemEntradasRecusadas()
        {
            try
            {
                return Ok(await _servicoFinanceiro.ObtemEntradasRecusadas());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AutorizaEntrada")]
        public async Task<ActionResult> AutorizaEntrada([FromBody] Financeiro_EntradaDTO entradaDTO)
        {
            try
            {
                await _servicoFinanceiro.AutorizaEntrada(entradaDTO);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CancelaEntrada")]
        public async Task<ActionResult> CancelaEntrada([FromBody] Financeiro_EntradaDTO entradaDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoFinanceiro.CancelaEntrada(entradaDTO, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
