using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos
{
    public class PedidoCompraFaturaDTO
    {
        public Int64 Id { get; set; }
        public Int64 IdPedidoCompra { get; set; }
        public string CodigoFormatado { get; set; }
        public DateTime DataFatura { get; set; }
        public double Valor { get; set; }
        public bool PagamentoEfetuado { get; set; }
    }
}
