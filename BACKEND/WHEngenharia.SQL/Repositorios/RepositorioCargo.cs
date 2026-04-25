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
    public class RepositorioCargo
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioCargo(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CargoDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<CargoDTO>>(await _context.Cargo.Where(x => x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<CargoDTO>>(await _context.Cargo.ToListAsync());
        }

        public async Task<CargoDTO> Get(Int64 Id)
        {
            return _mapper.Map<CargoDTO>(await _context.Cargo.FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<CargoDTO> Post(CargoDTO cargoDTO)
        {
            var cargo = new Cargo();
            cargo.Ativo = cargoDTO.Ativo;
            cargo.DataCadastro = cargoDTO.DataCadastro;
            cargo.DataUltimaAlteracao = cargoDTO.DataUltimaAlteracao;
            cargo.Descricao = cargoDTO.Descricao;
            cargo.Id = 0;
            cargo.IdUsuarioAlteracao = cargoDTO.IdUsuarioAlteracao;
            cargo.IdUsuarioCadastro = cargoDTO.IdUsuarioCadastro;

            await _context.Cargo.AddAsync(cargo);
            await _context.SaveChangesAsync();

            return _mapper.Map<CargoDTO>(cargo);
        }

        public async Task<CargoDTO> Put(CargoDTO cargoDTO)
        {
            var cargo = await _context.Cargo.FirstOrDefaultAsync(x => x.Id == cargoDTO.Id);

            if (cargo == null)
                throw new Exception("Cargo não encontrada");

            cargo.Ativo = cargoDTO.Ativo;
            cargo.DataUltimaAlteracao = cargoDTO.DataUltimaAlteracao;
            cargo.IdUsuarioAlteracao = cargoDTO.IdUsuarioAlteracao;

            _context.Entry(cargo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<CargoDTO>(cargo);
        }

        public async Task<bool> VerificaDescricaoExistente(string descricao, Int64? id = null)
        {
            if (id.HasValue)
                return await _context.Cargo.AnyAsync(x => x.Descricao == descricao && x.Id != id);
            else
                return await _context.Cargo.AnyAsync(x => x.Descricao == descricao);
        }
    }
}
