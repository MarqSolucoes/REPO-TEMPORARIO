using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.NotaFiscal;
using WHEngenharia.Servicos;
using WHEngenharia.SQL;

namespace WHEngenharia.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NotaFiscalController : ControllerBase
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;
        private ServicoNotaFiscal _servicoNotaFiscal;

        public NotaFiscalController(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _servicoNotaFiscal = new ServicoNotaFiscal(_context, _mapper);
        }

        [HttpGet("ItemFiltro")]
        public async Task<ActionResult<List<NotaFiscal_ItemFiltroDTO>>> GetItemFiltro(string item)
        {
            try
            {
                return Ok(await _servicoNotaFiscal.GetItemFiltro(item));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("ObtemNotasFiltradas")]
        public async Task<ActionResult<List<NotaFiscal_ItemDTO>>> ObtemNotasFiltradas(NotaFiscal_ItemFiltroRequestDTO notaFiscalItemFiltroRequestDTO)
        {
            try
            {
                return Ok(await _servicoNotaFiscal.Get(notaFiscalItemFiltroRequestDTO));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("EditarDataVencimento")]
        public async Task<ActionResult> EditarDataVencimento([FromBody]List<PedidoCompraNotaFiscalDTO> pedidoCompraNotasFiscaisDTO)
        {
            try
            {
                await _servicoNotaFiscal.EditarDataVencimento(pedidoCompraNotasFiscaisDTO);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
