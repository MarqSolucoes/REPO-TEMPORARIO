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
    public class RepositorioFluxoCaixaSaldoInicial
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioFluxoCaixaSaldoInicial(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<FluxoCaixaSaldoInicialDTO>> Get()
        {
            try
            {
                return _mapper.Map<List<FluxoCaixaSaldoInicialDTO>>(await _context.FluxoCaixaSaldoInicial.ToListAsync());
            }
            catch(Exception ex)
            {
                return new List<FluxoCaixaSaldoInicialDTO>();
            }
        }

        public async Task<FluxoCaixaSaldoInicialDTO> Put(FluxoCaixaSaldoInicialDTO fluxoCaixaSaldoInicialDTO)
        {
            var saldoInicial = await _context.FluxoCaixaSaldoInicial.FirstOrDefaultAsync(x => x.Id == fluxoCaixaSaldoInicialDTO.Id);

            saldoInicial.Saldo = fluxoCaixaSaldoInicialDTO.Saldo;
            saldoInicial.SaldoCalculado = fluxoCaixaSaldoInicialDTO.SaldoCalculado;

            _context.Entry(saldoInicial).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return _mapper.Map<FluxoCaixaSaldoInicialDTO>(saldoInicial);
        }
    }
}
