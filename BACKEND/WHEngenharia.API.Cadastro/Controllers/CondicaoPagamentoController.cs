using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Cadastro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CondicaoPagamentoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoCondicaoPagamento _servicoCondicaoPagamento;

        public CondicaoPagamentoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoCondicaoPagamento = new ServicoCondicaoPagamento(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<CondicaoPagamentoDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoCondicaoPagamento.Get(apenasAtivos));
        }
    }
}
