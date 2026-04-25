using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_ObjetoAprovacaoDTO
    {
        public Int64 IdSolicitacaoCompra { get; set; }
        public Int64 IdUsuarioFinalizacaoCotacao { get; set; }
        public Int64? IdDiretorAprovador { get; set; }
        public string ObservacaoDeAprovacao { get; set; }
        public DateTime DataAprovacaoDiretor { get; set; }
    }
}
