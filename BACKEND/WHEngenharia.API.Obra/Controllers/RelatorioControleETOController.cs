using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos.Relatorio;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Obra.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class RelatorioControleETOController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoRelatorioETO _servicoRelatorioETO;

        public RelatorioControleETOController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoRelatorioETO = new ServicoRelatorioETO(_context, _mapper);
        }

        [HttpGet("{obraBloqueada}")]
        public async Task<ActionResult> Get([FromRoute] bool obraBloqueada)
        {
            return Ok(await _servicoRelatorioETO.Get(obraBloqueada));
        }

        [HttpGet("GetProximasDatasAjuste/{idObra}")]
        public async Task<ActionResult> GetProximasDatasAjuste([FromRoute] int idObra)
        {
            return Ok(await _servicoRelatorioETO.GetProximasDatasAjuste(idObra));
        }

        //[HttpPost("Ajuste")]
        //public async Task<ActionResult> Ajuste(Relatorio_ControleEtoAjusteDTO relatorioControleEtoAjusteDTO)
        //{
        //    var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

        //    relatorioControleEtoAjusteDTO.IdUsuario = IdUsuario;

        //    return Ok(await _servicoRelatorioETO.Ajuste(relatorioControleEtoAjusteDTO));
        //}

        [HttpPost("VisualizarComentario/{idAjuste}")]
        public async Task<ActionResult> VisualizarComentario([FromRoute] Int64 idAjuste)
        {
            return Ok(await _servicoRelatorioETO.VisualizarComentario(idAjuste));
        }

        [HttpGet("Pdf/Download")]
        public IActionResult DownloadPdf()
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(_servicoRelatorioETO.GeraPdf(logoPath).Result, "application/pdf", "ControleETO.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Excel/Download")]
        public IActionResult DownloadExcel()
        {
            try
            {
                var result = _servicoRelatorioETO.Get(true).Result;

                using var workbook = new XLWorkbook();

                var worksheetETO = workbook.Worksheets.Add("ControleETO");

                worksheetETO.Cell(1, 1).Value = "Obra";
                worksheetETO.Cell(1, 2).Value = "Cliente";
                worksheetETO.Cell(1, 3).Value = "Descrição";
                worksheetETO.Cell(1, 4).Value = "Custo";
                worksheetETO.Cell(1, 5).Value = "Gasto";
                worksheetETO.Cell(1, 6).Value = "Saldo";

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetETO.Cell(index + 1, 1).Value = result[index - 1].Codigo;
                    worksheetETO.Cell(index + 1, 2).Value = result[index - 1].Cliente;
                    worksheetETO.Cell(index + 1, 3).Value = result[index - 1].Descricao;
                    worksheetETO.Cell(index + 1, 4).Value = result[index - 1].Custo;
                    worksheetETO.Cell(index + 1, 5).Value = result[index - 1].Gasto;
                    worksheetETO.Cell(index + 1, 6).Value = result[index - 1].Saldo;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ControleETO.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ExcelComAjuste/Download")]
        public IActionResult DownloadExcelComAjuste()
        {
            try
            {
                var result = _servicoRelatorioETO.Get(true).Result;

                using var workbook = new XLWorkbook();

                var worksheetETO = workbook.Worksheets.Add("ControleETO");

                worksheetETO.Cell(1, 1).Value = "Obra";
                worksheetETO.Cell(1, 2).Value = "Cliente";
                worksheetETO.Cell(1, 3).Value = "Descrição";
                worksheetETO.Cell(1, 4).Value = "Custo";
                worksheetETO.Cell(1, 5).Value = "Gasto";
                worksheetETO.Cell(1, 6).Value = "Saldo";

                var idsObras = result.Select(x => x.IdObra).ToList();
                var medicoesDeAjustes = _servicoRelatorioETO.ObtemAjustes(idsObras).Result;
                var datasDosAjustes = medicoesDeAjustes.Select(x => x.Data).Distinct().OrderBy(x => x).ToList();

                var count = 7;
                datasDosAjustes.ForEach(x =>
                {
                    worksheetETO.Cell(1, count).Value = $"{x.ToString("dd/MM/yyyy")}";
                    count++;
                });

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetETO.Cell(index + 1, 1).Value = result[index - 1].Codigo;
                    worksheetETO.Cell(index + 1, 2).Value = result[index - 1].Cliente;
                    worksheetETO.Cell(index + 1, 3).Value = result[index - 1].Descricao;
                    worksheetETO.Cell(index + 1, 4).Value = result[index - 1].Custo;
                    worksheetETO.Cell(index + 1, 5).Value = result[index - 1].Gasto;
                    worksheetETO.Cell(index + 1, 6).Value = result[index - 1].Saldo;

                    count = 7;

                    datasDosAjustes.ForEach(x =>
                    {
                        worksheetETO.Cell(index + 1, count).Value = medicoesDeAjustes.FirstOrDefault(y => y.IdObra == result[index - 1].IdObra && y.Data == x)?.Valor ?? 0;
                        count++;
                    });
                }

                count = 7;
                datasDosAjustes.ForEach(x =>
                {
                    worksheetETO.Cell(result.Count + 1, count).Value = medicoesDeAjustes.Where(y => y.Data == x).Sum(y => y.Valor);
                    //worksheetETO.Cell(result.Count + 1, count).Style.Fill.BackgroundColor = XLColor.Gray;
                    count++;
                });

                var linha = result.Count + 2;
                foreach (var item in datasDosAjustes.GroupBy(x => new { x.Date.Year, x.Date.Month }))
                {
                    worksheetETO.Cell(linha + 1, 6).Value = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.Key.Month)}/{item.Key.Year}";
                    worksheetETO.Cell(linha + 1, 7).Value = medicoesDeAjustes.Where(x=>x.Data.Month==item.Key.Month && x.Data.Year == item.Key.Year).Sum(x=>x.Valor);

                    linha++;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ControleETO.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
