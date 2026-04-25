using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioPedidoCompraFatura
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioPedidoCompraFatura(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PedidoCompraFaturaDTO> Post(PedidoCompraFaturaDTO pedidoCompraFaturaDTO)
        {
            var pedidoCompraFatura = new PedidoCompraFatura();
            pedidoCompraFatura.DataFatura = pedidoCompraFaturaDTO.DataFatura;
            pedidoCompraFatura.Id = 0;
            pedidoCompraFatura.IdPedidoCompra = pedidoCompraFaturaDTO.IdPedidoCompra;
            pedidoCompraFatura.PagamentoEfetuado = pedidoCompraFaturaDTO.PagamentoEfetuado;
            pedidoCompraFatura.Valor = 0;

            await _context.PedidoCompra_Faturas.AddAsync(pedidoCompraFatura);
            await _context.SaveChangesAsync();

            return _mapper.Map<PedidoCompraFaturaDTO>(pedidoCompraFatura);
        }
    }
}
