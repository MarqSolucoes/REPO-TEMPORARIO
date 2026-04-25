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
    public class RepositorioStatusSolicitacaoCompra
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioStatusSolicitacaoCompra(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<StatusSolicitacaoCompraDTO>> Get()
        {
                return _mapper.Map<List<StatusSolicitacaoCompraDTO>>(await _context.StatusSolicitacaoCompra.AsNoTracking().OrderBy(x => x.Descricao).ToListAsync());
        }
    }
}
