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
    public class ServicoCondicaoPagamento
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioCondicaoPagamento _repositorioCondicaoPagamento;

        public ServicoCondicaoPagamento(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCondicaoPagamento = new RepositorioCondicaoPagamento(_context, _mapper);
        }

        public async Task<List<CondicaoPagamentoDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCondicaoPagamento.Get(apenasAtivos);
        }
    }
}
