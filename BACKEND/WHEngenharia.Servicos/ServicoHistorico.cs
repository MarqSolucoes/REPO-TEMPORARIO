using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Repositorios;
using WHEngenharia.SQL;
using WHEngenharia.Dominio.Modelos.Genericos.Historico;

namespace WHEngenharia.Servicos
{
    public class ServicoHistorico
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioHistorico _repositorioHistorico;

        public ServicoHistorico(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioHistorico = new RepositorioHistorico(_context, _mapper);
        }

        public async Task<List<HistoricoDTO>> Post(Historico_RequestDTO parametrosDTO)
        {
            return await _repositorioHistorico.Post(parametrosDTO);
        }

        public async Task<List<string>> RetornaCodigos(int tipoCodigo)
        {
            return await _repositorioHistorico.RetornaCodigos(tipoCodigo);
        }
    }
}
