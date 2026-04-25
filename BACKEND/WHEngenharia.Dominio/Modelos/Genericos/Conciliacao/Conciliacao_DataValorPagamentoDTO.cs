using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public  class Conciliacao_DataValorPagamentoDTO
    {
        public int id { get; set; }
        public DateTime data { get; set; }
        public double valor { get; set; }
    }
}
