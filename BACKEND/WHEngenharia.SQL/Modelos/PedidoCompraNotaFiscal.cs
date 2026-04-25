using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraNotaFiscal 
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public Int64 IdPedidoCompraArquivo { get; set; }
        public Int64? IdFornecedor { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public bool? Aprovada { get; set; }
        public bool DevolucaoVisualizada { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public double Valor { get; set; }
        public double ValorImposto { get; set; }
        public double ValorFrete { get; set; }
        public DateTime? DataAprovacao { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Cancelada { get; set; }
        public Int64? IdUsuarioAprovacao { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdPedidoCompra")]
        public virtual PedidoCompra PedidoCompra { get; set; }
        
        [ForeignKey("IdPedidoCompraArquivo")]
        public virtual PedidoCompraArquivos Arquivo { get; set; }

        [ForeignKey("IdFornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("IdUsuarioAprovacao")]
        public virtual Usuario UsuarioDiretorAprovador { get; set; }

        public List<PedidoCompraNotaFiscalMateriais> Materiais { get; set; }

        public List<PedidoCompraNotaFiscalPagamento> Pagamentos { get; set; }

        
    }
}
