using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WHEngenharia.Dominio.Modelos.Genericos
{
    public class Financeiro_EntradaDTO
    {
        public Int64? IdPedidoCompra { get; set; }
        public Int64? IdPedidoCompraNotaFiscal { get; set; }
        public Int64? IdPedidoInterno { get; set; }
        public string Descricao { get; set; }
        public string PedidoCompra { get; set; }
        public string PedidoInterno { get; set; }
        public string Fornecedor { get; set; }
        public string Cliente { get; set; }
        public string Obra { get; set; }
        public string DEF { get; set; }
        public string NomeNF { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public string DataVencimento { get; set; }
        public double Valor { get; set; }

        public string MotivoRecusa { get; set; }
    }
}
