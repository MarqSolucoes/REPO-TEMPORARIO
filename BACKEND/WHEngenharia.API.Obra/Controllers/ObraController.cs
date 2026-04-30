using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Agenda;
using WHEngenharia.Dominio.Modelos.Genericos.Obra;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Obra.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ObraController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoObra _servicoObra;

        public ObraController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<ObraDTO>>> Get(bool apenasAtivos = false)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            return Ok(await _servicoObra.Get(IdUsuario, apenasAtivos));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ObraDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoObra.Get(Id));
        }

        [HttpPost]
        public async Task<ActionResult<ObraDTO>> Post([FromBody] ObraDTO obraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                obraDTO.IdUsuarioAlteracao = IdUsuario;
                obraDTO.IdUsuarioCadastro = IdUsuario;
                obraDTO.DataCadastro = DateTime.Now;
                obraDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoObra.Post(obraDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<ObraDTO>> Put([FromBody] ObraDTO obraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                obraDTO.IdUsuarioAlteracao = IdUsuario;
                obraDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoObra.Put(obraDTO);

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

                var obraDTO = await _servicoObra.Get(id);
                obraDTO.DataUltimaAlteracao = DateTime.Now;
                obraDTO.IdUsuarioAlteracao = IdUsuario;
                obraDTO.Finalizada = ativarDesativar;

                var result = await _servicoObra.Put(obraDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("BloquearDesbloquear/{id}/{bloquearDesbloquear}")]
        public async Task<ActionResult> BloquearDesbloquear([FromRoute] Int64 id, [FromRoute] bool bloquearDesbloquear)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoObra.BloquearDesbloquear(id, IdUsuario, bloquearDesbloquear);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("HabilitarAprovacaoAutomatica/{id}")]
        public async Task<ActionResult> HabilitarAprovacaoAutomatica([FromRoute]Int64 id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var obraDTO = await _servicoObra.Get(id);
                obraDTO.DataUltimaAlteracao = DateTime.Now;
                obraDTO.IdUsuarioAlteracao = IdUsuario;
                obraDTO.AprovacaoAutomatica = !obraDTO.AprovacaoAutomatica;

                var result = await _servicoObra.Put(obraDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("ContaCorrenteExcel/Download/{idObra}")]
        public IActionResult ContaCorrenteExcel([FromRoute]Int64 idObra)
        {
            try
            {
                var obra = _servicoObra.Get(idObra).Result;

                var result = _servicoObra.ObtemDadosContaCorrente(idObra).Result;

                using var workbook = new XLWorkbook();

                var worksheetETO = workbook.Worksheets.Add("ContaCorrente");

                worksheetETO.Cell(1, 1).Value = "CONTA CORRENTE";

                worksheetETO.Cell(3, 1).Value = $"Obra: {obra.Codigo} {obra.Cliente.NomeFantasia}";

                worksheetETO.Cell(5, 1).Value = $"ETO Prev: {obra.ValorTotal}";

                worksheetETO.Cell(7, 1).Value = $"PAGAMENTOS";
                worksheetETO.Cell(8, 1).Value = "O.C.";
                worksheetETO.Cell(8, 2).Value = "N. FISCAL";
                worksheetETO.Cell(8, 3).Value = "DATA";
                worksheetETO.Cell(8, 4).Value = "VALOR";
                worksheetETO.Cell(8, 5).Value = "FORNECEDOR/CLIENTE";

                worksheetETO.Cell(7, 6).Value = $"RECEBIMENTOS";
                worksheetETO.Cell(8, 6).Value = "VALOR PREVISTO";
                worksheetETO.Cell(8, 7).Value = "VALOR RECEBIDO";
                worksheetETO.Cell(8, 8).Value = "FATURAMENTO";
                worksheetETO.Cell(8, 9).Value = "RECEBIMENTO";
                
                worksheetETO.Cell(7, 10).Value = "SALDO";

                var saldo = 0.0;

                for (var index = 1; index <= result.Count; index++)
                {
                    if (!result[index - 1].Credito)
                    {
                        worksheetETO.Cell(index + 8, 1).Value = result[index - 1].OrdemCompra;
                        worksheetETO.Cell(index + 8, 2).Value = result[index - 1].NotaFiscal;
                        worksheetETO.Cell(index + 8, 3).Value = result[index - 1].Data?.ToString("dd/MM/yyyy");
                        worksheetETO.Cell(index + 8, 4).Value = result[index - 1].Valor;
                        worksheetETO.Cell(index + 8, 5).Value = result[index - 1].Fornecedor;

                        saldo = saldo - result[index - 1].Valor;
                    }
                    else
                    {
                        worksheetETO.Cell(index + 8, 6).Value = result[index - 1].ValorPrevisto;
                        worksheetETO.Cell(index + 8, 7).Value = result[index - 1].Valor;
                        worksheetETO.Cell(index + 8, 8).Value = result[index - 1].DataRecebimentoPrevisto.ToString("dd/MM/yyyy");
                        worksheetETO.Cell(index + 8, 9).Value = result[index - 1].Data?.ToString("dd/MM/yyyy");

                        saldo = saldo + result[index - 1].Valor;
                    }

                    worksheetETO.Cell(index + 8, 10).Value = saldo;

                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ContaCorrente_{obra.Codigo}.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ContaCorrentePDF/Download/{idObra}")]
        public IActionResult ContaCorrentePDF([FromRoute] Int64 idObra)
        {
            try
            {
                var obra = _servicoObra.Get(idObra).Result;

                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoObra.GeraPDFContaCorrente(logoPath, idObra).Result,
                    "application/pdf",
                    $"ContaCorrente_{obra.Codigo}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("ResumoETOExcel/Download/")]
        public IActionResult ResumoETOExcel()
        {
            try
            {
                var result = _servicoObra.ObtemDadosResumoETO().Result;

                using var workbook = new XLWorkbook();

                var worksheetETO = workbook.Worksheets.Add("ResumoETO");

                worksheetETO.Cell(1, 1).Value = $"OBRA";
                worksheetETO.Cell(1, 2).Value = "NOME";
                worksheetETO.Cell(1, 3).Value = "ESCOPO";
                worksheetETO.Cell(1, 4).Value = "ETO";
                worksheetETO.Cell(1, 5).Value = "GASTO";
                worksheetETO.Cell(1, 6).Value = "SALDO";

                for (var index = 1; index <= result.Count; index++)
                {
                    worksheetETO.Cell(index + 1, 1).Value = result[index - 1].CodigoObra;
                    worksheetETO.Cell(index + 1, 2).Value = result[index - 1].Cliente;
                    worksheetETO.Cell(index + 1, 3).Value = result[index - 1].Escopo;
                    worksheetETO.Cell(index + 1, 4).Value = result[index - 1].ETO;
                    worksheetETO.Cell(index + 1, 5).Value = result[index - 1].Gasto;
                    worksheetETO.Cell(index + 1, 6).Value = result[index - 1].Saldo;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"ResumoETO.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ResumoETOPdf/Download")]
        public IActionResult ResumoETOPdf()
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoObra.GeraPDFResumoETO(logoPath).Result,
                    "application/pdf",
                    "ResumoETO.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("EtoObraPdf/Download/{idObra}")]
        public IActionResult EtoObraPdf([FromRoute] Int64 idObra)
        {
            try
            {
                var obra = _servicoObra.Get(idObra).Result;

                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoObra.GeraPDFETOObra(logoPath, idObra).Result,
                    "application/pdf",
                    $"ETO_{obra.Codigo}.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ExcelAjusteETO/Download/")]
        public IActionResult ExcelAjusteETO()
        {
            try
            {
                var result = _servicoObra.MontaExcelAjusteETO(false);

                return File(
                    result.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    "AjusteETO.xlsx"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Upload/AjusteETO")]
        public ActionResult Upload([FromForm] IFormFile arquivo)
        {
            try
            {
                //var streamArquivoOriginal = _servicoObra.MontaExcelAjusteETO();

                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var extensao = Path.GetExtension(arquivo.FileName).ToUpper();
                var extensoesPermitidas = new List<string>() { ".XLS", ".XLSX" };

                if (!extensoesPermitidas.Contains(extensao))
                    throw new Exception("Extensão do arquivo não permitida");

                using var streamArquivoUpload = new MemoryStream();
                arquivo.CopyTo(streamArquivoUpload);
                streamArquivoUpload.Position = 0;

                using var workbook = new XLWorkbook(streamArquivoUpload);
                var worksheet = workbook.Worksheet(1);

                var objetos = new List<Obra_AjusteETODTO>();

                var headerRow = worksheet.Row(1);
                var colunas = headerRow.CellsUsed().ToList();
                int colCount = colunas.Count;

                // Mapeia as colunas com datas válidas
                var colunasData = new Dictionary<int, DateTime>();
                for (int col = 1; col <= colCount; col++)
                {
                    var texto = worksheet.Cell(1, col).GetString().Trim();
                    if (DateTime.TryParseExact(texto, "dd/MM/yyyy 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data))
                    {
                        colunasData[col] = data;
                    }
                    else if (DateTime.TryParseExact(texto, "yyyy/MM/dd 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data2))
                    {
                        colunasData[col] = data2;
                    }
                    else if (DateTime.TryParseExact(texto, "MM/dd/yyyy 00:00:00", CultureInfo.InvariantCulture, DateTimeStyles.None, out var data3))
                    {
                        colunasData[col] = data3;
                    }
                    else if (DateTime.TryParse(texto, CultureInfo.InvariantCulture, DateTimeStyles.None, out var data4))
                    {
                        colunasData[col] = data4;
                    }
                }

                // Itera sobre as linhas de dados
                int rowCount = worksheet.LastRowUsed().RowNumber();
                for (int row = 2; row <= rowCount; row++)
                {
                    var codigoObra = worksheet.Cell(row, 1).GetString().Trim();
                    if (string.IsNullOrWhiteSpace(codigoObra))
                        continue;

                    var objeto = new Obra_AjusteETODTO { CodigoObra = codigoObra };

                    foreach (var kvp in colunasData)
                    {
                        var col = kvp.Key;
                        var data = kvp.Value;

                        var cell = worksheet.Cell(row, col);
                        if (cell.TryGetValue<double>(out var valor) && valor != 0)
                        {
                            objeto.Valores.Add(new Tuple<DateTime, double>(data, valor));
                        }
                    }

                    objetos.Add(objeto);
                }

                objetos.ForEach(o =>
                {
                    _servicoObra.AtualizaPrevisoesETO(o, IdUsuario);
                });

                var novoAjuste = new ObraAjusteDTO();
                novoAjuste.Id = 0;
                novoAjuste.DataCadastro = DateTime.Now;
                novoAjuste.IdUsuarioCadastro = IdUsuario;
                novoAjuste.NomeLogicoAntes = $"Ajuste Situação Anterior - {DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}.xlsx";
                novoAjuste.NomeLogicoDepois = $"Ajuste Situação Nova - {DateTime.Now.ToString("dd-MM-yyyy HH-mm-ss")}{extensao.ToLower()}";

                _servicoObra.SalvaAjuste(novoAjuste);

                //System.IO.File.WriteAllBytes($"{_configuration["DiretorioUpload"]}\\{novoAjuste.NomeLogicoAntes}", streamArquivoOriginal.ToArray());
                System.IO.File.WriteAllBytes($"{_configuration["DiretorioUpload"]}\\{novoAjuste.NomeLogicoDepois}", streamArquivoUpload.ToArray());

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("AjusteETO/Download/{id}/{tipo}")]
        public IActionResult AjusteETO([FromRoute] Int64 id, [FromRoute] int tipo)
        {
            try
            {
                var nomeArquivo = _servicoObra.GetFile(id, tipo).Result;

                if (string.IsNullOrEmpty(nomeArquivo))
                    throw new Exception("Arquivo não encontrado");

                MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("text/plain");

                if (nomeArquivo.ToLower().EndsWith(".pdf"))
                    mediaType = new MediaTypeHeaderValue("application/pdf");
                else if (nomeArquivo.ToLower().EndsWith(".txt"))
                    mediaType = new MediaTypeHeaderValue("text/plain");
                else if (nomeArquivo.ToLower().EndsWith(".png"))
                    mediaType = new MediaTypeHeaderValue("image/png");
                else if (nomeArquivo.ToLower().EndsWith(".jpg"))
                    mediaType = new MediaTypeHeaderValue("image/jpg");
                else if (nomeArquivo.ToLower().EndsWith(".jpeg"))
                    mediaType = new MediaTypeHeaderValue("image/jpeg");
                else if (nomeArquivo.ToLower().EndsWith(".ppt"))
                    mediaType = new MediaTypeHeaderValue("application/vnd.ms-powerpoint");
                else if (nomeArquivo.ToLower().EndsWith(".pptx"))
                    mediaType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.presentationml.presentation");
                else if (nomeArquivo.ToLower().EndsWith(".doc"))
                    mediaType = new MediaTypeHeaderValue("application/msword");
                else if (nomeArquivo.ToLower().EndsWith(".docx"))
                    mediaType = new MediaTypeHeaderValue("applicationapplication/vnd.openxmlformats-officedocument.wordprocessingml.document");
                else if (nomeArquivo.ToLower().EndsWith(".xls"))
                    mediaType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                else if (nomeArquivo.ToLower().EndsWith(".xlsx"))
                    mediaType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

                return File(
                System.IO.File.ReadAllBytes($"{_configuration["DiretorioUpload"]}\\{nomeArquivo}"), mediaType.MediaType.ToString(), $"{nomeArquivo}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("ObtemAjustes")]
        public async Task<ActionResult> ObtemAjustes()
        {
            return Ok(await _servicoObra.ObtemAjustes());
        }

        [HttpPost("Cancelar/{idObra}")]
        public async Task<ActionResult> Cancelar([FromRoute]Int64 idObra)
        {
            try
            {
                await _servicoObra.Cancelar(idObra);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
