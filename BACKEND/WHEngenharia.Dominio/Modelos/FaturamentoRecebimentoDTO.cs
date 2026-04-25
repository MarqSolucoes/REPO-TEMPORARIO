using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class FaturamentoRecebimentoDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdFaturamento { get; set; }
        public DateTime DataRecebimento { get; set; }
        public double ValorRecebido { get; set; }
    }
}
