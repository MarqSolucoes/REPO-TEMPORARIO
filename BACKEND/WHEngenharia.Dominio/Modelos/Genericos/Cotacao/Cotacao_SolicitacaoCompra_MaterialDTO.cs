using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Cotacao
{
    public class Cotacao_SolicitacaoCompra_MaterialDTO
    {
        public Int64 IdSolicitacaoCompraMaterial { get; set; }
        public Int64 IdMaterial { get; set; }
        public string Descricao { get; set; }
        public double Quantidade { get; set; }
        public double? ValorUnitarioEstimado { get; set; }
        public bool ItemSelecionado { get; set; }
        public UnidadeMaterialDTO UnidadeMaterial { get; set; }

        public List<Cotacao_SolicitacaoCompra_Material_CotacaoDTO> Cotacoes { get; set; }
    }
}
