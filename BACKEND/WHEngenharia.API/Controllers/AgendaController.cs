using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PdfSharpCore;
using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AgendaController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoAgenda _servicoAgenda;

        public AgendaController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoAgenda = new ServicoAgenda(_context, _mapper);
        }

        [HttpPost("AlteraDEF/{id}/{idDef}")]
        public async Task<ActionResult> AlteraDef(Int64 id, Int64 idDef)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            await _servicoAgenda.AlteraDef(id, idDef, IdUsuario, NomeUsuario);
            return Ok();
        }

        [HttpPost("AlteraValor")]
        public async Task<ActionResult> AlteraValor([FromBody] Agenda_AlteracaoValorDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.AlteraValor(parametros);
            return Ok();
        }

        [HttpPost("AlteraDataPagamento")]
        public async Task<ActionResult> AlteraDataPagamento([FromBody] Agenda_AlteracaoDataDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.AlteraDataPagamento(parametros);
            return Ok();
        }



        [HttpPost("AlteraDefLote")]
        public async Task<ActionResult> AlteraDefEmLote([FromBody] Agenda_AlteracaoDefLoteDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.AlteraDefEmLote(parametros);
            return Ok();
        }

        [HttpPost("AlteraDataPagamentoLote")]
        public async Task<ActionResult> AlteraDataPagamentoEmLote([FromBody] Agenda_AlteracaoDataLoteDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.AlteraDataPagamentoEmLote(parametros);
            return Ok();
        }

        [HttpPost("InformarPagamentoRecebimentoLote")]
        public async Task<ActionResult> InformarPagamentoRecebimentoLote([FromBody] Agenda_PagamentoRecebimentoLoteDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.InformarPagamentoRecebimentoLote(parametros);
            return Ok();
        }

        [HttpPost("CancelarPagamentoRecebimento")]
        public async Task<ActionResult> CancelarPagamentoRecebimento([FromBody] Agenda_CancelamentoPagamentoRecebimentoDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoAgenda.CancelarPagamentoRecebimento(parametros);
            return Ok();
        }



        [HttpPost("ObtemAgendaFaturamento")]
        public async Task<ActionResult> ObtemAgendaFaturamento(Agenda_RequestDTO parametros)
        {
            return Ok(await _servicoAgenda.ObtemAgendaFaturamento(parametros));
        }

        [HttpPost("ObtemAgenda")]
        public async Task<ActionResult> Get(Agenda_RequestDTO parametros)
        {
            return Ok(await _servicoAgenda.Get(parametros));
        }

        [HttpPost("PDF/Download")]
        public IActionResult DownloadPDF(Agenda_RequestDTO parametros)
        {
            try
            {
                var result = _servicoAgenda.Get(parametros).Result;

                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoAgenda.GeraPDFAgenda(logoPath, result).Result,
                    "application/pdf",
                    $"Agenda.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PDFFaturamento/Download")]
        public IActionResult DownloadPDFFaturamento(Agenda_RequestDTO parametros)
        {
            try
            {
                var result = _servicoAgenda.ObtemAgendaFaturamento(parametros).Result;

                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoAgenda.GeraPDFAgenda(logoPath, result).Result,
                    "application/pdf",
                    $"Agenda.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Excel/Faturamento/Download")]
        public IActionResult DownloadAgendaFaturamentoPDF(Agenda_ExcelFaturamento_RequestDTO parametros)
        {
            try
            {
                return File(
                    _servicoAgenda.ExcelFaturamento(parametros).Result,
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AgendaFaturamento.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("PDF/Faturamento/Download")]
        public IActionResult DownloadPDFAgendaFaturamentoPDF(Agenda_ExcelFaturamento_RequestDTO parametros)
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoAgenda.PdfFaturamento(parametros, logoPath).Result,
                    "application/pdf", $"AgendaFaturamento.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Excel/Download")]
        public IActionResult DownloadExcel(Agenda_RequestDTO parametros)
        {
            try
            {
                var result = _servicoAgenda.Get(parametros).Result;

                using var workbook = new XLWorkbook();

                var worksheetFaturado = workbook.Worksheets.Add("Agenda");

                worksheetFaturado.Cell(1, 1).Value = "Obra";
                worksheetFaturado.Cell(1, 2).Value = "DEF";
                worksheetFaturado.Cell(1, 3).Value = "Ordem Compra";
                worksheetFaturado.Cell(1, 4).Value = "Pedido Interno";
                worksheetFaturado.Cell(1, 5).Value = "Nº Nota Fiscal";
                worksheetFaturado.Cell(1, 6).Value = "Data Lançamento";
                worksheetFaturado.Cell(1, 7).Value = "Data Pagamento";
                worksheetFaturado.Cell(1, 8).Value = "Valor";
                worksheetFaturado.Cell(1, 9).Value = "Pagar ao Fornecedor";
                worksheetFaturado.Cell(1, 10).Value = "Pagar ao Colaborador";

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetFaturado.Cell(index + 1, 1).Value = result[index - 1].CodigoObra;
                    worksheetFaturado.Cell(index + 1, 2).Value = result[index - 1].CodigoDef;
                    worksheetFaturado.Cell(index + 1, 3).Value = result[index - 1].CodigoPedidoCompra;
                    worksheetFaturado.Cell(index + 1, 4).Value = result[index - 1].CodigoPedidoInterno;
                    worksheetFaturado.Cell(index + 1, 5).Value = result[index - 1].NumeroNF;
                    worksheetFaturado.Cell(index + 1, 6).Value = result[index - 1].DataLancamento.Year == 2500 ? "" : result[index - 1].DataLancamento.ToString("dd/MM/yyyy");
                    worksheetFaturado.Cell(index + 1, 7).Value = result[index - 1].DataPagamento.Year == 2500 ? "" : result[index - 1].DataPagamento.ToString("dd/MM/yyyy");
                    worksheetFaturado.Cell(index + 1, 8).Value = result[index - 1].Valor.ToString();
                    worksheetFaturado.Cell(index + 1, 9).Value = result[index - 1].NomeFantasiaFornecedor;
                    worksheetFaturado.Cell(index + 1, 10).Value = result[index - 1].NomeUsuarioBeneficiario;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Agenda.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ExcelFaturamento/Download")]
        public IActionResult DownloadExcelFaturamento(Agenda_RequestDTO parametros)
        {
            try
            {
                var result = _servicoAgenda.ObtemAgendaFaturamento(parametros).Result;

                using var workbook = new XLWorkbook();

                var worksheetFaturado = workbook.Worksheets.Add("Agenda");

                worksheetFaturado.Cell(1, 1).Value = "Obra";
                worksheetFaturado.Cell(1, 2).Value = "DEF";
                worksheetFaturado.Cell(1, 3).Value = "Ordem Compra";
                worksheetFaturado.Cell(1, 4).Value = "Pedido Interno";
                worksheetFaturado.Cell(1, 5).Value = "Nº Nota Fiscal";
                worksheetFaturado.Cell(1, 6).Value = "Data Lançamento";
                worksheetFaturado.Cell(1, 7).Value = "Data Pagamento";
                worksheetFaturado.Cell(1, 8).Value = "Valor";
                worksheetFaturado.Cell(1, 9).Value = "Pagar ao Fornecedor";
                worksheetFaturado.Cell(1, 10).Value = "Pagar ao Colaborador";

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetFaturado.Cell(index + 1, 1).Value = result[index - 1].CodigoObra;
                    worksheetFaturado.Cell(index + 1, 2).Value = result[index - 1].CodigoDef;
                    worksheetFaturado.Cell(index + 1, 3).Value = result[index - 1].CodigoPedidoCompra;
                    worksheetFaturado.Cell(index + 1, 4).Value = result[index - 1].CodigoPedidoInterno;
                    worksheetFaturado.Cell(index + 1, 5).Value = result[index - 1].NumeroNF;
                    worksheetFaturado.Cell(index + 1, 6).Value = result[index - 1].DataLancamento.Year == 2500 ? "" : result[index - 1].DataLancamento.ToString("dd/MM/yyyy");
                    worksheetFaturado.Cell(index + 1, 7).Value = result[index - 1].DataPagamento.Year == 2500 ? "" : result[index - 1].DataPagamento.ToString("dd/MM/yyyy");
                    worksheetFaturado.Cell(index + 1, 8).Value = result[index - 1].Valor.ToString();
                    worksheetFaturado.Cell(index + 1, 9).Value = result[index - 1].NomeFantasiaFornecedor;
                    worksheetFaturado.Cell(index + 1, 10).Value = result[index - 1].NomeUsuarioBeneficiario;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"AgendaFaturamento.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ExcelCarimbos/Download")]
        public IActionResult ExcelCarimbos(Agenda_ExcelCarimboRequestDTO parametros)
        {
            try
            {
                var result = _servicoAgenda.ObtemNotasComCarimbo(parametros);

                using var workbook = new XLWorkbook();

                var worksheet = workbook.Worksheets.Add("Agenda");

                worksheet.Cell(1, 1).Value = "Obra";
                worksheet.Cell(1, 2).Value = "Pedido Compra";
                worksheet.Cell(1, 3).Value = "Nota Fiscal";
                worksheet.Cell(1, 4).Value = "Fornecedor";
                worksheet.Cell(1, 5).Value = "CNPJ";
                worksheet.Cell(1, 6).Value = "Valor Bruto";
                worksheet.Cell(1, 7).Value = "Valor Material Abatido";
                worksheet.Cell(1, 8).Value = "Art 30";
                worksheet.Cell(1, 9).Value = "Data Pagamento Art 30";
                worksheet.Cell(1, 10).Value = "INSS";
                worksheet.Cell(1, 11).Value = "Data Pagamento INSS";
                worksheet.Cell(1, 12).Value = "ISS";
                worksheet.Cell(1, 13).Value = "Data Pagamento ISS";
                worksheet.Cell(1, 14).Value = "IR";
                worksheet.Cell(1, 15).Value = "Data Pagamento IR";
                worksheet.Cell(1, 16).Value = "Valor Líquido";

                for (int i = 1; i <= 16; i++)
                {
                    worksheet.Cell(1, i).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                    worksheet.Cell(1, i).Style.Font.SetBold(true);

                    worksheet.Cell(1, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    worksheet.Cell(1, i).Style.Border.OutsideBorderColor = XLColor.Black;
                }

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheet.Cell(index + 1, 1).Value = result[index - 1].Obra;
                    worksheet.Cell(index + 1, 2).Value = result[index - 1].PedidoCompra;
                    worksheet.Cell(index + 1, 3).Value = result[index - 1].NotaFiscal;
                    worksheet.Cell(index + 1, 4).Value = result[index - 1].fornecedor;
                    worksheet.Cell(index + 1, 5).Value = result[index - 1].cnpj;
                    worksheet.Cell(index + 1, 6).Value = $"R$ {result[index - 1].ValorBruto.ToString("N2")}";
                    worksheet.Cell(index + 1, 7).Value = $"R$ {result[index - 1].ValorMaterialAbatido.ToString("N2")}";
                    worksheet.Cell(index + 1, 8).Value = $"R$ {result[index - 1].ValorArt30.ToString("N2")}";
                    worksheet.Cell(index + 1, 9).Value = result[index - 1].DataPagamentoArt30.ToString("dd/MM/yyyy");
                    worksheet.Cell(index + 1, 10).Value = $"R$ {result[index - 1].ValorINSS.ToString("N2")}";
                    worksheet.Cell(index + 1, 11).Value = result[index - 1].DataPagamentoINSS.ToString("dd/MM/yyyy");
                    worksheet.Cell(index + 1, 12).Value = $"R$ {result[index - 1].ValorISS.ToString("N2")}";
                    worksheet.Cell(index + 1, 13).Value = result[index - 1].DataPagamentoISS.ToString("dd/MM/yyyy");
                    worksheet.Cell(index + 1, 14).Value = $"R$ {result[index - 1].ValorIR.ToString("N2")}";
                    worksheet.Cell(index + 1, 15).Value = result[index - 1].DataPagamentoIR.ToString("dd/MM/yyyy");
                    worksheet.Cell(index + 1, 16).Value = $"R$ {result[index - 1].ValorLiquido.ToString("N2")}";

                    for (int i = 1; i <= 16 ; i++)
                    {
                        worksheet.Cell(index + 1, i).Style.Fill.SetBackgroundColor(XLColor.LightGray);
                        worksheet.Cell(index + 1, i).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                        worksheet.Cell(index + 1, i).Style.Border.OutsideBorderColor = XLColor.Black;
                    }
                }

                worksheet.Cell(result.Count + 2, 6).Value = $"R$ {result.Sum(x=>x.ValorBruto).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 7).Value = $"R$ {result.Sum(x=>x.ValorMaterialAbatido).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 8).Value = $"R$ {result.Sum(x => x.ValorArt30).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 10).Value = $"R$ {result.Sum(x => x.ValorINSS).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 12).Value = $"R$ {result.Sum(x => x.ValorISS).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 14).Value = $"R$ {result.Sum(x => x.ValorIR).ToString("N2")}";
                worksheet.Cell(result.Count + 2, 16).Value = $"R$ {result.Sum(x => x.ValorLiquido).ToString("N2")}";

                worksheet.Cell(result.Count + 2, 6).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 6).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 7).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 7).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 8).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 8).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 10).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 10).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 12).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 12).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 14).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 14).Style.Font.SetBold(true);
                worksheet.Cell(result.Count + 2, 16).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
                worksheet.Cell(result.Count + 2, 16).Style.Font.SetBold(true);

                worksheet.Cell(result.Count + 2, 6).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 6).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 7).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 7).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 8).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 8).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 10).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 10).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 12).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 12).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 14).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 14).Style.Border.OutsideBorderColor = XLColor.Black;
                worksheet.Cell(result.Count + 2, 16).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                worksheet.Cell(result.Count + 2, 16).Style.Border.OutsideBorderColor = XLColor.Black;

                worksheet.Columns().AdjustToContents();

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"CarimboNotasFiscais.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("PDF/Carimbo/Download/{id}")]
        public IActionResult DownloadNotasFiscaisCarimbo([FromRoute] Int64 id)
        {
            try
            {
                var result = _servicoAgenda.ObtemNotaFiscalComCarimbo(id, $"{_configuration["DiretorioUpload"]}\\").Result;

                Response.Headers.Add($"Content-Disposition", $"{result.Item2}");
                Response.Headers.Add("Access-Control-Expose-Headers", "Content-Disposition");

                return File(result.Item1.ToArray(), "application/pdf");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao processar PDF: {ex.Message}");
            }
        }


        [HttpPost("Carimbo/Download/Zip")]
        public async Task<IActionResult> DownloadTodasNotasFiscais([FromBody] Agenda_ExcelCarimboRequestDTO parametros)
        {
            try
            {
                var ids = await _servicoAgenda.ObtemIdsPedidosComNotasComCarimbos(parametros);

                using var memoryStream = new MemoryStream();
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    foreach (var id in ids)
                    {
                        var resultado = await _servicoAgenda.ObtemNotaFiscalComCarimbo(id, $"{_configuration["DiretorioUpload"]}\\");

                        var nomeArquivo = resultado.Item2;

                        byte[] conteudoPdf = resultado.Item1.ToArray();
                        var entry = archive.CreateEntry(nomeArquivo, CompressionLevel.Fastest);
                        using var entryStream = entry.Open();
                        await entryStream.WriteAsync(conteudoPdf, 0, conteudoPdf.Length);
                    }
                }

                memoryStream.Position = 0;
                return File(memoryStream.ToArray(), "application/zip", "notas_fiscais.zip");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar o ZIP: {ex.Message}");
            }
        }

        [HttpPost("AjusteManual/{idFluxoCaixa}")]
        public async Task<IActionResult> AjusteManual([FromRoute] Int64 idFluxoCaixa, [FromBody] List<Agenda_AjusteManualRequestDTO> parametros)
        {
            try
            {
                await _servicoAgenda.AjusteManual(idFluxoCaixa, parametros);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
