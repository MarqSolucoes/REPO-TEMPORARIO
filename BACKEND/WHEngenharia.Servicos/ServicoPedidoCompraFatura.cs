using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoPedidoCompraFatura
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioPedidoCompraFatura _repositorioPedidoCompraFatura;

        public ServicoPedidoCompraFatura(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioPedidoCompraFatura = new RepositorioPedidoCompraFatura(_context, _mapper);
        }

        public async Task<PedidoCompraFaturaDTO> Post(PedidoCompraFaturaDTO pedidoCompraFaturaDTO)
        {
            return await _repositorioPedidoCompraFatura.Post(pedidoCompraFaturaDTO);
        }
    }
}
