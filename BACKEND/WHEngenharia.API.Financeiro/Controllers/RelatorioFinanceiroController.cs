using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Financeiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioFinanceiroController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoRelatorioFinanceiro _servicoRelatorioFinanceiro;

        public RelatorioFinanceiroController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoRelatorioFinanceiro = new ServicoRelatorioFinanceiro(_context, _mapper);
        }

        [HttpGet("Pdf/Download")]
        public IActionResult DownloadPdfPedidoCompra()
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoRelatorioFinanceiro.GeraPdfPedidoCompra(0, logoPath).Result,
                    "application/pdf",
                    "PedidoCompra.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
