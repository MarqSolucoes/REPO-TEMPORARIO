using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StatusSolicitacaoCompraController : ControllerBase
    {
        private IMapper _mapper;
        private IConfiguration _configuration;
        private WHEngenhariaContext _context;
        private ServicoStatusSolicitacaoCompra _servicoStatusSolicitacaoCompra;

        public StatusSolicitacaoCompraController(WHEngenhariaContext context, IMapper mapper, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _configuration = configuration;
            _servicoStatusSolicitacaoCompra = new ServicoStatusSolicitacaoCompra(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            return Ok(await _servicoStatusSolicitacaoCompra.Get());
        }
    }
}
