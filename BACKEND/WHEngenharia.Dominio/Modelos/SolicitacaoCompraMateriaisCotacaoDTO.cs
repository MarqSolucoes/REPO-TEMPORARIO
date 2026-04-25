using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos.Genericos;

namespace WHEngenharia.Dominio.Modelos
{
    public class SolicitacaoCompraMateriaisCotacaoDTO
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

        public FornecedorDTO Fornecedor { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }

        public List<SolicitacaoCompraMaterialCotacaoPagamentoManualDTO> PagamentoManual { get; set; }
    }
}
