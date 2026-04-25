using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_PedidoCompraDTO
    {
        public Int64 Id { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public DateTime DataVencimento { get; set; } = DateTime.Now;
        public double ValorTotalPedido { get; set; }
        public double SaldoDisponivel { get; set; }
        public double SaldoCancelado { get; set; }
        public double ValorGasto { get; set; }
        public double ValorPendenteAprovacao { get; set; }
        public double ValorPrevistoGasto { get; set; }
        public double ValorFrete { get; set; }
        public double ValorImposto { get; set; }
        public Int64 IdFornecedorEscolhido { get; set; }
        
        public List<Conciliacao_DataValorPagamentoDTO> DataValorPagamento { get; set; }
        public List<PedidoCompraFaturaDTO> Faturas { get; set; }

        public FornecedorDTO Fornecedor { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public List<Conciliacao_PedidoCompraMateriaisDTO> Materiais { get; set; }
        public List<Conciliacao_PedidoCompraNotaFiscalDTO> NotasFiscais { get; set; } 
        public List<Conciliacao_PedidoCompraCancelamentoSaldoDTO> CancelamentosSaldo { get; set; }
    }
}
