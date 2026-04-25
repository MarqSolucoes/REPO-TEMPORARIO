using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioPedidoCompraNotaFiscal
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioPedidoCompraNotaFiscal(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PedidoCompraNotaFiscalDTO> Get(Int64 id)
        {
            return _mapper.Map<PedidoCompraNotaFiscalDTO>(await _context.PedidoCompra_NotaFiscal.AsNoTracking().Include(x=>x.Arquivo).FirstOrDefaultAsync(x => x.Id == id));
        }

        public async Task ApagaArquivo(Int64 idPedidoCompraNotaArquivo, string diretorio)
        {
            var arquivo = await _context.PedidoCompra_Arquivos.FirstOrDefaultAsync(x => x.Id == idPedidoCompraNotaArquivo);

            File.Delete($"{diretorio}\\{arquivo.NomeLogico}");

            _context.PedidoCompra_Arquivos.Remove(arquivo);
            await _context.SaveChangesAsync();
        }

        public async Task AtualizaArquivoNotaFiscal(Int64 idPedidoCompraArquivoAntigo, Int64 idPedidoCompraArquivo, string nomeArquivo)
        {
            await _context.Database.ExecuteSqlRawAsync($"UPDATE PedidoCompra_NotaFiscal SET IdPedidoCompraArquivo = {idPedidoCompraArquivo}, Nome = '{nomeArquivo}' WHERE IdPedidoCompraArquivo = {idPedidoCompraArquivoAntigo}");
        }
    }
}
