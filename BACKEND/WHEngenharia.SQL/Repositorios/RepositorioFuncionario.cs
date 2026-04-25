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
    public class RepositorioFuncionario
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioFuncionario(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FuncionarioDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<FuncionarioDTO>>(await _context.Funcionario.Where(x => x.Ativo).OrderBy(x => x.Nome).ToListAsync());
            else
                return _mapper.Map<List<FuncionarioDTO>>(await _context.Funcionario.OrderBy(x => x.Nome).ToListAsync());
        }
    }
}
