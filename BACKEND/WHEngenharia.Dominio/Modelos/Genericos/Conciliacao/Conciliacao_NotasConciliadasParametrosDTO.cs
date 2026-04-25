using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Conciliacao
{
    public class Conciliacao_NotasConciliadasParametrosDTO
    {
        public List<Int64>? IdsFornecedores { get; set; }
        public string NumeroPedido { get; set; }
        public string NumeroNotaFiscal { get; set; }
        public double? Valor { get; set; }
        public DateTime? DataInicial { get; set; }
        public DateTime? DataFinal { get; set; }
    }
}
