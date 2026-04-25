using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.Historico;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioHistorico
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioHistorico(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<HistoricoDTO>> Post(Historico_RequestDTO parametrosDTO)
        {
            var result = new List<HistoricoDTO>();

            if (parametrosDTO.SolicitacaoCompra)
            {
                var solicitacoesCompra = await _context.SolicitacaoCompra.Where(x => x.Codigo.Contains(parametrosDTO.Codigo)).Select(x => new { x.Id, x.Codigo}).ToListAsync();

                result = _mapper.Map<List<HistoricoDTO>>(await _context.Historico.Where(x => solicitacoesCompra.Select(y => y.Id).Contains(x.IdSolicitacaoCompra.Value)).OrderBy(x => x.Data).ToListAsync());
                result.ForEach(x =>
                {
                    x.ValorSolicitacaoOuPedidoOuPI = solicitacoesCompra.FirstOrDefault(y => y.Id == x.IdSolicitacaoCompra).Codigo;
                });
            }
            else if(parametrosDTO.PedidoCompra)
            {
                var pedidosCompra = await _context.PedidoCompra.Where(x => x.Codigo.Contains(parametrosDTO.Codigo)).Select(x => new { x.Id, x.Codigo }).ToListAsync();

                result = _mapper.Map<List<HistoricoDTO>>(await _context.Historico.Where(x => pedidosCompra.Select(y => y.Id).Contains(x.IdPedidoCompra.Value)).OrderBy(x => x.Data).ToListAsync());
                result.ForEach(x =>
                {
                    x.ValorSolicitacaoOuPedidoOuPI = pedidosCompra.FirstOrDefault(y => y.Id == x.IdPedidoCompra).Codigo;
                });
            }
            else if(parametrosDTO.PedidoInterno)
            {
                var pedidosInternos = await _context.PedidoInterno.Where(x => x.CodigoFormatado.Contains(parametrosDTO.Codigo)).Select(x => new { x.Id, x.CodigoFormatado }).ToListAsync();

                result = _mapper.Map<List<HistoricoDTO>>(await _context.Historico.Where(x => pedidosInternos.Select(y=>y.Id).Contains(x.IdPedidoInterno.Value)).OrderBy(x => x.Data).ToListAsync());
                result.ForEach(x =>
                {
                    x.ValorSolicitacaoOuPedidoOuPI = pedidosInternos.FirstOrDefault(y => y.Id == x.IdPedidoInterno).CodigoFormatado;
                });
            }

            return result;
        }

        public async Task<List<string>> RetornaCodigos(int tipoCodigo)
        {
            switch (tipoCodigo)
            {
                case 1:
                    return _context.SolicitacaoCompra.Select(x => x.Codigo).OrderBy(x => x).ToList();
                    break;
                case 2:
                    return _context.PedidoCompra.Select(x => x.Codigo).OrderBy(x => x).ToList();
                    break;
                case 3:
                    return _context.PedidoInterno.Select(x => x.CodigoFormatado).OrderBy(x => x).ToList();
                    break;
                default:
                    return new List<string>();
                    break;
            }
        }
    }
}
