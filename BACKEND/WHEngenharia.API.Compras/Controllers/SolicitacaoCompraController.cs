using AutoMapper;
using ClosedXML;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.Dominio.Modelos.Genericos.Cotacao;
using WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.API.Compras.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoCompraController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoSolicitacaoCompra _servicoSolicitacaoCompra;
        private ServicoObra _servicoObra;

        public SolicitacaoCompraController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoSolicitacaoCompra = new ServicoSolicitacaoCompra(_context, _mapper);
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SolicitacaoCompraDTO>> Get([FromRoute] Int64 id)
        {
            return Ok(await _servicoSolicitacaoCompra.Get(id));
        }

        [HttpGet("Status/{idStatus}")]
        public async Task<ActionResult<List<SolicitacaoCompraDTO>>> GetByStatus(Int64 idStatus)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                return Ok(await _servicoSolicitacaoCompra.GetByStatus(idStatus, IdUsuario));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Usuario")]
        public async Task<ActionResult> GetByUser(SolicitacaoCompra_RequestParametrosDTO parametros)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            return Ok(await _servicoSolicitacaoCompra.GetByUser(IdUsuario, parametros));
        }

        [HttpGet("ParaCotacao/{Id}")]
        public async Task<ActionResult<Cotacao_SolicitacaoCompraDTO>> GetParaCotacao([FromRoute] Int64 Id)
        {
            return Ok(await _servicoSolicitacaoCompra.GetParaCotacao(Id));
        }

        [HttpGet("ObtemQuantidades")]
        public async Task<ActionResult<SolicitacaoPedidoCompraQuantidadesDTO>> ObtemQuantidades()
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            return Ok(await _servicoSolicitacaoCompra.ObtemQuantidades(IdUsuario));
        }
        
        [HttpGet("Cotacoes")]
        public async Task<ActionResult<List<SolicitacaoCompraMateriaisCotacaoDTO>>> GetCotacoes(Int64 idSolicitacaoCompraMaterial)
        {
            return Ok(await _servicoSolicitacaoCompra.GetCotacoes(idSolicitacaoCompraMaterial));
        }

        [HttpPost]
        public async Task<ActionResult<SolicitacaoCompraDTO>> Post([FromBody] SolicitacaoCompra_Post solicitacaoCompraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                solicitacaoCompraDTO.IdUsuarioCadastro = IdUsuario;
                solicitacaoCompraDTO.DataCadastro = DateTime.Now;
                solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoSolicitacaoCompra.Post(solicitacaoCompraDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<SolicitacaoCompraDTO>> Put([FromBody] SolicitacaoCompraDTO solicitacaoCompraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoSolicitacaoCompra.Put(solicitacaoCompraDTO);

                return CreatedAtAction("Put", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cancelar")]
        public async Task<ActionResult> CancelarSolicitacaoCompra(int idSolicitacaoCompra, string motivoCancelamento)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                if (string.IsNullOrEmpty(motivoCancelamento))
                    throw new Exception("Motivo do cancelamento inválido");

                await _servicoSolicitacaoCompra.CancelarSolicitacaoCompra(idSolicitacaoCompra, motivoCancelamento);

                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EnviarSolicitacaoParaAprovacao/{id}")]
        public async Task<ActionResult> EnviarSolicitacaoParaAprovacao(int id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraDTO = await _servicoSolicitacaoCompra.Get(id);

                if (solicitacaoCompraDTO != null)
                {
                    solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                    solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 1;

                    var result = await _servicoSolicitacaoCompra.Put(solicitacaoCompraDTO);

                    return Ok(result);
                }
                else
                    throw new Exception("Solicitação de compra não identificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Validar/{id}/{idComprador}")]
        public async Task<ActionResult> ValidarSolicitacaoCompra(int id, Int64 idComprador)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraDTO = await _servicoSolicitacaoCompra.Get(id);

                if (solicitacaoCompraDTO != null)
                {
                    solicitacaoCompraDTO.IdEngenheiroAprovador = IdUsuario;
                    solicitacaoCompraDTO.DataAprovacaoEngenheiro = DateTime.Now;
                    solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                    solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 2;

                    if (solicitacaoCompraDTO.IdUsuarioComprador == null && idComprador == 0)
                        throw new Exception("É necessário informar um comprador para a solicitação de compra");
                    else if (solicitacaoCompraDTO.IdUsuarioComprador == null && idComprador > 0)
                        solicitacaoCompraDTO.IdUsuarioComprador = idComprador;

                        var result = await _servicoSolicitacaoCompra.Put(solicitacaoCompraDTO);

                    return Ok(result);
                }
                else
                    throw new Exception("Solicitação de compra não identificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Reprovar/{id}")]
        public async Task<ActionResult> ReprovarSolicitacaoCompra(int id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoSolicitacaoCompra.ReprovarSolicitacaoCompra(id, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Reabrir")]
        public async Task<ActionResult> ReabrirSolicitacaoCompra([FromBody] SolicitacaoCompra_ReaberturaDTO solicitacaoCompra_ReaberturaDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraDTO = await _servicoSolicitacaoCompra.Get(solicitacaoCompra_ReaberturaDTO.IdSolicitacaoCompra);
                
                if (solicitacaoCompraDTO != null)
                {
                    solicitacaoCompraDTO.IdCentroCustoObra = solicitacaoCompra_ReaberturaDTO.IdObraSelecionada;
                    solicitacaoCompraDTO.DataEntrega = solicitacaoCompra_ReaberturaDTO.DataEntrega;
                    solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    solicitacaoCompraDTO.IdUsuarioCadastro = IdUsuario;

                    var result = await _servicoSolicitacaoCompra.Reabrir(solicitacaoCompraDTO, solicitacaoCompra_ReaberturaDTO.Materiais);

                    return Ok(result);
                }
                else
                    throw new Exception("Solicitação de compra não identificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost("Cotacao")]
        public async Task<ActionResult<SolicitacaoCompraMateriaisCotacaoDTO>> Post([FromBody] SolicitacaoCompraMateriaisCotacaoDTO solicitacaoCompraMateriaisCotacaoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var result = await _servicoSolicitacaoCompra.Post(solicitacaoCompraMateriaisCotacaoDTO, IdUsuario);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Fornecedor")]
        public async Task<ActionResult> PostFornecedor([FromBody] SolicitacaoCompra_PostFornecedor fornecedores)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoSolicitacaoCompra.PostFornecedor(fornecedores, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/EnviarDiretoria")]
        public async Task<ActionResult> EnviarDiretoria(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                objetoAprovacaoDTO.IdUsuarioFinalizacaoCotacao = IdUsuario;

                await _servicoSolicitacaoCompra.EnviarDiretoria(objetoAprovacaoDTO);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/DevolverCompras")]
        public async Task<ActionResult> DevolverCompras(SolicitacaoCompra_ObjetoAprovacaoDTO objetoAprovacaoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                objetoAprovacaoDTO.IdDiretorAprovador = IdUsuario;
                objetoAprovacaoDTO.DataAprovacaoDiretor = DateTime.Now;

                await _servicoSolicitacaoCompra.DevolverCompras(objetoAprovacaoDTO);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/DefinirFornecedorPrincipal")]
        public async Task<ActionResult> DefinirFornecedorPrincipal(Int64 idSolicitacaoCompra, Int64 idFornecedor)
        {
            try
            {
                await _servicoSolicitacaoCompra.DefinirFornecedorPrincipal(idSolicitacaoCompra, idFornecedor);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/DefinirDataEntregaFornecedor")]
        public async Task<ActionResult> DefinirDataEntregaFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor, DateTime? dataEntrega)
        {
            try
            {
                await _servicoSolicitacaoCompra.DefinirDataEntregaFornecedor(idSolicitacaoCompra, idFornecedor, dataEntrega);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/ExcluirFornecedor")]
        public async Task<ActionResult> ExcluirFornecedor(Int64 idSolicitacaoCompra, Int64 idFornecedor)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoSolicitacaoCompra.ExcluirFornecedor(idSolicitacaoCompra, idFornecedor, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Principal/{Id}")]
        public async Task<ActionResult> PostCotacaoPrincipal([FromRoute] Int64 Id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoSolicitacaoCompra.PostCotacaoPrincipal(Id, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Finalizar/{Id}")]
        public async Task<ActionResult> PostCotacaoFinalizar([FromRoute] Int64 Id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraDTO = await _servicoSolicitacaoCompra.Get(Id);

                if (solicitacaoCompraDTO != null)
                {
                    if (solicitacaoCompraDTO.Materiais.Any(x => x.Cotacoes.Count == 0))
                        throw new Exception("Existe algum material da solicitação sem cotação");

                    solicitacaoCompraDTO.Materiais.ForEach(x =>
                    {
                        if (!x.Cotacoes.Any(y => y.QuantidadeCotado > 0))
                            throw new Exception($"Material {x.Material.Descricao} com quantidade zero para compra");

                        if (!x.Cotacoes.Any(y=>y.CotacaoFinal))
                        {
                            throw new Exception($"Material {x.Material.Descricao} sem cotação principal");
                        }
                    });

                    if (await _servicoSolicitacaoCompra.VerificaDivergenciaValorCondicaoManual(solicitacaoCompraDTO.Id))
                        throw new Exception("Existe alguma cotação manual com divergência no valor apontado e valor total do pedido no fornecedor");

                    solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    solicitacaoCompraDTO.IdUsuarioFinalizacaoCotacao = IdUsuario;
                    solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;
                    solicitacaoCompraDTO.DataFinalizacaoCotacao = DateTime.Now;

                    var result = await _servicoSolicitacaoCompra.FinalizarCotacao(solicitacaoCompraDTO);

                    return Ok(result);
                }
                else
                    throw new Exception("Solicitação de compra não identificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Aprovar/{Id}")]
        public async Task<ActionResult> PostCotacaoAprovar([FromRoute] Int64 Id)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraDTO = await _servicoSolicitacaoCompra.Get(Id);

                if (solicitacaoCompraDTO != null)
                {
                    solicitacaoCompraDTO.IdUsuarioAlteracao = IdUsuario;
                    solicitacaoCompraDTO.DataUltimaAlteracao = DateTime.Now;

                    solicitacaoCompraDTO.IdStatusSolicitacaoCompra = 4;

                    var result = await _servicoSolicitacaoCompra.AprovarCotacao(solicitacaoCompraDTO);

                    await _servicoSolicitacaoCompra.MontaPedidoCompra(solicitacaoCompraDTO.Id, IdUsuario);

                    await _servicoObra.CalculaCustoObra(solicitacaoCompraDTO.IdCentroCustoObra ?? 0);

                    return Ok(result);
                }
                else
                    throw new Exception("Solicitação de compra não identificada");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Reprovar/{Id}")]
        public async Task<ActionResult> PostCotacaoReprovar([FromRoute] Int64 Id, string comentario)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var result = await _servicoSolicitacaoCompra.PostCotacaoReprovar(Id);

                var solicitacaoCompraComentarioDTO = new SolicitacaoCompraComentariosDTO();
                solicitacaoCompraComentarioDTO.DataCadastro = DateTime.Now;
                solicitacaoCompraComentarioDTO.Id = 0;
                solicitacaoCompraComentarioDTO.IdSolicitacaoCompra = Id;
                solicitacaoCompraComentarioDTO.IdUsuarioCadastro = IdUsuario;
                solicitacaoCompraComentarioDTO.Observacao = comentario;

                await _servicoSolicitacaoCompra.PostComentario(solicitacaoCompraComentarioDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("Cotacao/Editar")]
        public async Task<ActionResult<SolicitacaoCompraMateriaisCotacaoDTO>> EditarCotacao([FromBody] Cotacao_SolicitacaoCompraDTO cotacaoSolicitacaoCompraDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                if (cotacaoSolicitacaoCompraDTO.Fornecedores.Any(x => x.CondicaoPagamento == null))
                    throw new Exception($"Escolha uma condição de pagamento para o fornecedor {cotacaoSolicitacaoCompraDTO.Fornecedores.FirstOrDefault(x => x.CondicaoPagamento == null)?.NomeFantasia}");

                await _servicoSolicitacaoCompra.EditarCotacao(cotacaoSolicitacaoCompraDTO, IdUsuario);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Cotacoes/ObtemParaAprovacao")]
        public async Task<ActionResult> ObtemCotacaoParaAprovacao()
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                return Ok(await _servicoSolicitacaoCompra.ObtemCotacaoParaAprovacao(IdUsuario));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("Upload/{idSolicitacaoCompra}")]
        public async Task<ActionResult> Upload([FromForm] IFormFile arquivo, [FromRoute] int idSolicitacaoCompra)
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
                        var solicitacaoCompraArquivosDTO = new SolicitacaoCompraArquivosDTO();
                        solicitacaoCompraArquivosDTO.Id = 0;
                        solicitacaoCompraArquivosDTO.DataCadastro = DateTime.Now;
                        solicitacaoCompraArquivosDTO.Extensao = extensao;
                        solicitacaoCompraArquivosDTO.IdSolicitacaoCompra = idSolicitacaoCompra;
                        solicitacaoCompraArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        solicitacaoCompraArquivosDTO.Nome = arquivo.FileName;
                        solicitacaoCompraArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        solicitacaoCompraArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        await _servicoSolicitacaoCompra.Upload(solicitacaoCompraArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{solicitacaoCompraArquivosDTO.NomeLogico}", memoryStream.ToArray());

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
                var arquivoDTO = _servicoSolicitacaoCompra.GetFile(id).Result;

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

        [HttpGet("Arquivos/{idSolicitacaoCompra}")]
        public async Task<List<SolicitacaoCompraArquivosDTO>> ObtemArquivos(Int64 idSolicitacaoCompra)
        {
            return await _servicoSolicitacaoCompra.ObtemArquivos(idSolicitacaoCompra);
        }

        [HttpDelete("Arquivos/{idArquivo}")]
        public async Task<ActionResult> DeleteArquivo(int idArquivo)
        {
            try
            {
                var arquivoDTO = await _servicoSolicitacaoCompra.GetFile(idArquivo);

                if (arquivoDTO == null)
                    throw new Exception("Arquivo não encontrado");

                System.IO.File.Delete($"{_configuration["DiretorioUpload"]}\\{arquivoDTO.NomeLogico}");

                await _servicoSolicitacaoCompra.DeleteArquivo(idArquivo);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Comentario/{idSolicitacaoCompra}")]
        public async Task<ActionResult<List<SolicitacaoCompraComentariosDTO>>> Comentarios([FromRoute]Int64 idSolicitacaoCompra)
        {
            return Ok(await _servicoSolicitacaoCompra.Comentarios(idSolicitacaoCompra));
        }

        [HttpPost("Comentario")]
        public async Task<ActionResult> PostComentario(Int64 id, string comentario)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var solicitacaoCompraComentarioDTO = new SolicitacaoCompraComentariosDTO();
                solicitacaoCompraComentarioDTO.DataCadastro = DateTime.Now;
                solicitacaoCompraComentarioDTO.Id = 0;
                solicitacaoCompraComentarioDTO.IdSolicitacaoCompra = id;
                solicitacaoCompraComentarioDTO.IdUsuarioCadastro = IdUsuario;
                solicitacaoCompraComentarioDTO.Observacao = comentario;
                
                await _servicoSolicitacaoCompra.PostComentario(solicitacaoCompraComentarioDTO);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Pdf/Download/{id}")]
        public IActionResult DownloadPdfPedidoCompra([FromRoute] Int64 id)
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoSolicitacaoCompra.GeraPdfPedidoCompra(id, logoPath).Result,
                    "application/pdf",
                    "PedidoCompra.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Pdf/Cotacoes/Download/{id}")]
        public IActionResult DownloadPdfCotacoes([FromRoute] Int64 id)
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoSolicitacaoCompra.GeraPdfCotacoes(id, logoPath).Result,
                    "application/pdf",
                    "PedidoCompra.pdf");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Excel/Cotacoes/Download/{id}")]
        public IActionResult DownloadExcelCotacoes([FromRoute] Int64 id)
        {
            //try
            //{
            //    var solicitacaoDTO = _servicoSolicitacaoCompra.Get(id).Result;
            //    solicitacaoDTO.Materiais = solicitacaoDTO.Materiais.OrderBy(x => x.Material.Descricao).ToList();

            //    int maiorLinha = 20;
            //    int maiorColuna = 18;

            //    using var workbook = new XLWorkbook();

            //    var worksheet = workbook.Worksheets.Add("Cotacoes");

            //    var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";
            //    var image = worksheet.AddPicture(logoPath)
            //        .MoveTo(1, 1);

            //    worksheet.Row(2).Height = 30;

            //    worksheet.Range(1, 1, 1, 3).Merge();
            //    worksheet.Range(1, 4, 1, 13).Merge();
            //    worksheet.Range(1, 14, 1, 16).Merge();
            //    worksheet.Range(1, 17, 1, 18).Merge();

            //    worksheet.Range(3, 1, 3, 3).Merge();
            //    worksheet.Range(3, 4, 3, 10).Merge();
            //    worksheet.Range(3, 11, 3, 14).Merge();

            //    worksheet.Range(4, 1, 4, 10).Merge();
            //    worksheet.Range(4, 11, 4, 14).Merge();

            //    worksheet.Range(5, 1, 7, 1).Merge();
            //    worksheet.Range(5, 2, 5, 10).Merge();
            //    worksheet.Range(5, 11, 5, 14).Merge();

            //    worksheet.Range(6, 2, 6, 10).Merge();
            //    worksheet.Range(6, 11, 7, 11).Merge();
            //    worksheet.Range(6, 12, 7, 12).Merge();
            //    worksheet.Range(6, 13, 6, 14).Merge();

            //    worksheet.Range(7, 2, 7, 10).Merge();

            //    worksheet.Cell(1, 4).Value = $"SOLICITAÇÃO DE COMPRAS";
            //    worksheet.Cell(1, 14).Value = $"OC Nº {solicitacaoDTO?.Codigo}";
            //    worksheet.Cell(1, 17).Value = $"DATA {solicitacaoDTO?.DataCadastro.ToString("dd/MM/yyyy")}";

            //    worksheet.Cell(1, 4).Value = $"MATERIAIS E SERVIÇOS";

            //    worksheet.Cell(3, 1).Value = $"Contrato Nº {solicitacaoDTO?.CentroCustoObra?.CodigoProposta}";
            //    worksheet.Cell(3, 4).Value = $"Cliente: {solicitacaoDTO?.CentroCustoObra?.Cliente?.NomeFantasia}";
            //    worksheet.Cell(3, 11).Value = $"FORNECEDOR";

            //    worksheet.Cell(4, 11).Value = $"TELEFONE";

            //    worksheet.Cell(5, 1).Value = $"ITEM";
            //    worksheet.Cell(5, 2).Value = $"ESPECIFICAÇÃO";
            //    worksheet.Cell(5, 11).Value = $"CONTATO";

            //    worksheet.Cell(6, 2).Value = $"MATERIAIS: MODELO, MARCA, CÓDIGO, COR E UNIDADE";
            //    worksheet.Cell(6, 11).Value = $"UNIDADE";
            //    worksheet.Cell(6, 12).Value = $"QTD";
            //    worksheet.Cell(6, 13).Value = $"CUSTO ORÇADO";

            //    worksheet.Cell(7, 2).Value = $"SERVIÇOS: DESCRIÇÃO COMPLETA A SER EXECUTADO";
            //    worksheet.Cell(7, 13).Value = $"UNITÁRIO";
            //    worksheet.Cell(7, 14).Value = $"TOTAL";


            //    var fornecedoresDTO = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.Select(y => y.Fornecedor).OrderBy(x => x.Nome).ToList();

            //    if (fornecedoresDTO.Count > 2)
            //        maiorColuna = 18 + ((fornecedoresDTO.Count - 2) * 2);

            //    maiorColuna = maiorColuna + 2;

            //    worksheet.Range(2, 1, 2, maiorColuna).Merge();

            //    int coluna = 15;
            //    int numeroFornecedor = 1;

            //    fornecedoresDTO.ForEach(x =>
            //    {
            //        worksheet.Cell(3, coluna).Value = $"{x.NomeFantasia}";
            //        worksheet.Cell(4, coluna).Value = $"{x.TelefoneFixo}";
            //        worksheet.Cell(5, coluna).Value = $"{x.NomeVendedor}";

            //        worksheet.Cell(6, coluna).Value = $"PREÇOS";

            //        worksheet.Cell(7, coluna).Value = $"UNITÁRIO";
            //        worksheet.Cell(7, coluna + 1).Value = $"TOTAL";

            //        worksheet.Range(3, coluna, 3, coluna + 1).Merge();
            //        worksheet.Range(4, coluna, 4, coluna + 1).Merge();
            //        worksheet.Range(5, coluna, 5, coluna + 1).Merge();
            //        worksheet.Range(6, coluna, 6, coluna + 1).Merge();

            //        if (numeroFornecedor % 2 != 0)
            //        {
            //            worksheet.Cell(3, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(4, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(5, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(6, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(7, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(7, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //        }
            //        else
            //        {
            //            worksheet.Cell(3, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(4, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(5, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(6, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(7, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(7, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //        }

            //        coluna += 2;
            //        numeroFornecedor++;
            //    });

            //    worksheet.Cell(6, coluna).Value = $"MELHOR PREÇO";
            //    worksheet.Cell(7, coluna).Value = $"UNITÁRIO";
            //    worksheet.Cell(7, coluna + 1).Value = $"TOTAL";

            //    worksheet.Range(3, coluna, 3, coluna + 1).Merge();
            //    worksheet.Range(4, coluna, 4, coluna + 1).Merge();
            //    worksheet.Range(5, coluna, 5, coluna + 1).Merge();
            //    worksheet.Range(6, coluna, 6, coluna + 1).Merge();


            //    int linha = 8;

            //    worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            //    linha++;

            //    var count = 1;
            //    solicitacaoDTO?.Materiais?.ForEach(material =>
            //    {
            //        worksheet.Range(linha, 2, linha, 10).Merge();

            //        worksheet.Cell(linha, 1).Value = $"{count}";
            //        worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //        worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //        worksheet.Cell(linha, 1).Style.Font.SetBold(true);

            //        if (solicitacaoDTO.Servico)
            //            worksheet.Cell(linha, 2).Value = $"{material.Material.Descricao} - {solicitacaoDTO.Observacao}";
            //        else
            //            worksheet.Cell(linha, 2).Value = $"{material.Material.Descricao}";

            //        worksheet.Cell(linha, 11).Value = $"{material.Material.UnidadeMaterial.Codigo}";
            //        worksheet.Cell(linha, 12).Value = $"{material.Quantidade}";

            //        worksheet.Cell(linha, 13).Value = $"{(material.ValorUnitarioEstimado ?? 0).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
            //        worksheet.Cell(linha, 14).Value = $"{((material.ValorUnitarioEstimado ?? 0) * (material.Quantidade)).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //        worksheet.Cell(linha, 13).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
            //        worksheet.Cell(linha, 14).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);

            //        coluna = 15;
            //        numeroFornecedor = 1;
            //        material.Cotacoes.OrderBy(x => x.Fornecedor.Nome).ToList().ForEach(cotacao =>
            //        {
            //            worksheet.Cell(linha, coluna).Value = $"{cotacao.ValorUnitarioComDesconto.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
            //            worksheet.Cell(linha, coluna + 1).Value = $"{(cotacao.ValorUnitarioComDesconto * material.Quantidade).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //            if (numeroFornecedor % 2 != 0)
            //            {
            //                worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //                worksheet.Cell(linha, coluna +1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            }
            //            else
            //            {
            //                worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //                worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            }

            //            numeroFornecedor++;
            //            coluna += 2;
            //        });

            //        var cotacoesMaiorQueZero = material.Cotacoes.Where(x => x.ValorUnitarioComDesconto > 0).ToList();
            //        var menorValor = 0.0;

            //        if (cotacoesMaiorQueZero.Count > 0)
            //            menorValor = cotacoesMaiorQueZero.Min(x => x.ValorUnitarioComDesconto);

            //        worksheet.Cell(linha, coluna).Value = $"{menorValor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";
            //        worksheet.Cell(linha, coluna + 1).Value = $"{(menorValor * material.Quantidade).ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //        linha++;
            //        count++;
            //    });

            //    worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            //    linha++;

            //    worksheet.Range(linha, 1, linha, 10).Merge();
            //    worksheet.Range(linha, 11, linha, 12).Merge();
            //    worksheet.Range(linha, 13, linha, 14).Merge();
            //    worksheet.Cell(linha, 1).Value = $"DATA DE ENTREGA NECESSÁRIA: {solicitacaoDTO.DataEntrega?.ToString("dd/MM/yyyy")}";

            //    coluna = 14;
            //    var total = 0.0;
            //    for (int i = 9; i < (9 + solicitacaoDTO.Materiais.Count()); i++)
            //    {
            //        total = total + Double.Parse(worksheet.Cell(i, coluna).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));
            //    }

            //    //não possui valor dos materiais, então verificar se possui um valor estimado
            //    if (total == 0)
            //        total = solicitacaoDTO.ValorEstimado??0;

            //    worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
            //    worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(XLColor.LightSteelBlue);
            //    worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, coluna - 1).Value = $"{total.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";


            //    coluna = 16;
            //    numeroFornecedor = 1;
            //    fornecedoresDTO.ForEach(fornecedor =>
            //    {
            //        worksheet.Range(linha, coluna - 1, linha, coluna).Merge();

            //        var total = 0.0;
            //        for (int i = 9; i < (9 + solicitacaoDTO.Materiais.Count()); i++)
            //        {
            //            total = total + Double.Parse(worksheet.Cell(i, coluna).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));
            //        }

            //        var cotacao = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);
            //        total += cotacao.Frete??0;
            //        total += cotacao.Imposto ?? 0;

            //        worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
            //        worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //        worksheet.Cell(linha, coluna - 1).Value = $"{total.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //        if (numeroFornecedor % 2 != 0)
            //            worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //        else
            //            worksheet.Cell(linha, coluna - 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            //        numeroFornecedor++;
            //        coluna += 2;

            //    });

            //    #region Soma do melhor preço

            //    var menorValor = Double.Parse(worksheet.Cell(linha, 15).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));

            //    for(int i=0;i<fornecedoresDTO.Count;i++)
            //    {
            //        var valorParcial = Double.Parse(worksheet.Cell(linha, 15 + (i*2)).Value.GetText(), CultureInfo.CreateSpecificCulture("pt-BR"));

            //        if (valorParcial < menorValor && valorParcial > 0)
            //            menorValor = valorParcial;
            //    }

            //    worksheet.Range(linha, coluna - 1, linha, coluna).Merge();

            //    worksheet.Cell(linha, coluna - 1).Style.Font.SetBold(true);
            //    worksheet.Cell(linha, coluna - 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, coluna - 1).Value = $"{menorValor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //    #endregion

            //    linha++;
                

            //    worksheet.Range(linha, 1, linha, 14).Merge();
            //    worksheet.Range(linha + 1, 1, linha + 1, 14).Merge();

            //    coluna = 15;
            //    numeroFornecedor = 1;
            //    fornecedoresDTO.ForEach(fornecedor =>
            //    {
            //        var cotacao = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);

            //        worksheet.Cell(linha, coluna).Value = "Frete";
            //        worksheet.Cell(linha + 1, coluna).Value = "Imposto";
            //        worksheet.Cell(linha, coluna + 1).Value = cotacao.Frete?.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "");
            //        worksheet.Cell(linha + 1, coluna + 1).Value = cotacao.Imposto?.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "");
                   
            //        if (numeroFornecedor % 2 != 0)
            //        {
            //            worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(linha + 1, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            worksheet.Cell(linha + 1, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //        }
            //        else
            //        {
            //            worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(linha, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(linha + 1, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //            worksheet.Cell(linha + 1, coluna + 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //        }

            //        numeroFornecedor++;

            //        coluna += 2;

            //    });

            //    linha += 2;

            //    var linhaAuxiliar = linha;
            //    coluna = 15;

            //    numeroFornecedor = 1;
            //    fornecedoresDTO.ForEach(fornecedor =>
            //    {
            //        var cotacao = solicitacaoDTO.Materiais.FirstOrDefault().Cotacoes.FirstOrDefault(x => x.IdFornecedor == fornecedor.Id);
            //        var condicaoPagamento = cotacao.CondicaoPagamento;

            //        worksheet.Cell(linha, coluna).Value = condicaoPagamento?.Descricao ?? "";

            //        if(condicaoPagamento.Descricao=="Manual")
            //        {
            //            cotacao.PagamentoManual.ForEach(pm =>
            //            {
            //                worksheet.Range(linhaAuxiliar, 1, linhaAuxiliar, 14).Merge();
            //                worksheet.Cell(linhaAuxiliar, coluna +1).Value = $"{pm.Data.ToString("dd/MM/yyyy")} - {pm.Valor.ToString("C2", CultureInfo.CreateSpecificCulture("pt-BR")).Replace("R$", "")}";

            //                if (numeroFornecedor % 2 != 0)
            //                    worksheet.Cell(linhaAuxiliar, coluna+1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //                else
            //                    worksheet.Cell(linhaAuxiliar, coluna+1).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            //                linhaAuxiliar++;
            //            });
            //        }
            //        else
            //        {
            //            if (numeroFornecedor % 2 != 0)
            //                worksheet.Cell(linha, coluna+1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //            else
            //                worksheet.Cell(linha, coluna+1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //        }

            //        if (numeroFornecedor % 2 != 0)
            //            worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //        else
            //            worksheet.Cell(linha, coluna).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            //        numeroFornecedor++;

            //        coluna += 2;

            //    });


            //    linha = linhaAuxiliar;

                



            //    worksheet.Range(linha, 1, linha, maiorColuna).Merge();

            //    linha++;

            //    worksheet.Range(linha, 1, linha, 12).Merge();
            //    worksheet.Range(linha, 13, linha, maiorColuna).Merge();
            //    worksheet.Cell(linha, 1).Value = $"APROVAÇÕES";

            //    worksheet.Cell(linha, 1).Style.Font.SetBold(true);
            //    worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //    linha++;

            //    worksheet.Range(linha, 1, linha, 2).Merge();
            //    worksheet.Range(linha, 3, linha, 4).Merge();
            //    worksheet.Range(linha, 5, linha, 6).Merge();
            //    worksheet.Range(linha, 7, linha, 8).Merge();
            //    worksheet.Range(linha, 9, linha, 10).Merge();
            //    worksheet.Range(linha, 11, linha, 12).Merge();

            //    worksheet.Cell(linha, 1).Value = $"{solicitacaoDTO.DataCadastro.ToString("dd/MM/yyyy")}";
            //    worksheet.Cell(linha, 3).Value = $"{solicitacaoDTO.UsuarioCadastro.Nome}";
            //    worksheet.Cell(linha, 5).Value = $"{solicitacaoDTO.DataAprovacaoEngenheiro?.ToString("dd/MM/yyyy")}";
            //    worksheet.Cell(linha, 7).Value = $"{solicitacaoDTO.UsuarioEngenheiroAprovador?.Nome}";
            //    worksheet.Cell(linha, 9).Value = $"{solicitacaoDTO.DataAprovacaoDiretor?.ToString("dd/MM/yyyy")}";
            //    worksheet.Cell(linha, 11).Value = $"{solicitacaoDTO.UsuarioDiretorAprovador?.Nome}";

            //    worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 3).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 5).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 7).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 9).Style.Fill.SetBackgroundColor(XLColor.Beige);
            //    worksheet.Cell(linha, 11).Style.Fill.SetBackgroundColor(XLColor.Beige);

            //    worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //    worksheet.Range(linha, 13, linha, maiorColuna).Merge();

            //    linha++;

            //    worksheet.Range(linha, 1, linha, 4).Merge();
            //    worksheet.Range(linha, 5, linha, 8).Merge();
            //    worksheet.Range(linha, 9, linha, 12).Merge();
            //    worksheet.Range(linha, 13, linha, maiorColuna).Merge();

            //    worksheet.Cell(linha, 1).Value = $"SOLICITANTE";
            //    worksheet.Cell(linha, 5).Value = $"ENGENHEIRO";
            //    worksheet.Cell(linha, 9).Value = $"DIRETOR";

            //    worksheet.Cell(linha, 1).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //    worksheet.Cell(linha, 5).Style.Fill.SetBackgroundColor(XLColor.LightGray);
            //    worksheet.Cell(linha, 9).Style.Fill.SetBackgroundColor(XLColor.LightGray);

            //    worksheet.Cell(linha, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);
            //    worksheet.Cell(linha, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Center);

            //    maiorLinha = linha;

            //    //for (var index = 1; index <= result.Count; index++)
            //    //{
            //    //    worksheet.Cell(index + 1, 1).Value = result[index - 1].CodigoObra;
            //    //    worksheet.Cell(index + 1, 2).Value = result[index - 1].Cliente;
            //    //    worksheet.Cell(index + 1, 3).Value = result[index - 1].Escopo;
            //    //    worksheet.Cell(index + 1, 4).Value = result[index - 1].ETO;
            //    //    worksheet.Cell(index + 1, 5).Value = result[index - 1].Gasto;
            //    //    worksheet.Cell(index + 1, 6).Value = result[index - 1].Saldo;
            //    //}

            //    for (int i = 1; i <= maiorLinha; i++)
            //    {
            //        for (int j = 1; j <= maiorColuna; j++)
            //        {
            //            worksheet.Cell(i, j).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
            //            worksheet.Cell(i, j).Style.Border.LeftBorder = XLBorderStyleValues.Medium;
            //            worksheet.Cell(i, j).Style.Border.RightBorder = XLBorderStyleValues.Medium;
            //            worksheet.Cell(i, j).Style.Border.TopBorder = XLBorderStyleValues.Medium;
            //        }
            //    }


            //    worksheet.Columns().AdjustToContents();

            //    using var stream = new MemoryStream();

            //    workbook.SaveAs(stream);

            //    var content = stream.ToArray();



            //    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Cotacoes.xlsx");
            //}
            //catch (Exception ex)
            //{
            //    return BadRequest(ex.Message);
            //}

            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                var excel = _servicoSolicitacaoCompra.ExcelCotacoes(id, logoPath).Result;

                return File(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Cotacoes.xlsx");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RemoverItensSelecionados")]
        public async Task<ActionResult> RemoverItensSelecionados([FromBody] Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {
            try
            {
                return Ok(await _servicoSolicitacaoCompra.RemoverItensSelecionados(cotacao_SolicitacaoCompraDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DuplicarItensSelecionados")]
        public async Task<ActionResult> DuplicarItensSelecionados([FromBody] Cotacao_SolicitacaoCompraDTO cotacao_SolicitacaoCompraDTO)
        {
            try
            {
                return Ok(await _servicoSolicitacaoCompra.DuplicarItensSelecionados(cotacao_SolicitacaoCompraDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EnviarParaRevisao/{idSolicitacaoCompra}")]
        public async Task<ActionResult> EnviarParaRevisao([FromRoute] Int64 idSolicitacaoCompra)
        {
            try
            {
                await _servicoSolicitacaoCompra.EnviarParaRevisao(idSolicitacaoCompra);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EnviarParaAprovacao/{idSolicitacaoCompra}")]
        public async Task<ActionResult> EnviarParaAprovacao([FromRoute] Int64 idSolicitacaoCompra)
        {
            try
            {
                await _servicoSolicitacaoCompra.EnviarParaAprovacao(idSolicitacaoCompra);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cotacao/Material/{idMaterial}/{idSolicitacaoCompra}")]
        public async Task<ActionResult> ExcluirMaterialCotacao([FromRoute]Int64 idMaterial, [FromRoute] Int64 idSolicitacaoCompra)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                await _servicoSolicitacaoCompra.ExcluirMaterialCotacao(idMaterial, idSolicitacaoCompra);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
