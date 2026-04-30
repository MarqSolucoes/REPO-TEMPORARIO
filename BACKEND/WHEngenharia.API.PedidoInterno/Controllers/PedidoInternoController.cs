using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.PedidoInterno;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.PedidoInterno.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PedidoInternoController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoPedidoInterno _servicoPedidoInterno;

        public PedidoInternoController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoPedidoInterno = new ServicoPedidoInterno(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<PedidoInternoDTO>>> Get()
        {
            return Ok(await _servicoPedidoInterno.Get());
        }

        [HttpGet("ObtemPedidosInternosParaAprovacao/{idUsuario}")]
        public async Task<ActionResult<List<PedidoInternoDTO>>> ObtemPedidosInternosParaAprovacao(Int64 idUsuario)
        {
            return Ok(await _servicoPedidoInterno.ObtemPedidosInternosParaAprovacao(idUsuario));
        }

        [HttpPost("AprovarReprovarPedidoInterno/{idPedidoInterno}/{valor}")]
        public async Task<ActionResult> AprovarReprovarPedidoInterno([FromRoute] Int64 idPedidoInterno, [FromRoute] bool valor)
        {
            try
            {
                await _servicoPedidoInterno.AprovarReprovarPedidoInterno(idPedidoInterno, valor);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<PedidoInternoDTO>> Get([FromRoute] Int64 Id)
        {
            return Ok(await _servicoPedidoInterno.Get(Id));
        }

        [HttpGet("Usuario")]
        public async Task<ActionResult<List<PedidoInternoDTO>>> GetByUser()
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            return Ok(await _servicoPedidoInterno.GetByUser(IdUsuario));
        }

        [HttpGet("ProximoCodigo")]
        public async Task<ActionResult> ProximoCodigo()
        {
            try
            {
                return Ok(await _servicoPedidoInterno.ProximoCodigo());
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] PedidoInternoDTO pedidoInternoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                pedidoInternoDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoInternoDTO.IdUsuarioCadastro = IdUsuario;
                pedidoInternoDTO.DataCadastro = DateTime.Now;
                pedidoInternoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoInterno.Post(pedidoInternoDTO);

                return CreatedAtAction("Post", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DefinirParcelaPaga/{idParcela}")]
        public async Task<ActionResult> DefinirParcelaPaga([FromRoute] Int64 idParcela)
        {
            try
            {
                await _servicoPedidoInterno.DefinirParcelaPaga(idParcela);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("AlterarParcela")]
        public async Task<ActionResult> AlterarParcela(PedidoInterno_AlteracaoParcelaDTO pedidoInternoAlteracaoDataPagamentoDTO)
        {
            try
            {
                await _servicoPedidoInterno.AlterarParcela(pedidoInternoAlteracaoDataPagamentoDTO);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<PedidoInternoDTO>> Put([FromBody] PedidoInternoDTO pedidoInternoDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                pedidoInternoDTO.IdUsuarioAlteracao = IdUsuario;
                pedidoInternoDTO.DataUltimaAlteracao = DateTime.Now;

                var result = await _servicoPedidoInterno.Put(pedidoInternoDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Upload/{idPedidoInterno}")]
        public async Task<ActionResult> Upload([FromForm] IFormFile arquivo, [FromRoute] int idPedidoInterno)
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
                        var pedidoInternoArquivosDTO = new PedidoInternoArquivosDTO();
                        pedidoInternoArquivosDTO.Id = 0;
                        pedidoInternoArquivosDTO.DataCadastro = DateTime.Now;
                        pedidoInternoArquivosDTO.Extensao = extensao;
                        pedidoInternoArquivosDTO.IdPedidoInterno = idPedidoInterno;
                        pedidoInternoArquivosDTO.IdUsuarioCadastro = IdUsuario;
                        pedidoInternoArquivosDTO.Nome = arquivo.FileName;
                        pedidoInternoArquivosDTO.NomeLogico = $"{Guid.NewGuid()}{extensao.ToLower()}";
                        pedidoInternoArquivosDTO.TamanhoMB = arquivo.Length / 1024.00 / 1024.00;
                        await _servicoPedidoInterno.Upload(pedidoInternoArquivosDTO);

                        await arquivo.CopyToAsync(memoryStream);
                        await System.IO.File.WriteAllBytesAsync($"{_configuration["DiretorioUpload"]}\\{pedidoInternoArquivosDTO.NomeLogico}", memoryStream.ToArray());

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
                var arquivoDTO = await _servicoPedidoInterno.GetFile(idArquivo);

                if (arquivoDTO == null)
                    throw new Exception("Arquivo não encontrado");

                System.IO.File.Delete($"{_configuration["DiretorioUpload"]}\\{arquivoDTO.NomeLogico}");

                await _servicoPedidoInterno.DeleteArquivo(idArquivo);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Arquivos/{idPedidoInterno}")]
        public async Task<List<PedidoInternoArquivosDTO>> ObtemArquivos(Int64 idPedidoInterno)
        {
            return await _servicoPedidoInterno.ObtemArquivos(idPedidoInterno);
        }

        [HttpGet("Download/{id}")]
        public IActionResult Download(Int64 id)
        {
            try
            {
                var arquivoDTO = _servicoPedidoInterno.GetFile(id).Result;

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

        [HttpGet("Pdf/Download/{id}")]
        public IActionResult DownloadPdfPedidoCompra([FromRoute] Int64 id)
        {
            try
            {
                var logoPath = $"{_configuration["DiretorioUpload"]}\\whLogoColorido.png";

                return File(
                    _servicoPedidoInterno.GeraPdfPedidoCompra(id, logoPath).Result,
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
