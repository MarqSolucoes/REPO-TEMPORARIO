using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioCategoriaMaterial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioCategoriaMaterial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CategoriaMaterialDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<CategoriaMaterialDTO>>(await _context.CategoriaMaterial.AsNoTracking().Where(x => x.Ativo).OrderBy(x => x.Descricao).ToListAsync());
            else
                return _mapper.Map<List<CategoriaMaterialDTO>>(await _context.CategoriaMaterial.AsNoTracking().OrderBy(x => x.Descricao).ToListAsync());
        }

        public async Task<CategoriaMaterialDTO> Get(Int64 Id)
        {
            return _mapper.Map<CategoriaMaterialDTO>(await _context.CategoriaMaterial.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<CategoriaMaterialDTO> Post(CategoriaMaterialDTO unidadeMaterialDTO)
        {
            var categoriaMaterial = new CategoriaMaterial();
            categoriaMaterial.Ativo = unidadeMaterialDTO.Ativo;
            categoriaMaterial.DataCadastro = unidadeMaterialDTO.DataCadastro;
            categoriaMaterial.DataUltimaAlteracao = unidadeMaterialDTO.DataUltimaAlteracao;
            categoriaMaterial.Descricao = unidadeMaterialDTO.Descricao;
            categoriaMaterial.Id = 0;
            categoriaMaterial.IdUsuarioAlteracao = unidadeMaterialDTO.IdUsuarioAlteracao;
            categoriaMaterial.IdUsuarioCadastro = unidadeMaterialDTO.IdUsuarioCadastro;

            await _context.CategoriaMaterial.AddAsync(categoriaMaterial);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoriaMaterialDTO>(categoriaMaterial);
        }

        public async Task<CategoriaMaterialDTO> Put(CategoriaMaterialDTO categoriaMaterialDTO)
        {
            var categoriaMaterial = await _context.CategoriaMaterial.FirstOrDefaultAsync(x => x.Id == categoriaMaterialDTO.Id);

            if (categoriaMaterial == null)
                throw new Exception("Categoria não encontrada");

            categoriaMaterial.Ativo = categoriaMaterialDTO.Ativo;
            categoriaMaterial.DataUltimaAlteracao = categoriaMaterialDTO.DataUltimaAlteracao;
            categoriaMaterial.IdUsuarioAlteracao = categoriaMaterialDTO.IdUsuarioAlteracao;

            _context.Entry(categoriaMaterial).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoriaMaterialDTO>(categoriaMaterial);
        }

        public async Task<bool> VerificaDescricaoExistente(string descricao, Int64? id = null)
        {
            if (id.HasValue)
                return await _context.CategoriaMaterial.AnyAsync(x => x.Descricao == descricao && x.Id != id);
            else
                return await _context.CategoriaMaterial.AnyAsync(x => x.Descricao == descricao);
        }
    }
}
