using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;
using WHEngenharia.Dominio.Modelos.Genericos.NotaFiscal;

namespace WHEngenharia.SQL.Repositorios
{
    public class RepositorioNotaFiscal
    {
        private IMapper _mapper;
        private WHEngenhariaContext _context;

        public RepositorioNotaFiscal(WHEngenhariaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NotaFiscal_ItemFiltroDTO>> GetItemFiltro(string item)
        {
            if (item == "Obra")
                return await _context.Obra.Select(x => new NotaFiscal_ItemFiltroDTO() { Id = x.Id, Descricao = x.Codigo }).ToListAsync();
            else if (item == "Pedido")
                return await _context.PedidoCompra.Select(x => new NotaFiscal_ItemFiltroDTO() { Id = x.Id, Descricao = x.Codigo }).ToListAsync();
            else if (item == "Fornecedor")
                return await _context.Fornecedor.Select(x => new NotaFiscal_ItemFiltroDTO() { Id = x.Id, Descricao = x.NomeFantasia }).OrderBy(x => x.Descricao).ToListAsync();
            else
                return new List<NotaFiscal_ItemFiltroDTO>();
        }

        public async Task<List<NotaFiscal_ItemDTO>> Get(NotaFiscal_ItemFiltroRequestDTO notaFiscalItemFiltroRequestDTO)
        {
            var query = _context.PedidoCompra_NotaFiscal.AsQueryable();

            query = query.Where(x => x.ImportadoParaFinanceiro == true);

            if (notaFiscalItemFiltroRequestDTO.idFornecedor.HasValue)
                query = query.Where(x => x.PedidoCompra.IdFornecedor == notaFiscalItemFiltroRequestDTO.idFornecedor);

            if (notaFiscalItemFiltroRequestDTO.idObra.HasValue)
                query = query.Where(x => x.PedidoCompra.IdCentroCustoObra == notaFiscalItemFiltroRequestDTO.idObra);

            if (notaFiscalItemFiltroRequestDTO.idPedido.HasValue)
                query = query.Where(x => x.IdPedidoCompra == notaFiscalItemFiltroRequestDTO.idPedido);

            if (notaFiscalItemFiltroRequestDTO.dataPagamentoInicial.HasValue)
                query = query.Where(x => x.DataVencimento.Date >= notaFiscalItemFiltroRequestDTO.dataPagamentoInicial.Value.Date);

            if (notaFiscalItemFiltroRequestDTO.dataPagamentoFinal.HasValue)
            {
                notaFiscalItemFiltroRequestDTO.dataPagamentoFinal = notaFiscalItemFiltroRequestDTO.dataPagamentoFinal.Value.AddHours(23).AddMinutes(59).AddSeconds(59);
                query = query.Where(x => x.DataVencimento <= notaFiscalItemFiltroRequestDTO.dataPagamentoFinal);
            }

            if (notaFiscalItemFiltroRequestDTO.idPagamento.HasValue)
            {
                if (notaFiscalItemFiltroRequestDTO.idPagamento.Value == 1)
                    query = query.Where(x => x.PagamentoEfetuado);
                else if (notaFiscalItemFiltroRequestDTO.idPagamento.Value == 2)
                    query = query.Where(x => !x.PagamentoEfetuado);
            }

            if (notaFiscalItemFiltroRequestDTO.idStatus.HasValue)
            {
                if (notaFiscalItemFiltroRequestDTO.idStatus.Value == 1)
                    query = query.Where(x => !x.Aprovada.HasValue);
                else if (notaFiscalItemFiltroRequestDTO.idStatus.Value == 2)
                    query = query.Where(x => x.Aprovada == true);
                else if (notaFiscalItemFiltroRequestDTO.idStatus.Value == 3)
                    query = query.Where(x => x.Aprovada == false);
            }

            if (!string.IsNullOrEmpty(notaFiscalItemFiltroRequestDTO.numeroNotaFiscal))
                query = query.Where(x => x.NumeroNotaFiscal.Contains(notaFiscalItemFiltroRequestDTO.numeroNotaFiscal));

            return await query.Select(x => new NotaFiscal_ItemDTO()
            {
                Id = x.Id,
                IdArquivo = x.IdPedidoCompraArquivo,
                Nome = _context.PedidoCompra_Arquivos.FirstOrDefault(y => y.Id == x.IdPedidoCompraArquivo).Nome,
                Fornecedor = x.PedidoCompra.Fornecedor.NomeFantasia,
                NumeroNota = x.NumeroNotaFiscal,
                Obra = x.PedidoCompra.CentroCustoObra.Codigo,
                Pagamento = x.PagamentoEfetuado ? "Sim" : "Não",
                Pedido = x.PedidoCompra.Codigo,
                Status = x.Aprovada.HasValue ? x.Aprovada.Value ? "Aprovada" : "Reprovada" : "Pendente",
                Valor = x.Valor,
                DataVencimento = x.DataVencimento
            }).ToListAsync();
        }

        public async Task EditarDataVencimento(List<PedidoCompraNotaFiscalDTO> pedidoCompraNotasFiscaisDTO)
        {
            pedidoCompraNotasFiscaisDTO.ForEach(pedidoCompraNotaFiscalDTO =>
            {
                _context.Database.ExecuteSqlRaw($"UPDATE PedidoCompra_NotaFiscal SET DataVencimento = '{pedidoCompraNotaFiscalDTO.DataVencimento.ToString("MM-dd-yyyy")} 00:00:00' WHERE Id = {pedidoCompraNotaFiscalDTO.Id}");
            });
        }
    }
}
