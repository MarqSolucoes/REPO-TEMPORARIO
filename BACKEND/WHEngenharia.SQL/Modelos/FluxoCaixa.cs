using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class FluxoCaixa
    {
        public Int64 Id { get; set; }
        public Int64 IdTipoFluxoCaixa { get; set; }
        public Int64? IdFluxoCaixaPai { get; set; }
        public bool Cancelado { get; set; }
        public Int64? IdObra { get; set; }
        public string CodigoObra { get; set; }
        public Int64 IdDef { get; set; }
        public string CodigoDef { get; set; }
        public Int64? IdPedidoCompra { get; set; }
        public string CodigoPedidoCompra { get; set; }
        public Int64? IdPedidoCompraFatura { get; set; }
        public string CodigoFatura { get; set; }
        public Int64? IdPedidoCompraNotaFiscal { get; set; }
        public string NumeroNotaFiscalPedidoCompra { get; set; }
        public Int64? IdPedidoInterno { get; set; }
        public string CodigoPedidoInterno { get; set; }
        public Int64? IdCliente { get; set; }
        public string NomeCliente { get; set; }
        public Int64? IdFaturamento { get; set; }
        public string NumeroNotaFiscalFaturamento { get; set; }
        public Int64? IdFornecedorBeneficiario { get; set; }
        public Int64? IdUsuarioBeneficiario { get; set; }
        public DateTime DataLancamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public double Valor { get; set; }
        public bool PagamentoEfetuado { get; set; }
        public DateTime? DataPagamentoEfetuado { get; set; }
        public Int64? IdUsuarioInformouPagamento { get; set; }


        [ForeignKey("IdFornecedorBeneficiario")]
        public virtual Fornecedor Fornecedor { get; set; }
        
        [ForeignKey("IdUsuarioBeneficiario")]
        public virtual Usuario UsuarioBeneficiario { get; set; }

        [ForeignKey("IdUsuarioInformouPagamento")]
        public virtual Usuario UsuarioPagamento { get; set; }

        [ForeignKey("IdObra")]
        public virtual Obra Obra { get; set; }

        [ForeignKey("IdFluxoCaixaPai")]
        public virtual FluxoCaixa FluxoCaixaPai { get; set; }

        public virtual ICollection<FluxoCaixa> FluxosCaixaFilhos { get; set; } = new List<FluxoCaixa>();
    }
}
