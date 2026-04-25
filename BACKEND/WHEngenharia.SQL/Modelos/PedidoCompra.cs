using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompra
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdStatusPedidoCompra { get; set; }
        public Int64 IdFornecedor { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public Int32 CodigoSequencia { get; set; }
        public Int32 CodigoAno { get; set; }
        public string Codigo { get; set; }
        public double ValorTotal { get; set; }
        public double ValorDesconto { get; set; }
        public string MotivoCancelamento { get; set; }
        public DateTime DataEntrega { get; set; }
        public bool? ImportadoParaFinanceiro { get; set; }
        public string EnderecoEntrega { get; set; }
        public double Frete { get; set; }
        public double Imposto { get; set; }
        public double Saldo { get; set; }
        public string ObservacaoParaFornecedor { get; set; }
        public Int64 IdUsuarioComprador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        [ForeignKey("IdStatusPedidoCompra")]
        public virtual StatusPedidoCompra StatusPedidoCompra { get; set; }

        [ForeignKey("IdSolicitacaoCompra")]
        public virtual SolicitacaoCompra SolicitacaoCompra { get; set; }

        [ForeignKey("IdFornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("IdTipoCentroCusto")]
        public virtual TipoCentroCusto TipoCentroCusto { get; set; }

        [ForeignKey("IdCentroCustoObra")]
        public virtual Obra CentroCustoObra { get; set; }

        [ForeignKey("IdCentroCustoDEF")]
        public virtual DEF CentroCustoDEF { get; set; }

        [ForeignKey("IdCondicaoPagamento")]
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }

        [ForeignKey("IdUsuarioComprador")]
        public virtual Usuario UsuarioComprador { get; set; }

        public List<PedidoCompraFatura> Faturas { get; set; }
        public List<PedidoCompraMateriais> Materiais { get; set; }
        public List<PedidoCompraArquivos> Arquivos { get; set; }
        public List<PedidoCompraNotaFiscal> NotasFiscais { get; set; }
        public List<PedidoCompraDevolucaoSaldo> CancelamentosSaldo { get; set; }
    }
}
