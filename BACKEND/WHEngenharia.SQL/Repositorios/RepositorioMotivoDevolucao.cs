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
    public class RepositorioMotivoDevolucao
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioMotivoDevolucao(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<MotivoDevolucaoSaldoDTO>> Get(bool apenasAtivos = false)
        {
            if (apenasAtivos)
                return _mapper.Map<List<MotivoDevolucaoSaldoDTO>>(await _context.MotivoDevolucaoSaldo.AsNoTracking().Where(x => x.Ativo).ToListAsync());
            else
                return _mapper.Map<List<MotivoDevolucaoSaldoDTO>>(await _context.MotivoDevolucaoSaldo.AsNoTracking().ToListAsync());
        }

        public async Task<MotivoDevolucaoSaldoDTO> Get(Int64 Id)
        {
            return _mapper.Map<MotivoDevolucaoSaldoDTO>(await _context.MotivoDevolucaoSaldo.AsNoTracking().FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<MotivoDevolucaoSaldoDTO> Post(MotivoDevolucaoSaldoDTO motivoDevolucaoSaldoDTO)
        {
            var motivoDevolucaoSaldo = new MotivoDevolucaoSaldo();
            motivoDevolucaoSaldo.Ativo = motivoDevolucaoSaldoDTO.Ativo;
            motivoDevolucaoSaldo.DataCadastro = motivoDevolucaoSaldoDTO.DataCadastro;
            motivoDevolucaoSaldo.DataUltimaAlteracao = motivoDevolucaoSaldoDTO.DataUltimaAlteracao;
            motivoDevolucaoSaldo.Descricao = motivoDevolucaoSaldoDTO.Descricao;
            motivoDevolucaoSaldo.Id = 0;
            motivoDevolucaoSaldo.IdUsuarioAlteracao = motivoDevolucaoSaldoDTO.IdUsuarioAlteracao;
            motivoDevolucaoSaldo.IdUsuarioCadastro = motivoDevolucaoSaldoDTO.IdUsuarioCadastro;

            await _context.MotivoDevolucaoSaldo.AddAsync(motivoDevolucaoSaldo);
            await _context.SaveChangesAsync();

            return _mapper.Map<MotivoDevolucaoSaldoDTO>(motivoDevolucaoSaldo);
        }

        public async Task<MotivoDevolucaoSaldoDTO> Put(MotivoDevolucaoSaldoDTO motivoDevolucaoSaldoDTO)
        {
            var motivoDevolucaoSaldo = await _context.MotivoDevolucaoSaldo.FirstOrDefaultAsync(x => x.Id == motivoDevolucaoSaldoDTO.Id);

            if (motivoDevolucaoSaldo == null)
                throw new Exception("Motivo de devolução de saldo não encontrado");

            motivoDevolucaoSaldo.Ativo = motivoDevolucaoSaldoDTO.Ativo;
            motivoDevolucaoSaldo.Descricao = motivoDevolucaoSaldoDTO.Descricao;
            motivoDevolucaoSaldo.DataUltimaAlteracao = motivoDevolucaoSaldoDTO.DataUltimaAlteracao;
            motivoDevolucaoSaldo.IdUsuarioAlteracao = motivoDevolucaoSaldoDTO.IdUsuarioAlteracao;

            _context.Entry(motivoDevolucaoSaldo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<MotivoDevolucaoSaldoDTO>(motivoDevolucaoSaldo);
        }

        public async Task<bool> VerificaDescricaoExistente(string descricao, Int64? id = null)
        {
            if (id.HasValue)
                return await _context.MotivoDevolucaoSaldo.AnyAsync(x => x.Descricao == descricao && x.Id != id);
            else
                return await _context.MotivoDevolucaoSaldo.AnyAsync(x => x.Descricao == descricao);
        }
    }
}
