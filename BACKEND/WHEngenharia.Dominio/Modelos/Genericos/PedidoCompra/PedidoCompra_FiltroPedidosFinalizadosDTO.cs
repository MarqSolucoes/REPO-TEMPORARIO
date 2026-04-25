using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos.PedidoCompra
{
    public class PedidoCompra_FiltroPedidosFinalizadosDTO
    {
        public List<Int64> idsObra { get; set; }
        public List<Int64> idsCliente { get; set; }
        public List<Int64> idsMateriais { get; set; }
        public List<Int64> idsFornecedor { get; set; }
        public List<Int64> idsPedidoCompra { get; set; }
        public DateTime? dataSolicitacaoInicial { get; set; }
        public DateTime? dataSolicitacaoFinal { get; set; }
    }
}
