using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoPedidoInternoRecorrente
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioPedidoInternoRecorrente _repositorioPedidoInternoRecorrente;

        public ServicoPedidoInternoRecorrente(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioPedidoInternoRecorrente = new RepositorioPedidoInternoRecorrente(_context, _mapper);
        }

        public async Task<List<PedidoInternoRecorrenteDTO>> Get()
        {
            return await _repositorioPedidoInternoRecorrente.Get();
        }

        public async Task<PedidoInternoRecorrenteDTO> Get(Int64 Id)
        {
            return await _repositorioPedidoInternoRecorrente.Get(Id);
        }

        public async Task<PedidoInternoRecorrenteDTO> Post(PedidoInternoRecorrenteDTO pedidoInternoRecorrenteDTO)
        {
            if (string.IsNullOrEmpty(pedidoInternoRecorrenteDTO.Descricao.Trim()))
                throw new Exception("O campo'Descrição' deve ser informado");

            if (pedidoInternoRecorrenteDTO.Valor <= 0)
                throw new Exception($"Valor incorreto");

            return await _repositorioPedidoInternoRecorrente.Post(pedidoInternoRecorrenteDTO);
        }

        public async Task<PedidoInternoRecorrenteDTO> Put(PedidoInternoRecorrenteDTO PedidoInternoRecorrenteDTO)
        {
            if (string.IsNullOrEmpty(PedidoInternoRecorrenteDTO.Descricao.Trim()))
                throw new Exception("O campo'Descrição' deve ser informado");

            if (PedidoInternoRecorrenteDTO.Valor <= 0)
                throw new Exception($"Valor incorreto");

            return await _repositorioPedidoInternoRecorrente.Put(PedidoInternoRecorrenteDTO);
        }

        public async Task GeraPedidosInternos()
        {
            await _repositorioPedidoInternoRecorrente.GeraPedidosInternos();
        }
    }
}
