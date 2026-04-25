using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.Dominio.Modelos;

namespace WHEngenharia.SQL.Modelos
{
    public class SolicitacaoCompraMateriaisDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdMaterial { get; set; }
        public double Quantidade { get; set; }
        public double? ValorUnitarioEstimado { get; set; }
        public Int64 IdTipoCentroCusto { get; set; }
        public Int64? IdCentroCustoObra { get; set; }
        public Int64? IdCentroCustoDEF { get; set; }

        public MaterialDTO Material { get; set; }

        public List<SolicitacaoCompraMateriaisCotacaoDTO> Cotacoes { get; set; }
    }
}
