using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_InformarFaturamentoDTO
    {
        public Int64 idFaturamento { get; set; }
        public DateTime dataRecebimento { get; set; }
        public double valorRecebido { get; set; }
        public double valorRestante {  get; set; }
        public DateTime? dataProximoRecebimento { get; set; }
    }
}
