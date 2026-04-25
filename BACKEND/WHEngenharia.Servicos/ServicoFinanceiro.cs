using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos;
using WHEngenharia.SQL;
using WHEngenharia.SQL.Repositorios;

namespace WHEngenharia.Servicos
{
    public class ServicoFinanceiro
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        private RepositorioFinanceiro _repositorioFinanceiro;

        public ServicoFinanceiro(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _repositorioFinanceiro = new RepositorioFinanceiro(_context, _mapper);
        }

        public async Task<List<Financeiro_EntradaDTO>> ObtemEntradas()
        {
            return await _repositorioFinanceiro.ObtemEntradas();
        }

        public async Task<List<Financeiro_EntradaDTO>> ObtemEntradasRecusadas()
        {
            return await _repositorioFinanceiro.ObtemEntradasRecusadas();
        }

        public async Task AutorizaEntrada(Financeiro_EntradaDTO entradaDTO)
        {
            await _repositorioFinanceiro.AutorizaEntrada(entradaDTO);
        }

        public async Task CancelaEntrada(Financeiro_EntradaDTO entradaDTO, Int64 idUsuario)
        {
            await _repositorioFinanceiro.CancelaEntrada(entradaDTO, idUsuario);
        }
    }
}
