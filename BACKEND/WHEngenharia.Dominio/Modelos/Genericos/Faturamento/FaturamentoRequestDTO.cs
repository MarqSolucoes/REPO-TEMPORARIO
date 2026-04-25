using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class FaturamentoRequestDTO
    {
        public Int64? IdObra { get; set; }
        public Int64? IdCliente { get; set; }
        public Int64 IdStatusFaturamento { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public string Observacao { get; set; }
        public DateTime? DataFaturamentoInicial { get; set; }
        public DateTime? DataFaturamentoFinal { get; set; }
        public DateTime? DataRecebimentoInicial { get; set; }
        public DateTime? DataRecebimentoFinal { get; set; }
    }
}
