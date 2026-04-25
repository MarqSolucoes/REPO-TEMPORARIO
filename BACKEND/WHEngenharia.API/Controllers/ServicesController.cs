

using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ServicesController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoPedidoInternoRecorrente _servicoPedidoInternoRecorrente;
        private ServicoFluxoCaixa _servicoFluxoCaixa;
        private ServicoFaturamento _servicoFaturamento;

        public ServicesController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoPedidoInternoRecorrente = new  ServicoPedidoInternoRecorrente(_context, _mapper);
            _servicoFluxoCaixa = new ServicoFluxoCaixa(_context, _mapper);
            _servicoFaturamento = new ServicoFaturamento(_context, _mapper);
        }

        [AllowAnonymous]
        [HttpPost("GeraPedidosInternos")]
        public async Task<ActionResult> GeraPedidosInternos()
        {
            try
            {
                await _servicoPedidoInternoRecorrente.GeraPedidosInternos();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("GeraPrevisaoFluxoCaixa")]
        public async Task<ActionResult> GeraPrevisaoFluxoCaixa()
        {
            try
            {
                await _servicoFluxoCaixa.GeraPrevisaoFluxoCaixa();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("AjustaDataFaturamento")]
        public async Task<ActionResult> AjustaDataFaturamento()
        {
            try
            {
                await _servicoFaturamento.AjustaDataFaturamento();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
