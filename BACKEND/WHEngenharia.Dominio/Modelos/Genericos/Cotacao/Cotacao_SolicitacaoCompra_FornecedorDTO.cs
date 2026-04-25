using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Cotacao
{
    public class Cotacao_SolicitacaoCompra_FornecedorDTO
    {
        public Int64 Id { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
        public string CNPJ { get; set; }
        public double TotalCotado { get; set; }
        public double? Frete { get; set; }
        public double? Imposto { get; set; }
        public double? Desconto { get; set; }
        public CondicaoPagamentoDTO CondicaoPagamento { get; set; }
        public List<SolicitacaoCompraMaterialCotacaoPagamentoManualDTO> PagamentoManual { get; set; }
    }
}
