using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraMateriais
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdMaterial { get; set; }
        public double Quantidade { get; set; }
        public double? ValorUnitarioEstimado { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }

        [ForeignKey("IdSolicitacaoCompra")]
        public virtual SolicitacaoCompra SolicitacaoCompra { get; set; }
        
        [ForeignKey("IdMaterial")]
        public virtual Material Material { get; set; }
        
        [ForeignKey("IdTipoCentroCusto")]
        public virtual TipoCentroCusto TipoCentroCusto { get; set; }

        public List<SolicitacaoCompraMateriaisCotacao> Cotacoes { get; set; }
    }
}
