using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraNotaFiscalMateriais
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompraNotaFiscal { get; set; }
        public Int64 IdPedidoCompraMateriais { get; set; }
        public double Quantidade { get; set; }
        public double Valor { get; set; }

        [ForeignKey("IdPedidoCompraNotaFiscal")]
        public virtual PedidoCompraNotaFiscal NotaFiscal { get; set; }
    }
}
