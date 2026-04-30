using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Compras.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoCompraNotaFiscalController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoPedidoCompraNotaFiscal _servicoPedidoCompraNotaFiscal;
        private ServicoObra _servicoObra;

        public PedidoCompraNotaFiscalController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoPedidoCompraNotaFiscal = new ServicoPedidoCompraNotaFiscal(_context, _mapper);
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] Int64 id)
        {
            try
            {
                var result = await _servicoPedidoCompraNotaFiscal.Get(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
    }
}