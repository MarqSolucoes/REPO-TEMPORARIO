using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_Post
    {
        public Int64 IdStatusSolicitacaoCompra { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }
        public Int64 IdFornecedor { get; set; }
        public string Observacao { get; set; }
        public double ValorEstimado { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataAprovacaoEngenheiro { get; set; }
        public string Nome { get; set; }
        public bool Servico { get; set; }
        public Int64? IdUsuarioComprador { get; set; }
        public Int64 IdUsuarioCadastro { get; set; }
        public Int64? IdEngenheiroAprovador { get; set; }
        public DateTime DataCadastro { get; set; }
        public Int64 IdUsuarioAlteracao { get; set; }
        public DateTime DataUltimaAlteracao { get; set; }

        public List<SolicitacaoCompraMateriaisDTO> Materiais { get; set; }
        public List<SolicitacaoCompra_PostPagamentos> Pagamentos { get; set; }
    }
}
