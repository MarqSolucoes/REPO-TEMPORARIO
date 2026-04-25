using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_Reabertura_MaterialDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdMaterial { get; set; }
        public double Quantidade { get; set; }
        public double? ValorUnitario { get; set; }

        public List<SolicitacaoCompraMateriaisCotacaoDTO> Cotacoes { get; set; }
    }
}
