using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using WHEngenharia.Servicos;
using WHEngenharia.SQL.Modelos;
using WHEngenharia.SQL;
using System.Security.Claims;
using WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.API.Compras.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitacaoCompraRascunhoController: ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoSolicitacaoCompraRascunho _servicoSolicitacaoCompraRascunho;
        private ServicoObra _servicoObra;

        public SolicitacaoCompraRascunhoController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoSolicitacaoCompraRascunho = new ServicoSolicitacaoCompraRascunho(_context, _mapper);
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            return Ok(await _servicoSolicitacaoCompraRascunho.Get(IdUsuario));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete(Int64 Id)
        {
            await _servicoSolicitacaoCompraRascunho.Delete(Id);

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

            solicitacaoCompraRascunhoDTO.IdUsuarioCadastro = IdUsuario;
            solicitacaoCompraRascunhoDTO.DataCadastro = DateTime.Now;

            await _servicoSolicitacaoCompraRascunho.Post(solicitacaoCompraRascunhoDTO);

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> Put([FromBody] SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            await _servicoSolicitacaoCompraRascunho.Put(solicitacaoCompraRascunhoDTO);

            return Ok();
        }


    }
}
