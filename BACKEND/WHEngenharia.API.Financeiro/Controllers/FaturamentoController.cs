using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
using WHEngenharia.Dominio.Modelos.Genericos.Faturamento;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Financeiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FaturamentoController: ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoFaturamento _servicoFaturamento;

        public FaturamentoController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoFaturamento = new ServicoFaturamento(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<FaturamentoDTO>>> Get()
        {
            return Ok(await _servicoFaturamento.Get());
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<FaturamentoDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoFaturamento.Get(Id));
        }

        [HttpGet("GetObra/{idObra}")]
        public async Task<ActionResult<List<FaturamentoDTO>>> GetObra([FromRoute] Int64 idObra)
        {
            return Ok(await _servicoFaturamento.GetObra(idObra));
        }

        [HttpGet("GetClienteDetalhe/{idCliente}")]
        public async Task<ActionResult<Faturamento_DetalheClienteDTO>> GetClienteDetalhe([FromRoute] Int64 idCliente)
        {
            return Ok(await _servicoFaturamento.GetClienteDetalhe(idCliente));
        }

        [HttpPost("GetFiltrado")]
        public async Task<ActionResult<Faturamento_FiltradoReturnDTO>> GetFiltrado(FaturamentoRequestDTO parametros)
        {
            return Ok(await _servicoFaturamento.GetFiltrado(parametros));
        }

        [HttpPost("CancelarFaturamento")]
        public async Task<ActionResult> CancelarFaturamento([FromBody] Faturamento_CancelarFaturamentoDTO parametros)
        {
            parametros.idUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            await _servicoFaturamento.CancelarFaturamento(parametros);

            return Ok();
        }

        [HttpPost("ObtemValoresAFaturar")]
        public async Task<ActionResult> ObtemValoresAFaturar(FaturamentoRequestDTO parametros)
        {
            return Ok(await _servicoFaturamento.ObtemValoresAFaturar(parametros));
        }

        [HttpGet("ObtemValoresAFaturar/{idObra}")]
        public async Task<ActionResult> ObtemValoresAFaturar(Int64 idObra)
        {
            return Ok(await _servicoFaturamento.ObtemValoresAFaturar(idObra));
        }

        [HttpPost("AjustaFaturamento")]
        public async Task<ActionResult> AjustaFaturamento([FromBody] Faturamento_AjusteFaturamentoDTO faturamentos)
        {
            await _servicoFaturamento.AjustaFaturamento(faturamentos);

            return Ok();
        }


        [HttpPost]
        public async Task<ActionResult<FaturamentoDTO>> Post([FromBody] FaturamentoDTO faturamentoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                faturamentoDTO.IdUsuarioAlteracao = IdUsuario;
                faturamentoDTO.IdUsuarioCadastro = IdUsuario;
                faturamentoDTO.DataCadastro = DateTime.Now;
                faturamentoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoFaturamento.Post(faturamentoDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<FaturamentoDTO>> Put([FromBody] FaturamentoDTO faturamentoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                faturamentoDTO.IdUsuarioAlteracao = IdUsuario;
                faturamentoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoFaturamento.Put(faturamentoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Upload/{idFaturamento}")]
        public async Task<ActionResult> Upload([FromForm] IFormFile arquivo, [FromRoute] int idFaturamento)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var extensao = Path.GetExtension(arquivo.FileName).ToUpper();

                var extensoesPermitidas = _configuration["ExtensoesPermitidasUpload"].Split(',').ToList();

                if (extensoesPermitidas.Contains(extensao))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var faturamentoArquivosDTO = new FaturamentoArquivosDTO();
                        faturamentoArquivosDTO.Id = 0;
                        faturamentoArquivosDTO.DataCadastro = DateTime.Now;
                        faturamentoArquivosDTO.Extensao = extensao;
                        faturamentoArquivosDTO.IdFaturamento = idFaturamento;
                        faturamentoArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        faturamentoArquivosDTO.Nome = arquivo.FileName;
                        faturamentoArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        faturamentoArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        await _servicoFaturamento.Upload(faturamentoArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{faturamentoArquivosDTO.NomeLogico}", memoryStream.ToArray());

                        return Ok();
                    }

                }
                else
                    throw new Exception("Extensão do arquivo não permitida");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("Arquivos/{idArquivo}")]
        public async Task<ActionResult> DeleteArquivo(int idArquivo)
        {
            try
            {
                var arquivoDTO = await _servicoFaturamento.GetFile(idArquivo);

                if (arquivoDTO == null)
                    throw new Exception("Arquivo não encontrado");

                System.IO.File.Delete($"{_configuration["DiretorioUpload"]}\\{arquivoDTO.NomeLogico}");

                await _servicoFaturamento.DeleteArquivo(idArquivo);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Arquivos/{idFaturamento}")]
        public async Task<List<FaturamentoArquivosDTO>> ObtemArquivos(Int64 idFaturamento)
        {
            return await _servicoFaturamento.ObtemArquivos(idFaturamento);
        }

        [HttpGet("Download/{id}")]
        public IActionResult Download(Int64 id)
        {
            try
            {
                var arquivoDTO = _servicoFaturamento.GetFile(id).Result;

                if (arquivoDTO == null)
                    throw new Exception("Arquivo não encontrado");

                MediaTypeHeaderValue mediaType = new MediaTypeHeaderValue("text/plain");

                if (arquivoDTO.Extensao.ToLower() == ".pdf")
                    mediaType = new MediaTypeHeaderValue("application/pdf");
                else if (arquivoDTO.Extensao.ToLower() == ".txt")
                    mediaType = new MediaTypeHeaderValue("text/plain");
                else if (arquivoDTO.Extensao.ToLower() == ".png")
                    mediaType = new MediaTypeHeaderValue("image/png");
                else if (arquivoDTO.Extensao.ToLower() == ".jpg")
                    mediaType = new MediaTypeHeaderValue("image/jpg");
                else if (arquivoDTO.Extensao.ToLower() == ".jpeg")
                    mediaType = new MediaTypeHeaderValue("image/jpeg");
                else if (arquivoDTO.Extensao.ToLower() == ".ppt")
                    mediaType = new MediaTypeHeaderValue("application/vnd.ms-powerpoint");
                else if (arquivoDTO.Extensao.ToLower() == ".pptx")
                    mediaType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.presentationml.presentation");
                else if (arquivoDTO.Extensao.ToLower() == ".doc")
                    mediaType = new MediaTypeHeaderValue("application/msword");
                else if (arquivoDTO.Extensao.ToLower() == ".docx")
                    mediaType = new MediaTypeHeaderValue("applicationapplication/vnd.openxmlformats-officedocument.wordprocessingml.document");
                else if (arquivoDTO.Extensao.ToLower() == ".xls")
                    mediaType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                else if (arquivoDTO.Extensao.ToLower() == ".xlsx")
                    mediaType = new MediaTypeHeaderValue("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");



                return File(
                System.IO.File.ReadAllBytes($"{_configuration["DiretorioUpload"]}\\{arquivoDTO.NomeLogico}"),
                mediaType.MediaType.ToString(),
                $"{arquivoDTO.Nome}{arquivoDTO.Extensao.ToLower()}"
            );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Excel/Download")]
        public IActionResult DownloadExcel(FaturamentoRequestDTO parametros)
        {
            try
            {
                var result = _servicoFaturamento.GetFiltrado(parametros).Result;

                using var workbook = new XLWorkbook();

                var worksheetFaturado = workbook.Worksheets.Add("Faturado");

                worksheetFaturado.Cell(1, 1).Value = "Obra";
                worksheetFaturado.Cell(1, 2).Value = "Cliente";
                worksheetFaturado.Cell(1, 3).Value = "Nº Nota Fiscal";
                worksheetFaturado.Cell(1, 4).Value = "Bruto Faturado";
                worksheetFaturado.Cell(1, 5).Value = "INSS";
                worksheetFaturado.Cell(1, 6).Value = "ISS";
                worksheetFaturado.Cell(1, 7).Value = "IR";
                worksheetFaturado.Cell(1, 8).Value = "Art. 30";
                worksheetFaturado.Cell(1, 9).Value = "Valor Líquido";
                worksheetFaturado.Cell(1, 10).Value = "Bruto a Faturar";
                worksheetFaturado.Cell(1, 11).Value = "Desconto";
                worksheetFaturado.Cell(1, 12).Value = "Data Faturamento";
                worksheetFaturado.Cell(1, 13).Value = "Data Prevista Recebimento";
                worksheetFaturado.Cell(1, 14).Value = "Data Recebimento";
                worksheetFaturado.Cell(1, 15).Value = "Observação";
                worksheetFaturado.Cell(1, 16).Value = "Status";

                var index = 1;
                for (index = 1; index <= result.FaturamentosDTO.Count; index++)
                {

                    worksheetFaturado.Cell(index + 1, 1).Value = result.FaturamentosDTO[index - 1].Obra.Codigo;
                    worksheetFaturado.Cell(index + 1, 2).Value = result.FaturamentosDTO[index - 1].Obra.Cliente.NomeFantasia;
                    worksheetFaturado.Cell(index + 1, 3).Value = result.FaturamentosDTO[index - 1].NumeroNF;
                    worksheetFaturado.Cell(index + 1, 4).Value = result.FaturamentosDTO[index - 1].ValorBruto;
                    worksheetFaturado.Cell(index + 1, 5).Value = result.FaturamentosDTO[index - 1].ValorINSS;
                    worksheetFaturado.Cell(index + 1, 6).Value = result.FaturamentosDTO[index - 1].ValorISS;
                    worksheetFaturado.Cell(index + 1, 7).Value = result.FaturamentosDTO[index - 1].ValorIR;
                    worksheetFaturado.Cell(index + 1, 8).Value = result.FaturamentosDTO[index - 1].ValorArt30;
                    worksheetFaturado.Cell(index + 1, 9).Value = result.FaturamentosDTO[index - 1].ValorLiquido;
                    worksheetFaturado.Cell(index + 1, 11).Value = result.FaturamentosDTO[index - 1].ValorDesconto;
                    worksheetFaturado.Cell(index + 1, 12).Value = result.FaturamentosDTO[index - 1].DataFaturamento.ToString("dd/MM/yyyy");
                    worksheetFaturado.Cell(index + 1, 13).Value = result.FaturamentosDTO[index - 1].DataRecebimentoPrevisto.ToString("dd/MM/yyyy") ?? "";
                    worksheetFaturado.Cell(index + 1, 14).Value = result.FaturamentosDTO[index - 1].DataRecebimentoRealizado?.ToString("dd/MM/yyyy") ?? "";
                    worksheetFaturado.Cell(index + 1, 15).Value = result.FaturamentosDTO[index - 1].Observacao;
                    worksheetFaturado.Cell(index + 1, 16).Value = result.FaturamentosDTO[index - 1].Status.Descricao;
                }

                result.MedicoesDTO = result.MedicoesDTO.OrderBy(x => x.Data).Where(x => x.ValorFaturado < x.Valor).ToList();

                for (var index2 = 1; index2 <= result.MedicoesDTO.Count; index2++)
                {
                    var valorBruto = (result.MedicoesDTO[index2 - 1].Valor - result.MedicoesDTO[index2 - 1].ValorFaturado);

                    var porcentagemDaMedicaoEmRelacaoAoBruto = (valorBruto * 100) / result.MedicoesDTO[index2 - 1].ObraDaMedicao.ValorTotal;
                    var valorMaterialPorPorcentagem = (result.MedicoesDTO[index2 - 1].ObraDaMedicao.ValorMaterial / 100) * porcentagemDaMedicaoEmRelacaoAoBruto;

                    double valorImpostoISS = valorBruto / 100 * result.MedicoesDTO[index2 - 1].ObraDaMedicao.AliquotaImpostoISS;
                    double valorImpostoINSS = ((valorBruto - valorMaterialPorPorcentagem) / 100 * (result.MedicoesDTO[index2 - 1].ObraDaMedicao.AliquotaImpostoINSS ?? 0)) - (valorBruto / 100 * (result.MedicoesDTO[index2 - 1].ObraDaMedicao.AliquotaImpostoIR ?? 0));
                    double valorImpostoIR = valorBruto / 100 * result.MedicoesDTO[index2 - 1].ObraDaMedicao.AliquotaImpostoIR ?? 0;
                    double valorImpostoArt30 = valorBruto / 100 * result.MedicoesDTO[index2 - 1].ObraDaMedicao.AliquotaImpostoArt30 ?? 0;

                    var valorLiquidoMenosImpostos = valorBruto - valorImpostoISS - valorImpostoINSS - valorImpostoIR - valorImpostoArt30;

                    worksheetFaturado.Cell(index + 1, 1).Value = result.MedicoesDTO[index2 - 1].ObraDaMedicao.Codigo;
                    worksheetFaturado.Cell(index + 1, 2).Value = result.MedicoesDTO[index2 - 1].ObraDaMedicao.Cliente.NomeFantasia;
                    worksheetFaturado.Cell(index + 1, 3).Value = "";
                    worksheetFaturado.Cell(index + 1, 4).Value = "";
                    worksheetFaturado.Cell(index + 1, 5).Value = valorImpostoINSS;
                    worksheetFaturado.Cell(index + 1, 6).Value = valorImpostoISS;
                    worksheetFaturado.Cell(index + 1, 7).Value = valorImpostoIR;
                    worksheetFaturado.Cell(index + 1, 8).Value = valorImpostoArt30;
                    worksheetFaturado.Cell(index + 1, 9).Value = valorLiquidoMenosImpostos;
                    worksheetFaturado.Cell(index + 1, 10).Value = valorBruto;
                    worksheetFaturado.Cell(index + 1, 11).Value = "";
                    worksheetFaturado.Cell(index + 1, 12).Value = result.MedicoesDTO[index2 - 1].Data.ToString("dd/MM/yyyy") ?? "";
                    worksheetFaturado.Cell(index + 1, 13).Value = result.MedicoesDTO[index2 - 1].Data.ToString("dd/MM/yyyy") ?? "";
                    worksheetFaturado.Cell(index + 1, 14).Value = "";
                    worksheetFaturado.Cell(index + 1, 15).Value = "A FATURAR";
                    worksheetFaturado.Cell(index + 1, 16).Value = "";

                    index++;
                }

                using var stream = new MemoryStream();

                workbook.SaveAs(stream);

                var content = stream.ToArray();

                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Faturamento.xlsx");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AlteraData")]
        public async Task<ActionResult> AlteraData([FromBody] Faturamento_AlteracaoDataDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoFaturamento.AlteraData(parametros);
            return Ok();
        }

        [HttpPost("AlteraValor")]
        public async Task<ActionResult> AlteraValor([FromBody] Faturamento_AlteracaoValorDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
            var NomeUsuario = (User.Identity as ClaimsIdentity).FindFirst("Nome").Value;

            parametros.IdUsuario = IdUsuario;
            parametros.NomeUsuario = NomeUsuario;

            await _servicoFaturamento.AlteraValor(parametros);
            return Ok();
        }

        [HttpPost("InformarFaturamento")]
        public async Task<ActionResult> InformarFaturamento(Faturamento_InformarFaturamentoDTO faturamento)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoFaturamento.InformarFaturamento(faturamento, IdUsuario);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
