using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.SQL.Modelos
{
    public class PedidoCompraFatura
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public string CodigoFormatado { get; set; }
        public DateTime DataFatura { get; set; }
        public double Valor { get; set; }
        public bool PagamentoEfetuado { get; set; }

        [ForeignKey("IdPedidoCompra")]
        public virtual PedidoCompra PedidoCompra { get; set; }
    }
}
