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
    public class ObraControleCustoController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoObra _servicoObra;

        public ObraControleCustoController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoObra = new ServicoObra(_context, _mapper);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] List<ObraETODTO> etosDTO)
        {
            try
            {
                var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

                etosDTO.ForEach(x =>
                {
                    x.DataUltimaAlteracao = DateTime.Now;
                    x.IdUsuarioAlteracao = IdUsuario;

                    if (x.Id == 0)
                    {
                        x.DataCadastro = DateTime.Now;
                        x.IdUsuarioCadastro = IdUsuario;
                    }
                });

                await _servicoObra.PostETOs(etosDTO);

                return Ok("Post");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost]
        //public async Task<ActionResult> Post(ObraControleCustoDTO obraControleCustoDTO)
        //{
        //    try
        //    {
        //        var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);
        //        obraControleCustoDTO.IdUsuarioCadastro = IdUsuario;
        //        obraControleCustoDTO.IdUsuarioAlteracao = IdUsuario;

        //        var result = await _servicoObra.Post(obraControleCustoDTO);

        //        return CreatedAtAction("Post", result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpPut]
        //public async Task<ActionResult<ObraControleCustoDTO>> Put([FromBody] ObraControleCustoDTO obraControleCustoDTO)
        //{
        //    try
        //    {
        //        var IdUsuario = Convert.ToInt64((User.Identity as ClaimsIdentity).FindFirst("Id").Value);

        //        obraControleCustoDTO.IdUsuarioAlteracao = IdUsuario;
        //        obraControleCustoDTO.DataUltimaAlteracao = DateTime.Now;

        //        var result = await _servicoObra.Put(obraControleCustoDTO);

        //        return CreatedAtAction("Put", result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        //[HttpDelete("{Id}")]
        //public async Task<ActionResult> Delete([FromRoute] Int64 Id)
        //{
        //    try
        //    {
        //        await _servicoObra.DeleteControleCusto(Id);

        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
