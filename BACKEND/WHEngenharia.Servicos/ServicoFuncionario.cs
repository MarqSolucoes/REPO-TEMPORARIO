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
    public class ServicoFuncionario
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFuncionario _repositorioCargo;

        public ServicoFuncionario(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioCargo = new RepositorioFuncionario(_context, _mapper);
        }

        public async Task<List<FuncionarioDTO>> Get(bool apenasAtivos = false)
        {
            return await _repositorioCargo.Get(apenasAtivos);
        }
    }
}
