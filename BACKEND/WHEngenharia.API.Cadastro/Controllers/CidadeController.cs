using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Cadastro.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoCidade _servicoCidade;

        public CidadeController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoCidade = new ServicoCidade(_context, _mapper);
        }

        [HttpGet]
        public async Task<ActionResult<List<CidadeDTO>>> Get(bool apenasAtivos = false)
        {
            return Ok(await _servicoCidade.Get(apenasAtivos));
        }

        [HttpPost("AtivarDesativar")]
        public async Task<ActionResult> AtivarDesativar(Int64 id, bool ativarDesativar)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var obraDTO = await _servicoCidade.Get(id);
                obraDTO.Ativo = ativarDesativar;

                var result = await _servicoCidade.Put(obraDTO);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<CidadeDTO>> Put([FromBody] CidadeDTO cidadeDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                var result = await _servicoCidade.Put(cidadeDTO);

                return CreatedAtAction("Put", result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
