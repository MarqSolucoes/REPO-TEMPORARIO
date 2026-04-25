using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class FuncionarioController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoFuncionario _servicoFuncionario;

        public FuncionarioController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoFuncionario = new ServicoFuncionario(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<FuncionarioDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoFuncionario.Get(apenasAtivos));
        }
    }
}
