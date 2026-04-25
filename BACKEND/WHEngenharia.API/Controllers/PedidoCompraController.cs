using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Conciliacao;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoCompra;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoCompraController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoPedidoCompra _servicoPedidoCompra;
        private ServicoPedidoCompraNotaFiscal _servicoPedidoCompraNotaFiscal;
        private ServicoObra _servicoObra;

        public PedidoCompraController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoPedidoCompra = new ServicoPedidoCompra(_context, _mapper);
            _servicoPedidoCompraNotaFiscal = new ServicoPedidoCompraNotaFiscal(_context, _mapper);
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> Get([FromRoute] Int64 id)
        {
            try
            {
                var result = await _servicoPedidoCompra.Get(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Status/{idStatus}")]
        public async Task<ActionResult<List<PedidoCompraDTO>>> GetByStatus(Int64 idStatus)
        {
            return Ok(await _servicoPedidoCompra.GetByStatus(idStatus));
        }

        [HttpPut]
        public async Task<ActionResult<PedidoCompraDTO>> Put([FromBody] PedidoCompraDTO pedidoCompraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                pedidoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoCompra.Put(pedidoCompraDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ObtemFinalizadosFiltrados")]
        public async Task<ActionResult> ObtemFinalizadosFiltrados(PedidoCompra_FiltroPedidosFinalizadosDTO parametrosDTO)
        {
            try
            {
                return Ok(await _servicoPedidoCompra.ObtemFinalizadosFiltrados(parametrosDTO));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ParaConciliacao")]
        public async Task<ActionResult> GetParaConciliacao()
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                return Ok(await _servicoPedidoCompra.GetParaConciliacaoPorUsuario(IdUsuario));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Conciliacao/ReprovadasDirecao")]
        public async Task<ActionResult> ConciliacaoReprovadasDirecao()
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                return Ok(await _servicoPedidoCompra.ConciliacaoReprovadasDirecao());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotaFiscal/MarcarVisualizacaoNotaFiscalReprovada/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> MarcarVisualizacaoNotaFiscalReprovada(Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                await _servicoPedidoCompra.MarcarVisualizacaoNotaFiscalReprovada(idPedidoCompraNotaFiscal);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Conciliacao/NotaFiscal/ReenviarDiretoria/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> ReenviarDiretoria(Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.ReenviarNotaFiscalParaDiretoria(idPedidoCompraNotaFiscal);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Conciliacao/ReprovadasFinanceiro")]
        public async Task<ActionResult> ConciliacaoReprovadasFinanceiro()
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                return Ok(await _servicoPedidoCompra.ConciliacaoReprovadasFinanceiro());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Conciliacao/NotaFiscal/ReenviarFinanceiro/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> ReenviarFinanceiro(Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.ReenviarNotaFiscalParaFinanceiro(idPedidoCompraNotaFiscal);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ParaConciliacao/{Id}")]
        public async Task<ActionResult<Conciliacao_PedidoCompraDTO>> GetParaConciliacao([FromRoute] Int64 Id)
        {
            try
            {
                return Ok(await _servicoPedidoCompra.GetParaConciliacao(Id));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("DevolverParaCotacao/{Id}")]
        public async Task<ActionResult> DevolverParaCotacao([FromRoute] Int64 Id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.DevolverParaCotacao(Id, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ObtemNotasConciliadas")]
        public async Task<ActionResult<List<PedidoCompraNotaFiscalDTO>>> ObtemNotasConciliadas([FromBody] Conciliacao_NotasConciliadasParametrosDTO parametrosDTO)
        {
            try
            {
                var result = await _servicoPedidoCompra.ObtemNotasConciliadas(parametrosDTO);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Conciliar/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> ConciliarNotaFiscal([FromBody] Conciliacao_PedidoCompraDTO conciliacao_PedidoCompraDTO, [FromRoute] Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.ConciliarNotaFiscal(conciliacao_PedidoCompraDTO, idPedidoCompraNotaFiscal, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                await _servicoPedidoCompra.ExcluiNotaFiscal(idPedidoCompraNotaFiscal);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Conciliar/Upload/{idPedidoCompra}")]
        public async Task<ActionResult<PedidoCompraNotaFiscalDTO>> UploadConciliacaoNF([FromForm] IFormFile arquivo, [FromRoute] int idPedidoCompra)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var extensao = Path.GetExtension(arquivo.FileName).ToUpper();

                var extensoesPermitidas = new List<string>() { ".PDF" };

                if (extensoesPermitidas.Contains(extensao))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        var pedidoCompraArquivosDTO = new PedidoCompraArquivosDTO();
                        pedidoCompraArquivosDTO.Id = 0;
                        pedidoCompraArquivosDTO.DataCadastro = DateTime.Now;
                        pedidoCompraArquivosDTO.Extensao = extensao;
                        pedidoCompraArquivosDTO.IdPedidoCompra = idPedidoCompra;
                        pedidoCompraArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        pedidoCompraArquivosDTO.Nome = arquivo.FileName;
                        pedidoCompraArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        pedidoCompraArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        pedidoCompraArquivosDTO = await _servicoPedidoCompra.Upload(pedidoCompraArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{pedidoCompraArquivosDTO.NomeLogico}", memoryStream.ToArray());

                        var pedidoCompraNotaFiscalDTO = new PedidoCompraNotaFiscalDTO();
                        pedidoCompraNotaFiscalDTO.Id = 0;
                        pedidoCompraNotaFiscalDTO.Aprovada = null;
                        pedidoCompraNotaFiscalDTO.DataVencimento = DateTime.Now;
                        pedidoCompraNotaFiscalDTO.DataCadastro = DateTime.Now;
                        pedidoCompraNotaFiscalDTO.DataUltimaAlteracao = DateTime.Now;
                        pedidoCompraNotaFiscalDTO.Descricao = "";
                        pedidoCompraNotaFiscalDTO.IdPedidoCompra = idPedidoCompra;
                        pedidoCompraNotaFiscalDTO.IdPedidoCompraArquivo = pedidoCompraArquivosDTO.Id;
                        pedidoCompraNotaFiscalDTO.IdUsuarioAlteracao = IdUsuario;
                        pedidoCompraNotaFiscalDTO.IdUsuarioCadastro = IdUsuario;
                        pedidoCompraNotaFiscalDTO.Nome = pedidoCompraArquivosDTO.Nome;
                        pedidoCompraNotaFiscalDTO.NumeroNotaFiscal = "";
                        pedidoCompraNotaFiscalDTO.ImportadoParaFinanceiro = null;
                        pedidoCompraNotaFiscalDTO = await _servicoPedidoCompra.Post(pedidoCompraNotaFiscalDTO);

                        return Ok(pedidoCompraNotaFiscalDTO);
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

        [HttpPost("Conciliar/CancelarSaldoAbrirPedido")]
        public async Task<ActionResult> CancelarSaldoAbrirPedido([FromBody] Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario = IdUsuario;

                var result = await _servicoPedidoCompra.CancelarSaldoAbrirPedido(conciliacao_CancelarSaldoAbrirPedidoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Conciliar/TrocarFornecedor")]
        public async Task<ActionResult> TrocarFornecedor([FromBody] Conciliacao_CancelarSaldoAbrirPedidoDTO conciliacao_CancelarSaldoAbrirPedidoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                conciliacao_CancelarSaldoAbrirPedidoDTO.IdUsuario = IdUsuario;

                var result = await _servicoPedidoCompra.TrocarFornecedor(conciliacao_CancelarSaldoAbrirPedidoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("Cancelar")]
        public async Task<ActionResult> CancelarPedidoCompra(int idPedidoCompra, string motivoCancelamento)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var pedidoCompraDTO = await _servicoPedidoCompra.Get(idPedidoCompra);

                if (pedidoCompraDTO != null)
                {

                    if (string.IsNullOrEmpty(motivoCancelamento))
                        throw new Exception("Motivo do cancelamento inválido");

                    //var result = await _servicoPedidoCompra.Put(pedidoCompraDTO);

                    await _servicoPedidoCompra.CancelarPedido(pedidoCompraDTO.Id, motivoCancelamento);

                    await _servicoObra.CalculaCustoObra(_servicoPedidoCompra.Get(idPedidoCompra).Result?.IdCentroCustoObra ?? 0);

                    return Ok();
                }
                else
                    throw new Exception("Pedido de compra não identificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Finalizar")]
        public async Task<ActionResult> FinalizarPedidoCompra(int idPedidoCompra)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var pedidoCompraDTO = await _servicoPedidoCompra.Get(idPedidoCompra);

                if (pedidoCompraDTO != null)
                {
                    pedidoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    pedidoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                    pedidoCompraDTO.IdStatusPedidoCompra = 2;
                    
                    var result = await _servicoPedidoCompra.Put(pedidoCompraDTO);

                    return Ok(result);
                }
                else
                    throw new Exception("Pedido de compra não identificado");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Upload/{idPedidoCompra}")]
        public async Task<ActionResult> Upload([FromForm] IFormFile arquivo, [FromRoute] int idPedidoCompra)
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
                        var pedidoCompraArquivosDTO = new PedidoCompraArquivosDTO();
                        pedidoCompraArquivosDTO.Id = 0;
                        pedidoCompraArquivosDTO.DataCadastro = DateTime.Now;
                        pedidoCompraArquivosDTO.Extensao = extensao;
                        pedidoCompraArquivosDTO.IdPedidoCompra = idPedidoCompra;
                        pedidoCompraArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        pedidoCompraArquivosDTO.Nome = arquivo.FileName;
                        pedidoCompraArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        pedidoCompraArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        await _servicoPedidoCompra.Upload(pedidoCompraArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{pedidoCompraArquivosDTO.NomeLogico}", memoryStream.ToArray());

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

        [HttpGet("Download/{id}")]
        public IActionResult Download(Int64 id)
        {
            try
            {
                var arquivoDTO = _servicoPedidoCompra.GetFile(id).Result;

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
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Arquivos/{idPedidoCompra}")]
        public async Task<List<PedidoCompraArquivosDTO>> ObtemArquivos(Int64 idPedidoCompra)
        {
            return await _servicoPedidoCompra.ObtemArquivos(idPedidoCompra);
        }

        [HttpDelete("Arquivos/{idArquivo}")]
        public async Task<ActionResult> DeleteArquivo(int idArquivo)
        {
            try
            {
                var arquivoDTO = await _servicoPedidoCompra.GetFile(idArquivo);

                if (arquivoDTO == null)
                    throw new Exception("Arquivo não encontrado");

                System.IO.File.Delete($"{_configuration["DiretorioUpload"]}\\{arquivoDTO.NomeLogico}");

                await _servicoPedidoCompra.DeleteArquivo(idArquivo);
                
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Pdf/Download/{id}/{exibirDataPagamento}")]
        public IActionResult DownloadPdfPedidoCompra([FromRoute]Int64 id, [FromRoute]bool exibirDataPagamento = true)
        {
            try
            {
                var emailUsuario = (User.Identity as ClaimsIdentity).FindFirst("Email").Value;
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoPedidoCompra.GeraPdfPedidoCompra(id, exibirDataPagamento, logoPath, emailUsuario).Result,
                    "application/pdf",
                    "PedidoCompra.pdf");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CancelarSaldo")]
        public async Task<ActionResult> CancelarSaldo(PedidoCompraDevolucaoSaldoDTO pedidoCompraDevolucaoSaldoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
                pedidoCompraDevolucaoSaldoDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoCompraDevolucaoSaldoDTO.IdUsuarioCadastro = IdUsuario;
                pedidoCompraDevolucaoSaldoDTO.DataCadastro = DateTime.Now;
                pedidoCompraDevolucaoSaldoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoCompra.CancelarSaldo(pedidoCompraDevolucaoSaldoDTO);

                await _servicoObra.CalculaCustoObra(_servicoPedidoCompra.Get(pedidoCompraDevolucaoSaldoDTO.IdPedidoCompra).Result?.IdCentroCustoObra ?? 0);

                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("NotaFiscal/ObtemParaAprovacao")]
        public async Task<ActionResult> ObtemNotasFiscaisParaAprovacao()
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var result = await _servicoPedidoCompra.ObtemNotasFiscaisParaAprovacao(IdUsuario);

                return Ok(result);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotaFiscal/AprovarReprovar/{idPedidoCompraNotaFiscal}/{aprovada}")]
        public async Task<ActionResult> AprovarReprovarNotaFiscal([FromRoute] Int64 idPedidoCompraNotaFiscal, [FromRoute] bool aprovada)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.AprovarReprovarNotaFiscal(idPedidoCompraNotaFiscal, aprovada, IdUsuario);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotaFiscal/Cancelar/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> CancelarNotaFiscal([FromRoute] Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.CancelarNotaFiscal(idPedidoCompraNotaFiscal, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotaFiscal/AlterarObra/{idPedidoCompraNotaFiscal}/{idNovaObra}")]
        public async Task<ActionResult> AlterarObraNotaFiscal([FromRoute] Int64 idPedidoCompraNotaFiscal, [FromRoute] Int64 idNovaObra)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoPedidoCompra.AlterarObraNotaFiscal(idPedidoCompraNotaFiscal, idNovaObra, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("NotaFiscal/PostNotaFiscalPagamento")]
        public async Task<ActionResult> PostNotaFiscalPagamento(PedidoCompraNotaFiscalPagamentoDTO pagamentoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                if (pagamentoDTO.Id == 0)
                {
                    pagamentoDTO.Id = 0;
                    pagamentoDTO.IdUsuarioCadastro = IdUsuario;
                    pagamentoDTO.DataCadastro = DateTime.Now;
                }

                pagamentoDTO.IdUsuarioAlteracao = IdUsuario;
                pagamentoDTO.DataUltimaAlteracao = DateTime.Now;

                return Ok(await _servicoPedidoCompra.PostNotaFiscalPagamento(pagamentoDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("NotaFiscal/ObtemNotaFiscalPagamento/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> ObtemNotaFiscalPagamento([FromRoute] Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                var result = await _servicoPedidoCompra.ObtemNotaFiscalPagamento(idPedidoCompraNotaFiscal);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("NotaFiscal/ObtemNotasFiscaisAlteracaoData/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> ObtemNotasFiscaisAlteracaoData([FromRoute] Int64 idPedidoCompraNotaFiscal)
        {
            try
            {
                return Ok(await _servicoPedidoCompra.ObtemNotasFiscaisAlteracaoData(idPedidoCompraNotaFiscal));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UploadNotaFiscal/{idPedidoCompraNotaFiscal}")]
        public async Task<ActionResult> UploadNotaFiscal([FromForm] IFormFile arquivo, [FromRoute] int idPedidoCompraNotaFiscal)
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
                        var pedidoCompraNotaFiscal = await _servicoPedidoCompraNotaFiscal.Get(idPedidoCompraNotaFiscal);
                        var idArquivoAntigo = pedidoCompraNotaFiscal.IdPedidoCompraArquivo;


                        var pedidoCompraArquivosDTO = new PedidoCompraArquivosDTO();
                        pedidoCompraArquivosDTO.Id = 0;
                        pedidoCompraArquivosDTO.DataCadastro = DateTime.Now;
                        pedidoCompraArquivosDTO.Extensao = extensao;
                        pedidoCompraArquivosDTO.IdPedidoCompra = pedidoCompraNotaFiscal.IdPedidoCompra;
                        pedidoCompraArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        pedidoCompraArquivosDTO.Nome = arquivo.FileName;
                        pedidoCompraArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        pedidoCompraArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        pedidoCompraArquivosDTO = await _servicoPedidoCompra.Upload(pedidoCompraArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{pedidoCompraArquivosDTO.NomeLogico}", memoryStream.ToArray());

                        await _servicoPedidoCompraNotaFiscal.AtualizaArquivoNotaFiscal(idArquivoAntigo, pedidoCompraArquivosDTO.Id, pedidoCompraArquivosDTO.Nome);

                        await _servicoPedidoCompraNotaFiscal.ApagaArquivo(idArquivoAntigo, $"{_configuration["DiretorioUpload"]}");

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

        //[HttpPost("NotaFiscal/Alterar/Upload/{idPedidoCompraArquivo}")]
        //public async Task<ActionResult<PedidoCompraNotaFiscalDTO>> AlteracaoNF([FromForm] IFormFile arquivo, [FromRoute] int idPedidoCompraArquivo)
        //{
        //    try
        //    {
        //        var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

        //        var extensao = Path.GetExtension(arquivo.FileName).ToUpper();

        //        var extensoesPermitidas = new List<string>() { ".PDF" };

        //        if (extensoesPermitidas.Contains(extensao))
        //        {
        //            using (var memoryStream = new MemoryStream())
        //            {

        //                var pedidoCompraArquivoDTO = await _servicoPedidoCompra.ObtemArquivo(idPedidoCompraArquivo);

        //                var arquivoAntigo = $"{_configuration["DiretorioUpload"]}\\{pedidoCompraArquivoDTO.NomeLogico}";

        //                System.IO.File.Delete(arquivoAntigo);

        //                pedidoCompraArquivoDTO.Id = 0;
        //                pedidoCompraArquivoDTO.Extensao = extensao;
        //                pedidoCompraArquivoDTO.IdUsuarioCadastro = IdUsuario;
        //                pedidoCompraArquivoDTO.Nome = arquivo.FileName;
        //                pedidoCompraArquivoDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
        //                pedidoCompraArquivoDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
        //                pedidoCompraArquivoDTO = await _servicoPedidoCompra.AtualizaNF(pedidoCompraArquivoDTO);

        //                await arqu ivo.CopyToAsync(memoryStream);
        //                await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{pedidoCompraArquivoDTO.NomeLogico}", memoryStream.ToArray());

        //                return Ok();
        //            }

        //        }
        //        else
        //            throw new Exception("Extesão do arquivo não permitida");

        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}
    }
}
