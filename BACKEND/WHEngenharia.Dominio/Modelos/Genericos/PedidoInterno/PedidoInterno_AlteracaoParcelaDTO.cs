using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.PedidoInterno
{
    public class PedidoInterno_AlteracaoParcelaDTO
    {
        public Int64 IdParcela { get; set; }
        public DateTime DataPagamento { get; set; }
        public double Valor { get; set; }
    }
}
