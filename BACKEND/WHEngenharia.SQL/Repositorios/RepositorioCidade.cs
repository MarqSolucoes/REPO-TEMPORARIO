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
    public class RepositorioCidade
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioCidade(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CidadeDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<CidadeDTO>>(await _context.Cidade.Where(x => x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<CidadeDTO>>(await _context.Cidade.ToListAsync());
        }

        public async Task<CidadeDTO> Get(Int64 Id)
        {
            return _mapper.Map<CidadeDTO>(await _context.Cidade.FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<CidadeDTO> Put(CidadeDTO cidadeDTO)
        {
            var cidade = await _context.Cidade.FirstOrDefaultAsync(x => x.Id == cidadeDTO.Id);

            if (cidade == null)
                throw new Exception("Cidade não encontrada");

            cidade.AliquotaImpostoISS = cidadeDTO.AliquotaImpostoISS;
            cidade.Ativo = cidadeDTO.Ativo;

            _context.Entry(cidade).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<CidadeDTO>(cidade);
        }
    }
}
