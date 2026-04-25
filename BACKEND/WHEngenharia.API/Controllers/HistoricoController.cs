using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Historico;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoHistorico _servicoHistorico;

        public HistoricoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoHistorico = new ServicoHistorico(_context, _mapper);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Historico_RequestDTO parametrosDTO)
        {
            return Ok(await _servicoHistorico.Post(parametrosDTO));
        }

        [HttpGet("RetornaCodigos/{tipoCodigo}")]
        public async Task<ActionResult> RetornaCodigos(int tipoCodigo)
        {
            return Ok(await _servicoHistorico.RetornaCodigos(tipoCodigo));
        }
    }
}
