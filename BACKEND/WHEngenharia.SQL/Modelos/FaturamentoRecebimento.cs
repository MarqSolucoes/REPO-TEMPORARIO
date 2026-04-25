using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class FaturamentoRecebimento
    {
        public Int64 Id { get; set; }
        public Int64 IdFaturamento { get; set; }
        public DateTime DataRecebimento { get; set; }
        public double ValorRecebido { get; set; }
    }
}
