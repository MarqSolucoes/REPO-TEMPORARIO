using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioDef
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioDef(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<DEFDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<DEFDTO>>(await _context.DEF.AsNoTracking().Where(x => x.Ativo).OrderBy(x=>x.Codigo).ToListAsync());
            else
                return _mapper.Map<List<DEFDTO>>(await _context.DEF.AsNoTracking().OrderBy(x => x.Codigo).ToListAsync());
        }

        public async Task<DEFDTO> Get(Int64 Id)
        {
            return _mapper.Map<DEFDTO>(await _context.DEF.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<DEFDTO> Post(DEFDTO defDTO)
        {
            var def = new DEF();
            def.Ativo = defDTO.Ativo;
            def.Codigo = defDTO.Codigo;
            def.DataCadastro = DateTime.Now;
            def.DataUltimaAlteracao = DateTime.Now;
            def.Descricao = defDTO.Descricao;
            def.Id = 0;
            def.IdUsuarioAlteracao = defDTO.IdUsuarioAlteracao;
            def.IdUsuarioCadastro = defDTO.IdUsuarioCadastro;

            await _context.DEF.AddAsync(def);
            await _context.SaveChangesAsync();

            return _mapper.Map<DEFDTO>(def);
        }

        public async Task<DEFDTO> Put(DEFDTO defDTO)
        {
            var def = await _context.DEF.FirstOrDefaultAsync(x => x.Id == defDTO.Id);

            if (def == null)
                throw new Exception("DEF não encontrado");

            def.Ativo = defDTO.Ativo;
            def.DataUltimaAlteracao = DateTime.Now;
            def.Descricao = defDTO.Descricao;
            def.IdUsuarioAlteracao = defDTO.IdUsuarioAlteracao;

            _context.Entry(def).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<DEFDTO>(def);
        }
    }
}
