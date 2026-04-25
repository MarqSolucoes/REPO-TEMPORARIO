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
    public class ServicoCidade
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioCidade _repositorioCidade;

        public ServicoCidade(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCidade = new RepositorioCidade(_context, _mapper);
        }

        public async Task<List<CidadeDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCidade.Get(apenasAtivos);
        }

        public async Task<CidadeDTO> Get(Int64 Id)
        {
            return await _repositorioCidade.Get(Id);
        }

        public async Task<CidadeDTO> Put(CidadeDTO cidadeDTO)
        {
            return await _repositorioCidade.Put(cidadeDTO);
        }
    }
}
