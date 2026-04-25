    using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.FluxoCaixa;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FluxoCaixaController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoFluxoCaixa _servicoFluxoCaixa;

        public FluxoCaixaController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoFluxoCaixa = new ServicoFluxoCaixa(_context, _mapper);
        }

        [HttpPost("ObtemFluxosDoDia")]
        public async Task<ActionResult> ObtemFluxosDoDia(FluxoCaixaRequestDTO request)
        {
            try
            {
                return Ok(await _servicoFluxoCaixa.ObtemFluxosDoDia(new DateTime(Convert.ToInt32(request.Data.Split('/')[2]), Convert.ToInt32(request.Data.Split('/')[1]), Convert.ToInt32(request.Data.Split('/')[0])), request.Credito));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("InformarPagamentoOuRecebimento")]
        public async Task<ActionResult> InformarPagamentoOuRecebimento([FromBody]FluxoCaixa_DataPagamentoRecebimentoParameterDTO parameterDTO)
        {
            try
            {
                await _servicoFluxoCaixa.InformarPagamentoOuRecebimento(parameterDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }


        [HttpPost("ObtemFluxoCaixaConsolidado")]
        public async Task<ActionResult> ObtemFluxoCaixaConsolidado(FluxoCaixaRequestDTO request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Data))
                    request.Data = DateTime.Now.ToString("MM/yyyy");

                DateTime dataInicial = new DateTime(Convert.ToInt32(request.Data.Split('/')[1]), Convert.ToInt32(request.Data.Split('/')[0]), 1);
                DateTime? dataFinal = string.IsNullOrEmpty(request.DataFinal) ? null : new DateTime(Convert.ToInt32(request.DataFinal.Split('/')[1]), Convert.ToInt32(request.DataFinal.Split('/')[0]), 1).AddMonths(1).AddMilliseconds(-1);
                
                return Ok(await _servicoFluxoCaixa.ObtemFluxoCaixaConsolidado(dataInicial, dataFinal));
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPut()]
        public async Task<ActionResult> Put(FluxoCaixaDTO fluxoCaixaDTO)
        {
            try
            {
                return Ok(await _servicoFluxoCaixa.Put(fluxoCaixaDTO));
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("CadastraNovosValoresOCPI")]
        public async Task<ActionResult> CadastraNovosValoresOCPI(FluxoCaixa_NovosValoresOCPIDTO fluxoCaixa_NovosValoresOCPIDTO)
        {
            try
            {
                await _servicoFluxoCaixa.CadastraNovosValoresOCPI(fluxoCaixa_NovosValoresOCPIDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost("Excel/Download")]
        public IActionResult DownloadExcel(FluxoCaixaRequestDTO parametros)
        {
            try
            {
                if (string.IsNullOrEmpty(parametros.Data))
                    parametros.Data = DateTime.Now.ToString("MM/yyyy");

                DateTime dataInicial = new DateTime(Convert.ToInt32(parametros.Data.Split('/')[1]), Convert.ToInt32(parametros.Data.Split('/')[0]), 1);
                DateTime? dataFinal = string.IsNullOrEmpty(parametros.DataFinal) ? null : new DateTime(Convert.ToInt32(parametros.DataFinal.Split('/')[1]), Convert.ToInt32(parametros.DataFinal.Split('/')[0]), 1).AddMonths(1).AddMilliseconds(-1);

                var result = _servicoFluxoCaixa.ObtemFluxoCaixaConsolidado(dataInicial, dataFinal).Result;

                using var workbook = new XLWorkbook();

                var worksheetFaturado = workbook.Worksheets.Add("FluxoCaixa");

                worksheetFaturado.Cell(1, 1).Value = "Dia";
                worksheetFaturado.Cell(1, 2).Value = "OC/PI";
                worksheetFaturado.Cell(1, 3).Value = "NF";
                worksheetFaturado.Cell(1, 4).Value = "ETO";
                worksheetFaturado.Cell(1, 5).Value = "Folha Pagamento";
                worksheetFaturado.Cell(1, 6).Value = "Imposto";
                worksheetFaturado.Cell(1, 7).Value = "Despesas Fixas";
                worksheetFaturado.Cell(1, 8).Value = "Reserva";
                worksheetFaturado.Cell(1, 9).Value = "Outros";
                worksheetFaturado.Cell(1, 10).Value = "Transferência";
                worksheetFaturado.Cell(1, 11).Value = "Total Diário";
                worksheetFaturado.Cell(1, 12).Value = "A Receber Faturado";
                worksheetFaturado.Cell(1, 13).Value = "A Receber a Faturar";
                worksheetFaturado.Cell(1, 14).Value = "Estornos/Créditos";
                worksheetFaturado.Cell(1, 15).Value = "Saldo";

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetFaturado.Cell(index + 1, 1).Value = result[index - 1].Dia?.ToString("dd/MM/yyyy") ?? "";
                    worksheetFaturado.Cell(index + 1, 2).Value = result[index - 1].OrdemCompraPedidoInterno;
                    worksheetFaturado.Cell(index + 1, 3).Value = result[index - 1].NotaFiscal;
                    worksheetFaturado.Cell(index + 1, 4).Value = result[index - 1].Eto;
                    worksheetFaturado.Cell(index + 1, 5).Value = result[index - 1].folhaPagamento;
                    worksheetFaturado.Cell(index + 1, 6).Value = result[index - 1].imposto;
                    worksheetFaturado.Cell(index + 1, 7).Value = result[index - 1].despesasFixas;
                    worksheetFaturado.Cell(index + 1, 8).Value = result[index - 1].reserva;
                    worksheetFaturado.Cell(index + 1, 9).Value = result[index - 1].outros;
                    worksheetFaturado.Cell(index + 1, 10).Value = result[index - 1].transferencia;
                    worksheetFaturado.Cell(index + 1, 11).Value = result[index - 1].totalDiario;
                    worksheetFaturado.Cell(index + 1, 12).Value = result[index - 1].aReceberFaturado;
                    worksheetFaturado.Cell(index + 1, 13).Value = result[index - 1].aReceberAFaturar;
                    worksheetFaturado.Cell(index + 1, 14).Value = result[index - 1].estornos;
                    worksheetFaturado.Cell(index + 1, 15).Value = result[index - 1].saldo;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Fluxocaixa.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PDF/Download")]
        public IActionResult DownloadPDF(FluxoCaixaRequestDTO parametros)
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoFluxoCaixa.DownloadPDF(logoPath, parametros).Result,
                    "application/pdf",
                    "EtoObra.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
