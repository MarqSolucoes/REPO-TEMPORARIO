using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.PedidoCompra
{
    public class PedidoCompra_NotaFiscalParaAprovacaoDTO
    {
        public PedidoCompraNotaFiscalDTO pedidoCompraNotaFiscal { get; set; }
        public float saldoPedido { get; set; }
        public string datasPagamento { get; set; }
        public bool vencimentoProximo { get; set; }
        public double valorPedido { get; set; }
    }
}
