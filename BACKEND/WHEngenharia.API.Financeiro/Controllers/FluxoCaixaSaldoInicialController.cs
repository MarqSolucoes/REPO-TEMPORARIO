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

namespace WHEngenharia.API.Financeiro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FluxoCaixaSaldoInicialController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoFluxoCaixaSaldoInicial _servicoFluxoCaixaSaldoInicial;

        public FluxoCaixaSaldoInicialController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoFluxoCaixaSaldoInicial = new ServicoFluxoCaixaSaldoInicial(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<ActionResult>>> Get()
        {
            return Ok(await _servicoFluxoCaixaSaldoInicial.Get());
        }

        [HttpPut]
        public async Task<ActionResult> Put(FluxoCaixaSaldoInicialDTO fluxoCaixaSaldoInicialDTO)
        {
            return Ok(await _servicoFluxoCaixaSaldoInicial.Put(fluxoCaixaSaldoInicialDTO));
        }
    }
}
