using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraMateriaisCotacao
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompraMaterial { get; set; }
        public Int64 IdFornecedor { get; set; }
        public Int64 IdCondicaoPagamento { get; set; }
        public double QuantidadeCotado { get; set; }
        public double ValorUnitarioCotado { get; set; }
        public double ValorUnitarioComDesconto { get; set; }
        public DateTime? DataEntrega { get; set; }
        public bool CotacaoFinal { get; set; }
        public bool CotacaoMaisBarata { get; set; }
        public double? Frete { get; set; }
        public double? Imposto { get; set; }
        public double? Desconto { get; set; }


        [ForeignKey("IdSolicitacaoCompraMaterial")]
        public SolicitacaoCompraMateriais Material { get; set; }

        [ForeignKey("IdFornecedor")]
        public virtual Fornecedor Fornecedor { get; set; }

        [ForeignKey("IdCondicaoPagamento")]
        public virtual CondicaoPagamento CondicaoPagamento { get; set; }

        public List<SolicitacaoCompraMaterialCotacaoPagamentoManual> PagamentoManual { get; set; }
    }
}
