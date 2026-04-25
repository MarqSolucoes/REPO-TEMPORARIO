using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.SolicitacaoCompra
{
    public class SolicitacaoCompra_RequestParametrosDTO
    {
        public List<Int64> idsObra { get; set; } = new List<Int64>();
        public List<Int64> idsStatus { get; set; } = new List<Int64>();
        public string codigo { get; set; }
        public string titulo { get; set; }
        public DateTime? dataSolicitacaoInicial { get; set; }
        public DateTime? dataSolicitacaoFinal { get; set; }

        public int skip { get; set; }
        public int take { get; set; }
    }
}
