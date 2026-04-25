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
    public class RepositorioSolicitacaoCompraRascunho
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioSolicitacaoCompraRascunho(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<SolicitacaoCompraRascunhoDTO>> Get(Int64 IdUsuario)
        {
            return _mapper.Map<List<SolicitacaoCompraRascunhoDTO>>(await _context.SolicitacaoCompraRascunho.AsNoTracking()
                .Where(x => x.IdUsuarioCadastro == IdUsuario)
                .ToListAsync());
        }

        public async Task Delete(Int64 Id)
        {
            await _context.Database.ExecuteSqlRawAsync($"DELETE FROM SolicitacaoCompra_Rascunho WHERE Id = {Id}");
        }

        public async Task Post(SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            var solicitacaoCompraRascunho = new SolicitacaoCompraRascunho();
            solicitacaoCompraRascunho.Id = 0;
            solicitacaoCompraRascunho.Titulo = solicitacaoCompraRascunhoDTO.Titulo;
            solicitacaoCompraRascunho.DataCadastro = solicitacaoCompraRascunhoDTO.DataCadastro;
            solicitacaoCompraRascunho.IdUsuarioCadastro = solicitacaoCompraRascunhoDTO.IdUsuarioCadastro;
            solicitacaoCompraRascunho.ObjetoSerializado = solicitacaoCompraRascunhoDTO.ObjetoSerializado;

            await _context.SolicitacaoCompraRascunho.AddAsync(solicitacaoCompraRascunho);
            await _context.SaveChangesAsync();
        }

        public async Task Put(SolicitacaoCompraRascunhoDTO solicitacaoCompraRascunhoDTO)
        {
            var solicitacaoCompraRascunho = await _context.SolicitacaoCompraRascunho.FirstOrDefaultAsync(x => x.Id == solicitacaoCompraRascunhoDTO.Id);

            solicitacaoCompraRascunho.Titulo = solicitacaoCompraRascunhoDTO.Titulo;
            solicitacaoCompraRascunho.ObjetoSerializado = solicitacaoCompraRascunhoDTO.ObjetoSerializado;

            _context.Entry(solicitacaoCompraRascunho).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
