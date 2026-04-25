using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos
{
    public class SolicitacaoPedidoCompraQuantidadesDTO
    {
        public Int64 qtdParaValidacao { get; set; }
        public Int64 qtdEmCotacao { get; set; }
        public Int64 qtdParaAprovacao { get; set; }
        public Int64 qtdDevolvidasDiretoria { get; set; }
        public Int64 qtdEmCompra { get; set; }
        public Int64 qtdPedidosFinalizados { get; set; }
        public Int64 qtdPedidosCancelados { get; set; }
        public Int64 qtdSolicitacoesFinalizadas { get; set; }
        public Int64 qtdSolicitacoesCanceladas { get; set; }
    }
}
