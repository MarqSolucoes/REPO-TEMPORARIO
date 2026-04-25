using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioCondicaoPagamento
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioCondicaoPagamento(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CondicaoPagamentoDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<CondicaoPagamentoDTO>>(await _context.CondicaoPagamento.Where(x => x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<CondicaoPagamentoDTO>>(await _context.CondicaoPagamento.ToListAsync());
        }
    }
}
