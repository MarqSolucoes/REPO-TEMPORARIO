using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WHEngenharia.SQL.Modelos;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_ReaberturaDTO
    {
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdObraSelecionada { get; set; }
        public DateTime DataEntrega { get; set; }

        public List<SolicitacaoCompra_Reabertura_MaterialDTO> Materiais { get; set; }
    }
}
