using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoCompraNotaFiscalMateriaisDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompraNotaFiscal { get; set; }
        public Int64 IdPedidoCompraMateriais { get; set; }
        public double Quantidade { get; set; }
        public double Valor { get; set; }
    }
}
