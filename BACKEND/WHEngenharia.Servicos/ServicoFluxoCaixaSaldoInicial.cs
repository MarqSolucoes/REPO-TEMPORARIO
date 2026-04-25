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
    public class ServicoFluxoCaixaSaldoInicial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFluxoCaixaSaldoInicial _repositorioFluxoCaixaSaldoInicial;

        public ServicoFluxoCaixaSaldoInicial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFluxoCaixaSaldoInicial = new RepositorioFluxoCaixaSaldoInicial(_context, _mapper);
        }

        public async Task<List<FluxoCaixaSaldoInicialDTO>> Get()
        {
            return await _repositorioFluxoCaixaSaldoInicial.Get();
        }

        public async Task<FluxoCaixaSaldoInicialDTO> Put(FluxoCaixaSaldoInicialDTO fluxoCaixaSaldoInicialDTO)
        {
            return await _repositorioFluxoCaixaSaldoInicial.Put(fluxoCaixaSaldoInicialDTO);
        }
    }
}
