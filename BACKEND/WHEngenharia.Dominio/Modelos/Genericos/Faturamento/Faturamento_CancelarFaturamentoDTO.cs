using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_CancelarFaturamentoDTO
    {
        public Int64 idFaturamento { get; set; }
        public Int64 idUsuario { get; set; }
        public DateTime dataRetornoFaturamento { get; set; }
    }
}
