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

namespace WHEngenharia.API.Obra.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ObraMedicaoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoObra _servicoObra;

        public ObraMedicaoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<ObraFaturamentoDTO> medicoesDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                medicoesDTO.ForEach(x =>
                {
                    x.DataUltimaAlteracao = DateTime.Now;
                    x.IdUsuarioAlteracao = IdUsuario;

                    if (x.Id == 0)
                    {
                        x.DataCadastro = DateTime.Now;
                        x.IdUsuarioCadastro = IdUsuario;
                    }
                });

                await _servicoObra.PostMedicoes(medicoesDTO);

                return Ok("Post");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
