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
    public class ServicoStatusSolicitacaoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioStatusSolicitacaoCompra _repositorioStatusSolicitacaoCompra;

        public ServicoStatusSolicitacaoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioStatusSolicitacaoCompra = new RepositorioStatusSolicitacaoCompra(_context, _mapper);
        }

        public async Task<List<StatusSolicitacaoCompraDTO>> Get()
        {
            return await _repositorioStatusSolicitacaoCompra.Get();
        }
    }
}
