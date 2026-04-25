using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Cotacao
{
    public class Cotacao_SolicitacaoCompra_Material_CotacaoDTO
    {
        public Int64 IdSolicitacaoCompraMaterialCotacao { get; set; }
        public Int64 IdFornecedor { get; set; }
        public string NomeFantasia { get; set; }
        public double ValorUnitarioCotado { get; set; }
        public double QuantidadeCotado { get; set; }
        public DateTime? DataEntrega { get; set; }
        public bool CotacaoFinal { get; set; }
        public bool CotacaoMaisBarata { get; set; }
    }
}
