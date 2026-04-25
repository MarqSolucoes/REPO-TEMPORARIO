using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.NotaFiscal;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoNotaFiscal
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioNotaFiscal _repositorioNotaFiscal;

        public ServicoNotaFiscal(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioNotaFiscal = new RepositorioNotaFiscal(_context, _mapper);
        }

        public async Task<List<NotaFiscal_ItemFiltroDTO>> GetItemFiltro(string item)
        {
            return await _repositorioNotaFiscal.GetItemFiltro(item);
        }

        public async Task<List<NotaFiscal_ItemDTO>> Get(NotaFiscal_ItemFiltroRequestDTO notaFiscalItemFiltroRequestDTO)
        {
            return await _repositorioNotaFiscal.Get(notaFiscalItemFiltroRequestDTO);
        }

        public async Task EditarDataVencimento(List<PedidoCompraNotaFiscalDTO> pedidoCompraNotasFiscaisDTO)
        {
            await _repositorioNotaFiscal.EditarDataVencimento(pedidoCompraNotasFiscaisDTO);
        }
    }
}
