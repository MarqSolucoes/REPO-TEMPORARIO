using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.Faturamento
{
    public class Faturamento_DetalheClienteDTO
    {
        public int NumeroObrasAtivas { get; set; }
        public double ValorTotalObrasAtivas { get; set; }
        public double ValorAFaturarObrasAtivas { get; set; }
        public double ValorFaturadoAReceberObrasAtivas { get; set; }
        public double ValorFaturadoRecebidoObrasAtivas { get; set; }
    }
}
