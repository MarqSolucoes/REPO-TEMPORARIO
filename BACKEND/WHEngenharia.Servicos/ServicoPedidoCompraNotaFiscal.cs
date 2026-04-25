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
    public class ServicoPedidoCompraNotaFiscal
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioPedidoCompraNotaFiscal _repositorioPedidoCompraNotaFiscal;

        public ServicoPedidoCompraNotaFiscal(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioPedidoCompraNotaFiscal = new RepositorioPedidoCompraNotaFiscal(_context, _mapper);
        }

        public async Task<PedidoCompraNotaFiscalDTO> Get(Int64 id)
        {
            return await _repositorioPedidoCompraNotaFiscal.Get(id);
        }

        public async Task ApagaArquivo(Int64 idPedidoCompraNotaArquivo, string diretorio)
        {
            await _repositorioPedidoCompraNotaFiscal.ApagaArquivo(idPedidoCompraNotaArquivo, diretorio);
        }

        public async Task AtualizaArquivoNotaFiscal(Int64 idPedidoCompraArquivoAntigo, Int64 idPedidoCompraArquivo, string nomeArquivo)
        {
            await _repositorioPedidoCompraNotaFiscal.AtualizaArquivoNotaFiscal(idPedidoCompraArquivoAntigo, idPedidoCompraArquivo, nomeArquivo);
        }
    }
}
